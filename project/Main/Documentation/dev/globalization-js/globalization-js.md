# Our JS globalization story

## Why do we need globalization?
Each language, and the countries that speak that language, have different expectations when it comes to how numbers (including currency and percentages) and dates should appear. Obviously, each language has different names for the days of the week and the months of the year. But they also have different expectations for the structure of dates, such as what order the day, month and year are in. In number formatting, not only does the character used to delineate number groupings and the decimal portion differ, but the placement of those characters differ as well.

A user using an application should be able to read and write dates and numbers in the format they are accustomed to. This library makes this possible, providing an API to convert user-entered number and date strings - in their own format - into actual numbers and dates, and conversely, to format numbers and dates into that string format.

Even if the application deals only with the English locale, it may still need globalization to format programming language bytes into human-understandable language and vice-versa in an effective and reasonable way. For example, to display something better than "Edited 1 minutes ago".

## Reference
[jquery/globalize on github](https://github.com/jquery/globalize)

## General ##
The culture is initialized by the browser's default culture at startup of the application in *Helper.Culture.js*.
For further operations as parsing this culture is used by default. If an other cultures deviating from the loaded culture should be required the specific CLDR data have to be loaded.

[Cldr.js on github](https://github.com/rxaviers/cldrjs#readme)

## Formatting dates and time
### Parameter ###
**value**
Date instance to be formatted, eg. new Date();

####Options####

A JSON object including one of the following.

**skeleton**
String value indicating a skeleton (see description above), eg. { skeleton: "GyMMMd" }.
Skeleton provides a more flexible formatting mechanism than the predefined list full, long, medium, or short represented by date, time, or datetime. Instead, they are an open-ended list of patterns containing only date field information, and in a canonical order.

**date**
One of the following String values: full, long, medium, or short, eg. { date: "full" }.

**time**
One of the following String values: full, long, medium, or short, eg. { time: "full" }.

**datetime**
One of the following String values: full, long, medium, or short, eg. { datetime: "full" }

**raw**
String value indicating a machine raw pattern eg. { pattern: "dd/mm" }.
*Note:*
This is NOT recommended for i18n in general. No globalization can be done by this pattern. Use skeleton instead.

### Examples ###
The following examples demonstrates a comparison of the former globalization version (v0.0.1) to the new one (v1.0.0).
### "01.09." ###
- *Old:* no globalization pattern
- *New:* Globalize.formatDate(new Date(), **{ skeleton: 'MMdd' }** )

### "01.09.15" ###
- *Old:* no globalization pattern
- *New:* Globalize.formatDate(new Date(), **{ date: "short" }** )

### "01.09.2015" ###
- *Old:* Globalize.format(new Date(), "d")
- *New:* Globalize.formatDate(new Date(), **{ date: "medium" }** )

### "Dienstag, 1. September 2015" ###
- *Old:* Globalize.format(new Date(), "D")
- *New:* Globalize.formatDate(new Date(), **{ date: 'full' }** )

### "09:08" ###
- *Old:* Globalize.format(new Date(), "t")
- *New:* Globalize.formatDate(new Date(), **{ time: 'short' }** )

### "Dienstag, 1. September 2015 09:08" ###
- *Old:* Globalize.format(new Date(), "f")
- *New:* Globalize.formatDate(new Date(), **{ skeleton: 'yMMMEdHm' }** )

**Note:**
New format is slightly different: "Di., 1. Sep. 2015, 09:08"

## Formatting numbers
### Parameter ###
**value**
A string number.

#### Options (optional) ####

A JSON object including none or any of the following options.

**style** Optional
String decimal (default), or percent.

**minimumIntegerDigits** Optional
Non-negative integer Number value indicating the minimum integer digits to be used. Numbers will be padded with leading zeroes if necessary.

**minimumFractionDigits and maximumFractionDigits** Optional
Non-negative integer Number values indicating the minimum and maximum fraction digits to be used. Numbers will be rounded or padded with trailing zeroes if necessary. Either one or both of these properties must be present. If they are, they will override minimum and maximum fraction digits derived from the CLDR patterns.

**minimumSignificantDigits and maximumSignificantDigits** Optional
Positive integer Number values indicating the minimum and maximum fraction digits to be shown. Either none or both of these properties are present. If they are, they override minimum and maximum integer and fraction digits. The formatter uses however many integer and fraction digits are required to display the specified number of significant digits.

**round** Optional
String with rounding method ceil, floor, round (default), or truncate.

**useGrouping** Optional
Boolean (default is true) value indicating whether a grouping separator should be used.

### Plain Numbers ###
Globalize.formatNumber(1234.567)
// > "1.234,567"

Globalize.formatNumber(1234.567, {minimumSignificantDigits: 3, maximumSignificantDigits : 5})
// > "1.234,6"

Globalize.formatNumber(12, {minimumSignificantDigits: 3, maximumSignificantDigits : 5})
// > "12,0"

Globalize.formatNumber(1234.567, {minimumFractionDigits: 1, maximumFractionDigits : 2})
// > "1.234,57"

Globalize.formatNumber(1234, {minimumFractionDigits: 1, maximumFractionDigits : 2})
// > "1.234,0"

### Percentage ###
Globalize.formatNumber(0.0123456789,{style: "percent",minimumSignificantDigits: 3, maximumSignificantDigits: 5})
// > "1,2346 %"

## Parsing dates
Return a function that parses a string representing a date into a JavaScript Date object according to the given options. The default parsing assumes numeric year, month, and day (i.e., { skeleton: "yMd" }).
The returned function is invoked with one argument: the String value to be parsed.

### Parameters ###
**value**
String with date to be parsed, eg. "11/1/10, 5:55 PM"

#### Options (optional) ####
See .dateFormatter( [options] ).


### Examples ###
As described above, by default the initialized culture is used for parsing. If another culture is required it has to be loaded by Cldr.

Globalize.parseDate("01.02.2015")
// > Sun Feb 01 2015 00:00:00 GMT+0100 (Mitteleuropäische Zeit)

## Parsing numbers
Return a function that parses a String representing a number according to the given options. If value is invalid, NaN is returned.
The returned function is invoked with one argument: the String representing a number value to be parsed.

### Parameters ###
**value**
String with number to be parsed, eg. "3.14".

#### Options (optional) ####
A JSON object including none or any of the following options.

**style** Optional
String decimal (default), or percent.

### Examples ###
Globalize.parseNumber("12.735,00")
// > 12735

Globalize.parseNumber("6.626E-34")
// > 6.626e-31

## Calendar constants ##
Globalize.cldr.main("dates").fields.day["relative-type-0"]
// > "heute"

Globalize.cldr.main("dates").fields.month.displayName
// > "Monat"

Globalize.cldr.main("dates").fields.week.displayName
// > "Woche"

Globalize.cldr.main("dates").fields.day.displayName
// > "Tag"