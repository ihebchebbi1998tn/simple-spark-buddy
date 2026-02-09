import {ResourceUtilization} from "@bryntum/schedulerpro";


export class CrmResourceUtilization extends ResourceUtilization {
	static readonly $name = 'CrmResourceUtilization';
	static readonly type = 'crmResourceUtilization';

	getState(): Object {
		let state: Object = super.getState();
		return {
			...state,
			...{
				isVisible: this.isVisible,
				height: this.height
			}
		};
	}

	async applyState(state: Object & { height: number, isVisible: boolean }) {
		super.applyState(state);

		this.height = state.height;
		if (state.isVisible) {
			await this.show();
		} else {
			await this.hide(false);
		}
	}
}

CrmResourceUtilization.initClass();