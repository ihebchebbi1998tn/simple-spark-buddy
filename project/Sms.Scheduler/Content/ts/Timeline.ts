import {
	DateHelper,
	DomHelper,
	ResourceModel,
	SchedulerAssignmentModel,
	SchedulerEventModel,
	SchedulerPro,
	SchedulerProConfig,
	SchedulerResourceModel,
	StringHelper,
} from "@bryntum/schedulerpro";
import {type Dispatch, isDispatch,} from "./Model/Dispatch";
import {namespace} from "@Main/namespace";
import {isTechnician, Technician} from "./Model/Technicians";
import {type Article, isTool, isVehicle} from "./Model/Article";
import type moment from "moment";
import type {Assignment} from "@Sms.Scheduler/Model/Assignment";
import {Absence, isAbsence} from "./Model/Absence";
import {isServiceOrderTimePosting} from "./Model/ServiceOrderTimePosting";

export class Timeline extends SchedulerPro {
	version: number = null;
	defaultEventDuration: moment.Duration = null;
	serviceOrderDispatchMaximumDuration: moment.Duration = null;
	ServiceOrderDispatchIgnoreCalculatedDuration = false;
	AllowSchedulingForPast: () => boolean = null;
	baseRowHeight = 30;
	singleRowHeight = 25;
	dataForFirstRow: () => string[] = null;
	dataForSecondRow: () => string[] = null;
	dataForThirdRow: () => string[] = null;

	static get $name() {
		return 'CRMTimeline';
	}

	// Factoryable type name
	static get type() {
		return 'CRMtimeline';
	}

	static get configurable(): Partial<SchedulerProConfig> {
		return {
			barMargin: 5,
			passStartEndParameters: true,
			createEventOnDblClick: false,
			multiEventSelect: true,
			selectResourceOnEventNavigate: false,
			readOnly: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit"),
			displayDateFormat: 'HH:mm',
			// @ts-ignore
			onAssignmentSelectionChange: ({selection, source}) => {
				let refreshRows = selection.length <= 1;
				(source as Timeline).selectGroupedAssignments(selection as Assignment[]);
				if (refreshRows) {
					source.refreshRows();
				}
			},

			// This controls the contents of each event bar. You can return JSON (a DOMConfig object) or a simple HTML string
			eventRenderer({eventRecord, resourceRecord, renderData}) {
				let dataText: string;
				if (isDispatch(eventRecord)) {
					dataText = eventRecord.Status.Value;
				} else if (isAbsence(eventRecord)) {
					dataText = eventRecord.AbsenceType;
				} else if (isServiceOrderTimePosting(eventRecord)) {
					dataText = eventRecord.OriginalData.ItemNo;
				}
				//@ts-expect-error
				const eventColors = window.Helper.Scheduler.GetTextColorForBackground(eventRecord);
				const eventHeader: any = {
					className: {
						'sch-event-header': true
					},
					children: [{
						class: 'sch-event-status-text',
						style: {
							color: eventColors.color
						},
						text: StringHelper.xss`${dataText}`,
					}]
				};

				const rows: any = {
					className: {
						'sch-event-rows': true
					},
					children: []
				};

				if (isDispatch(eventRecord)) {
					let icons: string[] = [];
					if(eventRecord.assignments.length > 1) {
						icons.push(`<span class="team-icon">T</span>`);
						if(eventRecord.OriginalData.Username === resourceRecord["ResourceKey"]) {
							rows.children.push({
								tag: 'i',
								className: 'b-fa b-fa-solid b-fa-crown leader-icon'
							});

						}
					}
					let selectedAssignments = this.selectedAssignments;
					let selectedDispatchIds = selectedAssignments.map(x => (x as Assignment).OriginalData.DispatchKey);
					let selectedDispatches = this.events.filter(x => isDispatch(x) && selectedDispatchIds.includes(x.OriginalData.Id)) as Dispatch[];
					let selectedGroupIds = selectedDispatches.map(x => x.OriginalData.ExtensionValues.SchedulingGroupId).filter(Boolean);
					if (selectedGroupIds.indexOf(eventRecord.OriginalData.ExtensionValues.SchedulingGroupId) !== -1) {
						icons.push(`<i class="b-fa b-fa-solid b-fa-link c-blue m-r-0"></i>`);
					}
					if (icons.length > 0) {
						eventHeader.children.push(DomHelper.createElement({
							html: `<span class="icons">${icons.join()}</span>`,
						}));
					}
					if(eventRecord.OriginalData.IsFixed) {
						const topLine = {
							class: {
								'sch-event-top-line': true
							},
							style: {
								'background-color': 'red'
							},
						};
						eventHeader.children.push(topLine);
					}
					rows.children.push({
						class: {
							'b-event-name': true,
							'sch-first-row': true
						},
						style: {
							color: eventColors.color
						}
					});

					if (this.dataForSecondRow()?.length > 0) {
						rows.children.push({
							class: 'sch-second-row',
							style: {
							color: eventColors.color
							}
						});
					}

					if (this.dataForThirdRow()?.length > 0) {
						rows.children.push({
							class: 'sch-third-row',
							style: {
								color: eventColors.color
							},
						});
					}

				} else if (isAbsence(eventRecord)) {
					rows.children.push({
						class: 'b-event-name',
						style: {
							color: eventColors.color
						},
						text: eventRecord.name
					});
				}
				return [eventHeader, rows];
			},

			columns: [
				{type: 'tree', minWidth: 0, autoWidth: true, cellCls: "b-0", leafIconCls: "", filterable: false},
				{
					type: 'resourceInfo',
					id: 'resourceInfo',
					tree: true,
					renderer: function (p) {
						if (!p.record["generatedParent"]) {
							return this.defaultRenderer(p);
						} else {
							let key = p.record["key"];
							if (!key) {
								return `${window.Helper.String.getTranslatedString("Unspecified")} (${window.Helper.String.getTranslatedString(p.record["field"])})`;
							} else {
								return key.toString();
							}
						}
					},
					showEventCount: false,
					text: window.Helper.getTranslatedString('Resources'),
					field: 'DisplayName',
					width: 200,
					editor: false,
					useNameAsImageName: false,
					filterable: {
						filterField: {
							triggers: {
								search: {
									cls: 'b-icon b-fa-filter'
								}
							},
							placeholder: window.Helper.getTranslatedString('FilterResources'),
							hidden: false
						}
					},
					showMeta: window.Helper.Scheduler.GetMetaFunctionText,
					sortable: false
				}
			],
		};
	}

	/**
	 * Checks if a resource is a generated parent node (created by treeGroup feature)
	 * These nodes are virtual grouping containers, not actual resources
	 */
	static isGeneratedParentNode(resource: SchedulerResourceModel): boolean {
		return Boolean(resource?.["generatedParent"]);
	}

	/**
	 * Finds the first valid technician child within a generated parent node
	 * Used for validating drops on group headers
	 */
	static getFirstTechnicianChild(resource: SchedulerResourceModel): Technician | null {
		if (!this.isGeneratedParentNode(resource)) {
			return isTechnician(resource) ? resource as Technician : null;
		}
		
		const children = resource.children;
		if (!Array.isArray(children) || children.length === 0) return null;
		
		for (const child of children as ResourceModel[]) {
			if (isTechnician(child)) {
				return child as Technician;
			}
			// Recursively check nested groups
			const nestedTechnician = this.getFirstTechnicianChild(child as SchedulerResourceModel);
			if (nestedTechnician) return nestedTechnician;
		}
		return null;
	}

	/**
	 * Checks if a generated parent node contains at least one valid technician
	 */
	static hasValidTechnicianChildren(resource: SchedulerResourceModel): boolean {
		return this.getFirstTechnicianChild(resource) !== null;
	}

	static eventDragValidator(data: {
		startDate: Date,
		endDate: Date,
		assignmentRecords: SchedulerAssignmentModel[],
		eventRecords: SchedulerEventModel[],
		newResource: SchedulerResourceModel,
		targetEventRecord: SchedulerEventModel
	}) {
		let {eventRecords, newResource, startDate, endDate} = data;
		
		// Handle generated parent nodes (resource groups)
		// Allow drop indicator if the group contains valid technician children
		if (this.isGeneratedParentNode(newResource)) {
			if (isDispatch(eventRecords[0])) {
				const hasValidChildren = this.hasValidTechnicianChildren(newResource);
				return {
					valid: hasValidChildren,
					message: hasValidChildren ? '' : window.Helper.getTranslatedString("NoValidResourcesInGroup")
				};
			}
		}
		
		if (!isDispatch(eventRecords[0])) {
			if ((isTechnician(newResource) && (eventRecords[0] as Absence).AbsenceTypeData instanceof Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_TimeEntryType) ||
				(!isTechnician(newResource) && (eventRecords[0] as Absence).AbsenceTypeData instanceof Crm.Article.Model.Lookups.CrmArticle_ArticleDowntimeReason)) {
				return {valid: true};
			}
			return {
				valid: false,
				message: `${window.Helper.getTranslatedString("NotValidFor")}: ${newResource.constructor.name}`,
			};
		}
		if (isDispatch(eventRecords[0]) && !isTechnician(newResource)) {
			return {
				valid: false,
				message: `${window.Helper.getTranslatedString("NotValidFor")}: ${newResource.constructor.name}}`
			};
		}
		let valid = isTechnician(newResource);
		const dispatch = eventRecords[0], order = dispatch.ServiceOrder.OriginalData;

		if (valid && order.RequiredSkillKeys.length == 0 && order.RequiredAssetKeys.length == 0) return {valid: true};

		const missingSkillKeys: string[] = [];
		const missingAssetKeys: string[] = [];

		let technicianHasSkills = window.Helper.Scheduler.hasSkillsForOrder((newResource as Technician), order, startDate, endDate, missingSkillKeys);
		let technicianHasAssets = window.Helper.Scheduler.hasAssetsForOrder((newResource as Technician), order, startDate, endDate, missingAssetKeys);

		const missingSkills = Array.from(new Set(missingSkillKeys)).map(key => window.Helper.Scheduler.CreateLookupProxy(Technician.lookups.skills, key));
		const missingAssets = Array.from(new Set(missingAssetKeys)).map(key => window.Helper.Scheduler.CreateLookupProxy(Technician.lookups.assets, key));

		valid &&= technicianHasSkills && technicianHasAssets;

		let validationText = "";
		if (!technicianHasSkills) {
			validationText = window.Helper.String.getTranslatedString("TechnicianHasNoneOfTheRequiredOrderSkills")
				.replace("{0}", window.Helper.User.getDisplayName((newResource as Technician).OriginalData))
				.replace("{1}", missingSkills.join());
		} else if (technicianHasSkills && window._.difference(order.RequiredSkillKeys, (newResource as Technician).ValidSkills.map(s => s.Key)).length > 0) {
			validationText = window.Helper.String.getTranslatedString("TechnicianLacksSomeOfRequiredOrderSkills")
				.replace("{0}", window.Helper.User.getDisplayName((newResource as Technician).OriginalData))
				.replace("{1}", missingSkills.join());
		}
		if (!technicianHasAssets) {
			validationText = window.Helper.String.getTranslatedString("TechnicianHasNoneOfTheRequiredOrderAssets")
				.replace("{0}", window.Helper.User.getDisplayName((newResource as Technician).OriginalData))
				.replace("{1}", missingAssets.join());
		} else if (technicianHasAssets && window._.difference(order.RequiredAssetKeys, (newResource as Technician).ValidAssets.map(a => a.Key)).length > 0) {
			validationText = window.Helper.String.getTranslatedString("TechnicianLacksSomeOfRequiredOrderAssets")
				.replace("{0}", window.Helper.User.getDisplayName((newResource as Technician).OriginalData))
				.replace("{1}", missingAssets.join());
		}

		return {
			valid: valid,
			message: valid ? '' : validationText
		};
	}

	static getEventContextMenuSelectedItems(scheduler, eventRecord) {
		let selectedEvents = [eventRecord];

		//trying to outsmart bryntum as always. If there are selected events but the context menu was opened on a non selected event then deselect all
		if (scheduler.selectedEvents && scheduler.selectedEvents.length > 0) {
			if (scheduler.selectedEvents.includes(eventRecord)) {
				selectedEvents = [...scheduler.selectedEvents];
			} else {
				//deselectAll method does not work, therefore we do it one by one!!
				let selecteds = [...scheduler.selectedEvents];
				for (let selected of selecteds) {
					scheduler.deselect(selected);
				}
			}
		}

		return selectedEvents;
	}

	numberOfRows() {
		const self = this;
		let nrOfRows = 1;
		if (self.dataForSecondRow()?.length > 0 || self.dataForThirdRow()?.length > 0) {
			nrOfRows = 2;
			if (self.dataForSecondRow()?.length > 0 && self.dataForThirdRow()?.length > 0) {
				nrOfRows = 3;
			}
		}

		return nrOfRows;
	}

	/**
	 * Collects generated parent nodes that contain at least one of the highlighted resources
	 * This ensures group headers show the drop indicator when their children are valid targets
	 */
	private collectParentNodesForHighlightedResources(highlightedResources: ResourceModel[]): ResourceModel[] {
		const highlightedIds = new Set(highlightedResources.map(r => r.id));
		const parentNodes: ResourceModel[] = [];
		
		const checkParentHasHighlightedChild = (parent: ResourceModel): boolean => {
			const children = parent.children;
			if (!Array.isArray(children) || children.length === 0) return false;
			
			for (const child of children as ResourceModel[]) {
				if (highlightedIds.has(child.id)) return true;
				const childChildren = child.children;
				if (Array.isArray(childChildren) && childChildren.length > 0 && checkParentHasHighlightedChild(child)) return true;
			}
			return false;
		};
		
		// Find all generated parent nodes that contain highlighted resources
		this.resourceStore.forEach((resource: ResourceModel) => {
			if (Timeline.isGeneratedParentNode(resource as SchedulerResourceModel) && 
				checkParentHasHighlightedChild(resource)) {
				parentNodes.push(resource);
			}
		});
		
		return parentNodes;
	}

	getResourcesToHighlight(eventRecords: SchedulerEventModel[]): SchedulerResourceModel[] {
		let dispatches = eventRecords?.filter(isDispatch);
		let absences = eventRecords?.filter((r): r is Absence => isAbsence(r) && r.AbsenceTypeData instanceof Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_TimeEntryType);
		let articleDowntimes = eventRecords?.filter((r): r is Absence => isAbsence(r) && r.AbsenceTypeData instanceof Crm.Article.Model.Lookups.CrmArticle_ArticleDowntimeReason);

		if (dispatches.length == 0 && absences.length == 0 && articleDowntimes.length == 0) {
			return [];
		}
		let resourcesToHighlight: ResourceModel[] = [];
		if (dispatches.length > 0) {
			if (dispatches[0].ServiceOrder.RequiredSkillKeys?.length == 0 && dispatches[0].ServiceOrder.RequiredAssetKeys?.length == 0) {
				resourcesToHighlight = this.resourceStore.query((resourceRecord: ResourceModel) => isTechnician(resourceRecord)) as Technician[];
			} else if (dispatches[0].ServiceOrder.RequiredAssetKeys?.length == 0 && dispatches[0].ServiceOrder.RequiredSkillKeys?.length > 0) {
				resourcesToHighlight = this.resourceStore.query((resourceRecord: ResourceModel) => isTechnician(resourceRecord) && window.Helper.Scheduler.hasSkillsForOrder(resourceRecord, dispatches[0].ServiceOrder.OriginalData, (dispatches[0].startDate as Date), DateHelper.add(dispatches[0].startDate, dispatches[0].duration))) as Technician[];
			} else if (dispatches[0].ServiceOrder.RequiredSkillKeys?.length == 0 && dispatches[0].ServiceOrder.RequiredAssetKeys?.length > 0) {
				resourcesToHighlight = this.resourceStore.query((resourceRecord: ResourceModel) => isTechnician(resourceRecord) && window.Helper.Scheduler.hasAssetsForOrder(resourceRecord, dispatches[0].ServiceOrder.OriginalData, (dispatches[0].startDate as Date), DateHelper.add(dispatches[0].startDate, dispatches[0].duration))) as Technician[];
			} else {
				resourcesToHighlight = this.resourceStore.query((resourceRecord) => isTechnician(resourceRecord) && window.Helper.Scheduler.hasAssetsForOrder(resourceRecord, dispatches[0].ServiceOrder.OriginalData, (dispatches[0].startDate as Date), DateHelper.add(dispatches[0].startDate, dispatches[0].duration)) && window.Helper.Scheduler.hasSkillsForOrder(resourceRecord, dispatches[0].ServiceOrder.OriginalData, (dispatches[0].startDate as Date), DateHelper.add(dispatches[0].startDate, dispatches[0].duration))) as Technician[];
			}
		}
		if (absences.length > 0) {
			resourcesToHighlight = this.resourceStore.query((resourceRecord: ResourceModel) => isTechnician(resourceRecord)) as Technician[];
		}
		if (articleDowntimes.length > 0) {
			resourcesToHighlight = this.resourceStore.query((resourceRecord: ResourceModel) => isTool(resourceRecord) || isVehicle(resourceRecord)) as Article[];
		}
		
		// Include generated parent nodes that contain highlighted resources
		// This ensures the blue line indicator appears on group headers as well
		const parentNodes = this.collectParentNodesForHighlightedResources(resourcesToHighlight);
		resourcesToHighlight.push(...parentNodes);
		
		return resourcesToHighlight;
	}

	selectGroupedAssignments(selection: Assignment[]) {
		let selectedDispatchIds = selection.map(x => x.OriginalData.DispatchKey);
		let allDispatches = this.eventStore.allRecords.filter(isDispatch);
		let selectedDispatches = allDispatches.filter(x => selectedDispatchIds.indexOf(x.OriginalData.Id) !== -1);
		let selectedGroupIds = selectedDispatches.map(x => x.OriginalData.ExtensionValues.SchedulingGroupId).filter(Boolean);
		let allDispatchIdsInGroup = allDispatches.filter(x => selectedGroupIds.indexOf(x.OriginalData.ExtensionValues.SchedulingGroupId) !== -1).map(x => x.OriginalData.Id);
		let allResourceAssignmentsInGroup = (this.assignments as any[]).filter(x => !!x.resource && allDispatchIdsInGroup.indexOf(x.OriginalData.DispatchKey) !== -1);
		let unselectedResourceAssignmentsInGroup = allResourceAssignmentsInGroup.filter(x => selection.indexOf(x) === -1);
		if (unselectedResourceAssignmentsInGroup.length > 0) {
			this.selectAssignments(unselectedResourceAssignmentsInGroup);
		}
	}
}

namespace("Sms.Scheduler").Timeline = Timeline;