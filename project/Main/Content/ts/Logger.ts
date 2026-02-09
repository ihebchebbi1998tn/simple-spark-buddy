import log4javascript from "log4javascript";
log4javascript.setShowStackTraces(true);

const Log = log4javascript.getLogger("main");
Log.addAppender(new log4javascript.BrowserConsoleAppender());
Log.setLevel(log4javascript.Level.INFO);

export default Log;