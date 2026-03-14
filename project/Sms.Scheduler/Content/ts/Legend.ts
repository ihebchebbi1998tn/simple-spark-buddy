import {Combo, ComboConfig, Model} from "@bryntum/schedulerpro";

export class ComboLegend extends Combo {
	static readonly $name = 'ComboLegend';
	static readonly type = 'comboLegend';

	static get configurable(): Partial<ComboConfig> {
		return {
			multiSelect: true,
			label: window.Helper.getTranslatedString('Legend'),
			editable: false,
			chipView: {
				iconTpl: (record: Model) => this.createIconTag(record)
			},
			listItemTpl: (record: Model) => this.createItemTag(record),
			displayField: 'text',
			valueField: 'value',
			store: {
				fields: [
					'border',
					'type'
				]
			}
		}
	}

	private static createIconTag(record) {
		const borderCls = `border-radius: 100%;border: ${record.border}`;
		return `<i class="b-fa b-fa-solid b-fa-circle" style="color: ${record.color};${(record.border ? borderCls : "")}; margin-right: .3em"></i>`
	}

	private static createItemTag(record) {
		return `${this.createIconTag(record)} ${record.text}`;
	}
}