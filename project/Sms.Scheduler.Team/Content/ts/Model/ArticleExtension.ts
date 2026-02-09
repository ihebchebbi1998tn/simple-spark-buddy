import {LazyOfTTMetadata} from "@Sms.Scheduler/Lazy";
import _ from "lodash";

export class ArticleExtension extends window.Sms.Scheduler.Model.Article {
	static get $name() {
		return 'ArticleExtension';
	}

	// Factoryable type name
	static get type() {
		return 'ArticleExtension';
	}
	_Teams: LazyOfTTMetadata<any, string[]>;

	get Teams(): Main.Rest.Model.Main_Usergroup[] {
		return this._Teams?.value;
	}
	set Teams(value: Main.Rest.Model.Main_Usergroup[]) {
		let keys = value?.map(s => s.Id);
		if (this.Teams == null || !_.isEqual(this._Teams.Metadata, keys)) {
			this._Teams = new LazyOfTTMetadata(() => {
				if (value?.length > 0) {
					function proxyGet(target, prop, reciever) {
						if (prop === 'toString') {
							return () => target.Name;
						}
						// @ts-ignore
						return Reflect.get(...arguments);
					}

					return value.map(t => new Proxy(t, {
						get: proxyGet
					}));
				} else {
					return undefined;
				}
			}, keys);
		}
	}
}
window.Sms.Scheduler.Model.Article = ArticleExtension;