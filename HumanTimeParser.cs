using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TimeParser {

    /*HumanTimeSpanParser - Parsing human input strings into a TimeSpan object
    Copyright (C) 2015 JRSCode

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

    class HumanTimeParser {
        public enum HumanTimeParserLanguage {
            English = 0, German = 1
        };

        /// <summary>
        /// Parses a string for time declarations and returns a TimeSpan object.
        /// Object will have a time span of zero seconds if nothing can be found.
        /// Ignores multiples of the same unit.
        /// </summary>
        /// <param name="input">Contains time declarations in whatever order.</param>
        /// <param name="language">Switches between available languages.</param>
        /// <returns></returns>
        public TimeSpan Parse(string input, HumanTimeParserLanguage language) {
            //Arrays containing regex strings for each language, 0=english, 1=german
            string[] millisecondStrings = new string[] { @"(\d+((\.|\,)\d+)?)(?=\s?(ms|(?i)milliseconds?))(?!\w)", @"(\d+((\.|\,)\d+)?)(?=\s?(ms|(?i)millisekunden?))(?!\w)" };
            string[] secondStrings = new string[] { @"(\d+((\.|\,)\d+)?)(?=\s?(s|(?i)sec(onds?)?)(?!\w))", @"(\d+((\.|\,)\d+)?)(?=\s?(s|(?i)sek(unden?)?)(?!\w))" };
            string[] minuteStrings = new string[] { @"(\d+((\.|\,)\d+)?)(?=\s?(m|(?i)min(utes?)?)(?!\w))", @"(\d+((\.|\,)\d+)?)(?=\s?(m|(?i)min(uten?)?)(?!\w))" };
            string[] hourStrings = new string[] { @"(\d+((\.|\,)\d+)?)(?=\s?(h(rs)?|(?i)hours?)(?!\w))", @"(\d+((\.|\,)\d+)?)(?=\s?((h|Std?\.)|(?i)(stunden?))(?!\w))" };
            string[] dayStrings = new string[] { @"(\d+((\.|\,)\d+)?)(?=\s?(d|(?i)days?)(?!\w))", @"(\d+((\.|\,)\d+)?)(?=\s?(d|(?i)tag(en?))(?!\w))" };

            Regex regexMilliseconds = new Regex(millisecondStrings[(int)language]);
            Regex regexSeconds = new Regex(secondStrings[(int)language]);
            Regex regexMinutes = new Regex(minuteStrings[(int)language]);
            Regex regexHours = new Regex(hourStrings[(int)language]);
            Regex regexDays = new Regex(dayStrings[(int)language]);

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            NumberStyles numStyle = NumberStyles.AllowDecimalPoint;
            TimeSpan ts = new TimeSpan();
            string msMatch, sMatch, mMatch, hMatch, dMatch;
            int milliseconds = 0, seconds = 0, minutes = 0, hours = 0, days = 0;
            decimal dMilliseconds = 0, dSeconds = 0, dMinutes = 0, dHours = 0, dDays = 0;

            //Getting first matches for each time unit.
            //Adding up multiple matches for the same unit does not fit this class's purpose.
            //
            //I also would like to accept both , and . as decimal point, regardless of culture,
            //so I set the culture to en-US and correct the strings to only contain .'s
            msMatch = regexMilliseconds.Match(input).Value.Replace(',', '.');
            sMatch = regexSeconds.Match(input).Value.Replace(',', '.');
            mMatch = regexMinutes.Match(input).Value.Replace(',', '.');
            hMatch = regexHours.Match(input).Value.Replace(',', '.');
            dMatch = regexDays.Match(input).Value.Replace(',', '.');

            //10,000 ticks in a millisecond
            if (msMatch.Contains(",") || msMatch.Contains(".")) {
                if (Decimal.TryParse(msMatch, numStyle, culture, out dMilliseconds)) {
                    ts = ts.Add(new TimeSpan((long)(dMilliseconds * 10000)));
                }
            } else {
                if (Int32.TryParse(msMatch, out milliseconds)) {
                    ts = ts.Add(new TimeSpan(0, 0, 0, 0, milliseconds));
                }
            }

            if (sMatch.Contains(",") || sMatch.Contains(".")) {
                if (Decimal.TryParse(sMatch, numStyle, culture, out dSeconds)) {
                    ts = ts.Add(new TimeSpan((long)(dSeconds * 10000 * 1000)));
                }
            } else {
                if (Int32.TryParse(sMatch, out seconds)) {
                    ts = ts.Add(new TimeSpan(0, 0, 0, seconds));
                }
            }

            if (mMatch.Contains(",") || mMatch.Contains(".")) {
                if (Decimal.TryParse(mMatch, numStyle, culture, out dMinutes)) {
                    ts = ts.Add(new TimeSpan((long)(dMinutes * 10000 * 1000 * 60)));
                }
            } else {
                if (Int32.TryParse(mMatch, out minutes)) {
                    ts = ts.Add(new TimeSpan(0, 0, minutes, 0));
                }
            }

            if (hMatch.Contains(",") || hMatch.Contains(".")) {
                if (Decimal.TryParse(hMatch, numStyle, culture, out dHours)) {
                    ts = ts.Add(new TimeSpan((long)(dHours * 10000 * 1000 * 60 * 60)));
                }
            } else {
                if (Int32.TryParse(hMatch, out hours)) {
                    ts = ts.Add(new TimeSpan(0, hours, 0, 0));
                }
            }

            if (dMatch.Contains(",") || dMatch.Contains(".")) {
                if (Decimal.TryParse(dMatch, numStyle, culture, out dDays)) {
                    ts = ts.Add(new TimeSpan((long)(dDays * 10000 * 1000 * 60 * 60 * 24)));
                }
            } else {
                if (Int32.TryParse(dMatch, out days)) {
                    ts = ts.Add(new TimeSpan(days, 0, 0, 0));
                }
            }

            return ts;
        }
    }
}
