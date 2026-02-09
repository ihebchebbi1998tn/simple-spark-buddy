import type { GridColumnConfig, SchedulerResourceModel } from "@bryntum/schedulerpro";
import type { Technician } from "@Sms.Scheduler/Model/Technicians";

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

	/**
	 * Checks if a resource is a team group node (UserGroup)
	 * Team groups are special generated parent nodes representing service teams
	 */
	static isTeamGroupNode(resource: SchedulerResourceModel): boolean {
		return resource?.["key"] instanceof Main.Rest.Model.Main_Usergroup;
	}

	/**
	 * Gets the team (UserGroup) from a team group node
	 */
	static getTeamFromGroupNode(resource: SchedulerResourceModel): Main.Rest.Model.Main_Usergroup | null {
		if (this.isTeamGroupNode(resource)) {
			return resource["key"] as Main.Rest.Model.Main_Usergroup;
		}
		return null;
	}

	/**
	 * Override to provide team-specific validation for generated parent nodes
	 * When dragging onto a team group, we validate based on the team's member technicians
	 */
	static override getFirstTechnicianChild(resource: SchedulerResourceModel): Technician | null {
		// For team group nodes, check if the team has valid member technicians
		if (this.isTeamGroupNode(resource)) {
			const team = this.getTeamFromGroupNode(resource);
			if (team?.Members?.length > 0) {
				// Team has members, look for technicians in children
				const children = resource.children as SchedulerResourceModel[];
				if (children?.length) {
					for (const child of children) {
						const technician = super.getFirstTechnicianChild(child);
						if (technician) return technician;
					}
				}
			}
			return null;
		}
		
		// For non-team groups, use base implementation
		return super.getFirstTechnicianChild(resource);
	}
}

window.Sms.Scheduler.Timeline = TimelineExtension;