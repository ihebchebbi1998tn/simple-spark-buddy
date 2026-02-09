import type { GridColumnConfig } from "@bryntum/schedulerpro";

export class TimelineExtension extends window.Sms.Scheduler.Timeline {
	static get $name() {
		return 'TimelineExtension';
	}

	// Factoryable type name
	static get type() {
		return 'TimelineExtension';
	}
	static get configurable(): any {
		let configurable = super.configurable;
		const columns = configurable.columns as Partial<GridColumnConfig>[];
		const resourceInfo = columns.find(c => c.id === 'resourceInfo');
		const baseResourceInfoRenderer = resourceInfo.renderer;
		resourceInfo.renderer = function (data) {
			if(data.record["key"] instanceof Main.Rest.Model.Main_Usergroup) {
				$(data.row.element).attr("team-id", data.record["key"].Id);
			}
			return baseResourceInfoRenderer.call(this, data);
		}
		return configurable;
	}
}

window.Sms.Scheduler.Timeline = TimelineExtension;