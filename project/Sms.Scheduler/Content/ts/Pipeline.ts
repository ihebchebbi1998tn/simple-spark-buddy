import {Model, Store, StringHelper, TreeGrid, TreeGridConfig} from "@bryntum/schedulerpro";
import {isAbsenceOrder, isArticleDowntime} from "./Model/AbsenceOrder";
import {isJob} from "./Model/Job";
import {isServiceOrder, MaterialAvailabilityStatusEnum, ServiceOrder} from "./Model/ServiceOrder";

export class Pipeline extends TreeGrid {
	pipelineFirstLineData: () => string;
	pipelineSecondLineData: () => string;
	private readonly recordsClasses = new Map<string, WeakSet<Model>>();

	static get $name() {
		return 'CRMPipeline';
	}

	// Factoryable type name
	static get type() {
		return 'CRMpipeline';
	}

	static get configurable(): Partial<TreeGridConfig> {
		return {
			rowHeight: 25,
			selectionMode: {
				cell: false,
				deselectOnClick: true,
			},
			columns: [
				{
					type: 'tree',
					field: 'name',
					text: window.Helper.String.getTranslatedString("Pipeline"),
					sortable: false,
					cellMenuItems: {
						OpenOrderButton: window.AuthorizationManager.currentUserHasPermission("ServiceOrder::Read") ? {
							text: `${window.Helper.getTranslatedString("Open Order")}`,
							icon: 'b-fa b-fa-fw b-fa-folder-open',
							onItem: (item) => {
								if (isServiceOrder(item.record)) {
									window.open(`#/Crm.Service/ServiceOrder/DetailsTemplate/${item.record.OriginalData.Id}`, window.Helper.Scheduler.URLTarget());
								} else if (isJob(item.record)) {
									window.open(`#/Crm.Service/ServiceOrder/DetailsTemplate/${item.record.serviceOrder.OriginalData.Id}`, window.Helper.Scheduler.URLTarget());
								}
							}
						} : null,
						openDispatches: window.AuthorizationManager.currentUserHasPermission("ServiceOrder::Read") ? {
							text: `${window.Helper.getTranslatedString("Open Dispatch")}`,
							icon: 'zmdi zmdi-layers',
							onItem: (item) => {
								if (isServiceOrder(item.record)) {
									window.open(`#/Crm.Service/ServiceOrder/DetailsTemplate/${item.record.OriginalData.Id}?tab=tab-dispatches`, window.Helper.Scheduler.URLTarget());
								} else if (isJob(item.record)) {
									window.open(`#/Crm.Service/ServiceOrder/DetailsTemplate/${item.record.serviceOrder.OriginalData.Id}?tab=tab-dispatches`, window.Helper.Scheduler.URLTarget());
								}
							}
						} : null
					},
					htmlEncode: false,
					renderer: ({record, size, grid}) => {
						if (isServiceOrder(record) || record.isLeaf) {
							record["cls"] = "draggable";
						}
						let suffix = record.isLeaf ? "" : ` (${record.allChildren.filter(c => c.isLeaf).length})`;
						if (isServiceOrder(record)) {
							record.iconCls = (grid as Pipeline).getIconClsForServiceOrders(record);
							return `<dl>
								<dd>
									<p class='serviceorder-title p-0 m-5'>
										${StringHelper.encodeHtml(record.OriginalData.OrderNo)} ${suffix} ${(grid as Pipeline).pipelineFirstLineData() != null ? '- ' + StringHelper.encodeHtml(record.getRowData((grid as Pipeline).pipelineFirstLineData())) : ""}
										${record.OriginalData.Latitude && record.OriginalData.Longitude ? `<i class='zmdi zmdi-pin zmdi-hc-fw' title='${record.OriginalData.Latitude}, ${record.OriginalData.Longitude}'></i>` : ""}
									</p>
									<p class="p-0 m-5">
										${(grid as Pipeline).pipelineSecondLineData() != null ? StringHelper.encodeHtml(record.getRowData((grid as Pipeline).pipelineSecondLineData())) : ""}
									</p>
								</dd>
							</dl>`;
						} else if (isAbsenceOrder(record) || isArticleDowntime(record)) {
							return `<dl>
								<dd>
									${StringHelper.encodeHtml(record.name)} (${StringHelper.encodeHtml(record.AbsenceType)}) ${suffix}
								</dd>
							</dl>`;
						} else if (isJob(record)) {
							return `<dl>
								<dd>
									${StringHelper.encodeHtml(record.toString())} ${suffix}
								</dd>
							</dl>`;
						} else {
							return `<dl>
								<dd>
									${StringHelper.encodeHtml(record.get('name'))} ${suffix}
								</dd>
							</dl>`;
						}
					}
				}
			]
		};
	}

	construct(config?: Partial<TreeGridConfig>) {
		super.construct(config);

		this.on("expandNode", this.reassignRowsClasses, this);

		//Overriding expandAll in order to reassign row's classes.
		//This cannot be done using an expandAll function in this class because it is assign by the super.construct
		const expandAll = this.expandAll;
		this.expandAll = async () => {
			await expandAll();
			this.reassignRowsClasses();
		}
	}

	assignClassToRecords(cls: string, records: WeakSet<Model>) {
		this.recordsClasses.set(cls, records);

		this.reassignRowsClasses();
	}

	unassignClassFromAllRecords(cls: string) {
		this.recordsClasses.set(cls, new WeakSet());

		this.reassignRowsClasses();
	}

	private reassignRowsClasses() {
		const allRecords = (this.store as Store).allRecords;
		allRecords.forEach(r => {
			const row = this.getRowFor(r);

			if (row) {
				//This creates an object with one entry per css class and value of true/false to indicate if the class is enabled or not for this record
				const classes = Object.fromEntries(Array.from(this.recordsClasses.keys()).map(cls => ([cls, this.recordsClasses.get(cls).has(r)])));

				row.assignCls(classes);
			}
		});
	}

	private getIconClsForServiceOrders(order: ServiceOrder) {
		let iconCls = "";
		if (order.MaterialAvailability === MaterialAvailabilityStatusEnum.FullyStocked) {
			iconCls = "b-tree-icon b-icon b-icon-tree-leaf sch-green";
		} else if (order.MaterialAvailability === MaterialAvailabilityStatusEnum.PartsMissing) {
			iconCls = "b-tree-icon b-icon b-icon-tree-leaf sch-yellow";
		} else if (order.MaterialAvailability === MaterialAvailabilityStatusEnum.OutOfStock) {
			iconCls = "b-tree-icon b-icon b-icon-tree-leaf sch-red";
		}
		return iconCls;
	}
}
