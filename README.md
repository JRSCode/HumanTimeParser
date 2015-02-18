# HumanTimeParser
C# class for parsing human input as strings into a TimeSpan object.
Both English and German supported.

Usage:
* Add the class to an existing project, then use like any other.

Examples for parseable input:
* "I would like to wait for 3m 44sec."
* "Movie begins in 3 days 2h 44 sec."
* "5d and 89.5 hours"
* "3s 44 hours"
* "1.5 hrs. and 50 seconds"

Example code:
```C#
   HumanTimeParser htp = new HumanTimeParser();
   TimeSpan ts = htp.Parse("60 seconds", HumanTimeParserLanguage.English);
```

English keywords: ms, millisecond, milliseconds, s, sec, second, seconds, m, min, minute, minutes, h, hrs, hour, hours, d, day, days

German keywords: ms, millisekunde, millisekunden, s, sek, sekunde, sekunden, m, minute, minuten, St, Std, stunde, stunden, d, tag, tage
