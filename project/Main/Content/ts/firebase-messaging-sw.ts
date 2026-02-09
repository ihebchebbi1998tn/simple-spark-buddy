import { initializeApp } from "firebase/app";
import { getMessaging, onBackgroundMessage } from "firebase/messaging/sw";

fetch("../Crm/PushNotification/GetConfiguration")
	.then((request) => {
		return request.json();
	}).then((configuration) => {
		const firebaseApp = initializeApp(configuration);
		const messaging = getMessaging(firebaseApp);

		onBackgroundMessage(messaging, msg => {
			// @ts-ignore
			self.registration.showNotification(msg.data.title, { "body": msg.data.body, "data": { "url": msg.data.url } });
		});
	});

self.addEventListener('notificationclick', e => {
	// @ts-ignore
	const url = self.registration.scope.split("/static-dist")[0];
	// @ts-ignore
	e.notification.close();
	// @ts-ignore
	clients.openWindow(url + e.notification.data.url.replace("~", "")).then(windowClient => windowClient ? windowClient.focus() : null);
});