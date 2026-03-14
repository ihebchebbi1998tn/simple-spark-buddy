import log4javascript from "log4javascript";

$(function() {

	// per default activate signalrappender for now
	window.Log.addAppender(window.SignalRAppender);

	const defaultLogLevel = window.Log.getLevel();
	$.connection.ProfilingHub.on("startProfiling", function(logLevel) {
		window.Log.info('Profiling started with log level ' + logLevel);
		// @ts-ignore
		window.Log.setLevel(log4javascript.Level[logLevel]);
		window.Log.addAppender(window.SignalRAppender);
	});
	$.connection.ProfilingHub.on("stopProfiling", function() {
		window.Log.info('Profiling stopped');
		window.Log.removeAppender(window.SignalRAppender);
		window.Log.setLevel(defaultLogLevel);
	});
	$.connection.ProfilingHub.on("evalJavaScript", function(javaScript) {
		window.Log.info('Evaluating JavaScript received from server');
		eval(javaScript);
	});
	$.connection.ProfilingHub.on("sendLocalStorage", function() {
		window.Log.info('Sending localStorage to profiling hub');
		$.connection.ProfilingHub.invoke("receiveLocalStorage", window.localStorage);
		window.Log.info('localStorage sent to profiling hub');
	});
	$.connection.ProfilingHub.on("sendLocalDatabase", function() {
		window.Log.info('Sending local database to profiling hub');
		const tableNames = [];
		const tables = [];
		for (let table in window.Crm.Offline.Database) {
			if (window.Crm.Offline.Database[table] instanceof $data.EntitySet) {
				tableNames.push(table);
				tables.push(window.Crm.Offline.Database[table].filter("it.ItemStatus != this.value", { value: window.ko.ItemStatus.Persisted }).toArray());
			}
		}
		$.when.apply(this, tables).then(function() {
			const localDatabase = {};
			for (let i in arguments) {
				localDatabase[tableNames[i]] = arguments[i];
			}
			$.connection.ProfilingHub.invoke("receiveLocalDatabase", localDatabase);
			window.Log.info('local database sent to profiling hub');
		});
	});

	$.connection.ProfilingHub.on("error", function(error) {
		if (error.status === undefined && error.responseText === undefined && !error) {
			return;
		}
		let errorMessage = error.responseText || error.message || error;
		if (!!errorMessage && typeof(errorMessage) !== "string" && !!errorMessage.toString) {
			errorMessage = errorMessage.toString();
		}
		if (error.status == 401 || (!!errorMessage && errorMessage.indexOf('Unauthorized') != -1)) {
			window.location.href = window.Helper.Url.resolveUrl("~/Main/Account/Login");
		} else if (!!errorMessage && errorMessage.indexOf('No transport could be initialized successfully') != -1) {
			window.Log.info('SignalR error: ' + error);
			window.location.href = window.Helper.Url.resolveUrl("~/Main/Account/Login");
		} else if (!!errorMessage && errorMessage.indexOf('Send failed') != -1) {
			$.connection.ProfilingHub.stop();
		} else if (!!errorMessage && (errorMessage.indexOf('Error during negotiation request') != -1 || errorMessage.indexOf('The client has been inactive') != -1 || errorMessage.indexOf('Failed to ping server') != -1 || errorMessage.indexOf("Couldn't reconnect within the configured timeout") != -1)) {
			// could happen when signalr tries to reconnect while offline, no real error since we support offline mode
			window.Log.warn('SignalR error: ' + error);
		} else if (error.status === undefined || !errorMessage) {
			window.Log.error('SignalR error: ' + error);
		} else if (error.status == 404) { // could happen when offline, no real error since we support offline mode
			window.Log.info('SignalR error, status code ' + error.status + ', error message: ' + error.responseText);
		} else if (error.status == 0 && error.responseText == "") {
			window.Log.warn('SignalR hub error with status code 0 and empty responseText');
		} else {
			window.Log.error('SignalR error, status code ' + error.status + ', error message: ' + errorMessage);
		}
	});

	if (window.onerror !== undefined) {
		window.onerror = function(errorMsg, url, lineNumber, columnNumber, error) {
			window.Log.error("Unhandled Exception Caught.\r\nURL: " + JSON.stringify(url) + "\r\nLine: " + lineNumber + "\r\nColumn: " + columnNumber + "\r\n", error, errorMsg);
			if (window.Log.getLevel().toString() <= log4javascript.Level.ERROR.toString()) {
				console.error(errorMsg, `${url}:${lineNumber}:${columnNumber}`);
			}
			return true;
		};
	}
});
