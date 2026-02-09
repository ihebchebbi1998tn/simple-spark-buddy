// default log settings
import log4javascript from "log4javascript";

log4javascript.setShowStackTraces(true);
window.Log = log4javascript.getLogger("main");

export class SignalRAppender extends log4javascript.Appender{
	constructor() {
		super();
		this.setLayout(new log4javascript.JsonLayout());
		this.setThreshold(log4javascript.Level.DEBUG);

		var offlineLogs = [];

		this.append = function (loggingEvent) {
			var appender = this;

			var getFormattedMessage = function () {
				var layout = appender.getLayout();
				var formattedMessage = layout.format(loggingEvent);
				if (layout.ignoresThrowable() && loggingEvent.exception) {
					formattedMessage += loggingEvent.getThrowableStrRep();
				}
				return formattedMessage;
			};

			if ($.connection.ProfilingHub.state === "Connected") {
				$.connection.ProfilingHub.invoke("log", loggingEvent.level.toString(), getFormattedMessage());
			} else if (loggingEvent.level.toString() == 'WARN' || loggingEvent.level.toString() == 'ERROR') {
				offlineLogs.push({ level: loggingEvent.level.toString(), message: getFormattedMessage().toString() });
			}
		};

		$(function () {
			if ($.connection && $.connection.ProfilingHub) {
				$($.connection.ProfilingHub).on("stateChanged", function() {
					if ($.connection.ProfilingHub.state === "Connected") {
						var oldOfflineLogs = offlineLogs;
						offlineLogs = [];
						oldOfflineLogs.forEach(function(offlineLog) {
							if (offlineLog.level == 'ERROR') {
								window.Log.error(offlineLog.message);
							} else if (offlineLog.level == 'WARN') {
								window.Log.warn(offlineLog.message);
							}
						});
					}
				});
			}
		});

		this.toString = function () {
			return 'SignalRAppender (' + window.Log.getLevel().toString() + ')';
		};
	}
}
window.SignalRAppender = new SignalRAppender();

// logging application cache events
$(function () {
	// @ts-ignore
	var cache = window.jakeCache || (navigator.userAgent.indexOf("AppBoundDomains") === -1 ? window.applicationCache : null);
	if (cache && cache.addEventListener) {
		var cacheStatusValues = [];
		cacheStatusValues[0] = 'uncached';
		cacheStatusValues[1] = 'idle';
		cacheStatusValues[2] = 'checking';
		cacheStatusValues[3] = 'downloading';
		cacheStatusValues[4] = 'updateready';
		cacheStatusValues[5] = 'obsolete';

		function logEvent(e) {
			var online, status, type, message;
			online = (navigator.onLine) ? 'yes' : 'no';
			status = cacheStatusValues[cache.status];
			type = e.type;
			message = 'applicationCacheEvent: online: ' + online;
			message += ', event: ' + type;
			message += ', status: ' + status;
			if (type == 'error' && navigator.onLine) {
				message += ' (prolly a syntax error in manifest)';
			}
			window.Log.debug(message);
		}
		
		cache.addEventListener('cached', logEvent, false);
		cache.addEventListener('checking', logEvent, false);
		cache.addEventListener('downloading', logEvent, false);
		cache.addEventListener('error', logEvent, false);
		cache.addEventListener('noupdate', logEvent, false);
		cache.addEventListener('obsolete', logEvent, false);
		cache.addEventListener('progress', logEvent, false);
		cache.addEventListener('updateready', logEvent, false);
	}
});
// logging jaydata queries
;(function($data: any){
	if (!$data || !$data.storageProviders.sqLitePro)
	{
		return;
	}
	function logQuery(queries, params) {
		if (!Array.isArray(queries)){
			queries = [queries]
			params = [params];
		}
		var logs = [];
		for (let i = 0; i < queries.length; i++) {
			var query = queries[i];
			if (!query){
				continue;
			}
			var j = 0;
			query = query.replace(/\?/g, function(m, pos, str){
				var r = typeof params[i][j] == "string" ? "'" + params[i][j] + "'" : params[i][j];
				j++;
				return r;
			});
			logs.push(query);
		}
		
		window.Log.debug(logs);
	}
	let origExec = $data.dbClient.openDatabaseClient.OpenDbCommand.prototype.exec;
	$data.dbClient.openDatabaseClient.OpenDbCommand.prototype.exec = function(query, parameters, callback, errorhandler, transaction, isWrite){
		logQuery(query, parameters);
		return origExec.apply(this, arguments);
	}
})(window.$data);
// log knockout errors
;(function(ko: any){
	if (!ko)
	{
		return;
	}
	ko.onError ||= function(error) {
		window.Log.error("knockout error", error);
		window.swal(window.Helper.String.getTranslatedString("Error"), (error as Error).message || error?.message, "error");
	};
})(window.ko);