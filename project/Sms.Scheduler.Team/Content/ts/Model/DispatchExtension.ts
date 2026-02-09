export class DispatchExtension extends window.Sms.Scheduler.Model.Dispatch {
	static get $name() {
		return 'DispatchExtension';
	}

	// Factoryable type name
	static get type() {
		return 'DispatchExtension';
	}
	get TeamId(): string {
		return this.OriginalData.ExtensionValues.TeamId;
	}
	set TeamId(value: string) {
		this.OriginalData.ExtensionValues.TeamId = value;
	}
}
window.Sms.Scheduler.Model.Dispatch = DispatchExtension;