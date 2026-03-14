import * as signalR from "@microsoft/signalr";
import {namespace} from "./namespace";
window.signalR ??= signalR;
(function () {
	const reconnectDelay = 5000;
	let connectionDeferred = $.Deferred();

	$(function(){
		$.connection = $.connection || {};
		const connection = $.connection.ProfilingHub = new window.signalR.HubConnectionBuilder()
			.withUrl(window.Helper.Url.resolveUrl("~/profilingHub"))
			.withAutomaticReconnect({
				nextRetryDelayInMilliseconds: (retryContext) => {
					if (retryContext.retryReason.message.indexOf("Unauthorized") !== -1){
						window.location.href = window.Helper.Url.resolveUrl("~/Main/Account/Login");
					}
					return reconnectDelay;
				}
			})
			.build();
	
		connection.onclose(function() {
			connectionDeferred = $.Deferred();
			$(connection).trigger("stateChanged");
		});
	
		connection.onreconnecting(function() {
			connectionDeferred = $.Deferred();
			$(connection).trigger("stateChanged");
		});
	
		connection.onreconnected(function() {
			$(document).trigger("signalrConnectionEstablished");
			connectionDeferred.resolve();
			$(connection).trigger("stateChanged");
		});
	
		async function start() {
			try {
				await connection.start();
				$(document).trigger("signalrConnectionEstablished");
				connectionDeferred.resolve();
				$(connection).trigger("stateChanged");
			} catch (error: any) {
				const errorMessage = (error.message || error);
				if (errorMessage.indexOf("Account/Login") > -1 || errorMessage.indexOf("No transport could be initialized") > -1) {
					window.location.href = window.Helper.Url.resolveUrl("~/Main/Account/Login");
				}
				connectionDeferred.reject(0);
			}
		}
		start();
		
		$(window.Helper.Offline).on("error", (event, errorMessage) => {
			if (errorMessage.indexOf("not authorized") !== -1) {
				window.location.href = window.Helper.Url.resolveUrl("~/Main/Account/Login");
			}
		});

		$(document).on("click", ".logout", function () {
			connection.stop();
		});

		window.addEventListener("beforeunload", ev => { connection.stop(); });
	});

	namespace("window.Crm").SignalR = {
		establishConnection: () => connectionDeferred.promise()
	};

})();
