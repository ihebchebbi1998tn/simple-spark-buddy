import * as signalR from "@microsoft/signalr";
import log4javascript, {Level} from "log4javascript";
import Log from "./Logger";
import {HelperUrl} from "./helper/Helper.Url";


export default class SignalrProfilingHub {
	reconnectDelay = 5000;
	connection: signalR.HubConnection;
	connectionStatus: signalR.HubConnectionState;

	constructor() {
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(HelperUrl.resolveUrl("~/profilingHub"))
			.withAutomaticReconnect({
				nextRetryDelayInMilliseconds: (retryContext) => {
					if (retryContext.retryReason.message.indexOf("Unauthorized") !== -1){
						window.location.href = window.Helper.Url.resolveUrl("~/Main/Account/Login");
					}
					return this.reconnectDelay;
				}
			})
			.build();
		this.setupSignalrLogger();
		window.addEventListener("beforeunload", () => void this.connection.stop());
	}

	async start(): Promise<void> {
		try {
			await this.connection.start();
		} catch (error) {
			const errorMessage = ((error as Error).message || error);
			Log.error(errorMessage);
		}
	}

	private createSignalerAppender(): log4javascript.Appender {
		const signalRAppender = new log4javascript.Appender();
		signalRAppender.setLayout(new log4javascript.JsonLayout());
		signalRAppender.setThreshold(log4javascript.Level.INFO);
		let offlineLogs: { level: keyof typeof log4javascript.Level | string, message: string }[] = [];
		signalRAppender.append = (loggingEvent) => {
			const getFormattedMessage = () => {
				const layout = signalRAppender.getLayout();
				let formattedMessage = layout.format(loggingEvent);
				if (layout.ignoresThrowable() && loggingEvent.exception) {
					formattedMessage += loggingEvent.getThrowableStrRep();
				}
				return formattedMessage;
			};

			if (this.connection.state === "Connected") {
				this.connection.invoke("log", loggingEvent.level.toString(), getFormattedMessage());
			} else if (loggingEvent.level.toString() === "WARN" || loggingEvent.level.toString() === "ERROR") {
				offlineLogs.push({
					level: loggingEvent.level.toString(),
					message: getFormattedMessage().toString()
				});
			}
		};

		this.connection.on("stateChanged", () => {
			if (this.connection.state === "Connected") {
				const oldOfflineLogs = offlineLogs;
				offlineLogs = [];
				oldOfflineLogs.forEach(offlineLog => {
					if (offlineLog.level === "ERROR") {
						Log.error(offlineLog.message);
					} else if (offlineLog.level === "WARN") {
						Log.warn(offlineLog.message);
					}
				});
			}
		});


		signalRAppender.toString = function () {
			return `SignalRAppender (${log4javascript.Level[Log.getLevel()]})`;
		};

		return signalRAppender;
	}

	private setupSignalrLogger(): void {
		// per default activate signalrappender for now
		const SignalRAppender = this.createSignalerAppender();
		Log.addAppender(SignalRAppender);

		const defaultLogLevel = Log.getLevel();
		this.connection.on("startProfiling", (logLevel: string) => {
			Log.info("Profiling started with log level " + logLevel);
			Log.setLevel(Level[logLevel]);
			Log.addAppender(SignalRAppender);
		});

		this.connection.on("stopProfiling", function () {
			Log.info("Profiling stopped");
			Log.removeAppender(SignalRAppender);
			Log.setLevel(defaultLogLevel);
		});

		this.connection.on("evalJavaScript", function (javaScript: string) {
			Log.error("Evaluating JavaScript from server is not supported!", javaScript);
		});

		this.connection.on("error", (error) => {
			if (error.status === undefined && error.responseText === undefined && !error) {
				return;
			}
			const errorMessage: string = error.responseText || error.message || error.toString();
			if (error.status === 401 || (!!errorMessage && errorMessage.includes("Unauthorized"))) {
				window.location.href = HelperUrl.resolveUrl("~/Main/Account/Login");
			} else if (!!errorMessage && errorMessage.includes("No transport could be initialized successfully")) {
				Log.info(`SignalR error: ${error}`);
				window.location.href = HelperUrl.resolveUrl("~/Main/Account/Login");
			} else if (!!errorMessage && errorMessage.includes("Send failed")) {
				this.connection.stop();
			} else if (!!errorMessage && (errorMessage.includes("Error during negotiation request")
				|| errorMessage.includes("The client has been inactive")
				|| errorMessage.includes("Failed to ping server")
				|| errorMessage.includes("Couldn't reconnect within the configured timeout"))) {
				// could happen when signalr tries to reconnect while offline, no real error since we support offline mode
				Log.warn("SignalR error: " + error);
			} else if (error.status === undefined || !errorMessage) {
				Log.error("SignalR error: " + error);
			} else if (error.status === 404) { // could happen when offline, no real error since we support offline mode
				Log.info(`SignalR error, status code ${error.status}, error message: ${error.responseText}`);
			} else if (error.status === 0 && error.responseText === "") {
				Log.warn("SignalR hub error with status code 0 and empty responseText");
			} else {
				Log.error(`SignalR error, status code ${error.status}, error message: ${errorMessage}`);
			}
		});

		if (window.onerror !== undefined) {
			window.onerror = function (errorMsg, url, lineNumber, columnNumber, error) {
				Log.error("Unhandled Exception Caught.\r\nURL: " + JSON.stringify(url) + "\r\nLine: " + lineNumber + "\r\nColumn: " + columnNumber + "\r\n", error);
				if (log4javascript.Level[Log.getLevel()] <= log4javascript.Level[log4javascript.Level.ERROR]) {
					console.error(errorMsg, `${url}:${lineNumber}:${columnNumber}`);
				}
				return true;
			};
		}
	}
}

