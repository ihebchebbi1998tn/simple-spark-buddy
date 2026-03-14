# Moment.js for date/time operations

## Preamble ##
Moment.js is exclusively used for date/time operations. Formatting date/time strings culture depending is not scope of moment.js.

You can find a short introduction with a few examples in this document.

## Reference ##
You can find detailed development docs on [http://momentjs.com](http://momentjs.com).

## Examples ##

### Now ###
	var now = moment();
This is essentially the same as calling moment(new Date()).

### Add ###

	moment().add(7, 'days');

There are some shorthand keys.
	
	moment().add(7, 'd');	// d = days
	
**Note:** Please do **not** use shorthand keys for better readability.

### Start of Time ###
	moment().startOf(String);
Mutates the original moment by setting it to the start of a unit of time.

	moment().startOf('year');    // set to January 1st, 12:00 am this year
	moment().startOf('month');   // set to the first of this month, 12:00 am
	moment().startOf('week');    // set to the first day of this week, 12:00 am
	moment().startOf('day');     // set to 12:00 am today
	
### Format ###
	moment().format(String);

Format can be used for static date/time formats.

	moment().format("YYYY-MM-DD");

**Note:** Do not use moment.js for globalization!