import {namespace} from "./namespace";

export class LogListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.Main_Log, Main.Rest.Model.ObservableMain_Log> {
	arrowIcon = ko.observable<string>(null);

	constructor() {
		super("Main_Log", "CreateDate", "DESC");
		this.addBookmarks();
		this.arrowIcon("zmdi zmdi-chevron-down")
	}

	initItem(item: Main.Rest.Model.Main_Log): Main.Rest.Model.ObservableMain_Log {
		let result = super.initItem(item);
		// @ts-ignore
		result.Color = this.getColor(item)
		return result;
	}

	toggleException() {
		this.arrowIcon() !== "zmdi zmdi-chevron-down" ? this.arrowIcon("zmdi zmdi-chevron-down") : this.arrowIcon("zmdi zmdi-chevron-up")
	}

	getColor(item: Main.Rest.Model.Main_Log): string {
		switch (item.Level) {
			case "ERROR":
				return 'bgm-red'
			case "WARN":
				return 'bgm-yellow'
			case "INFO":
				return 'bgm-blue'
			case "DEBUG":
				return 'bgm-grey'
			default:
				return ""
		}
	}

	bulkActionAllowed(item: Main.Rest.Model.Main_Log): boolean {
		return ["ERROR", "WARN", "INFO", "DEBUG"].includes(item.Level)
	}

	addBookmarks(): void {
		const defaultBookmark = {
			Category: window.Helper.String.getTranslatedString("LogState"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "AllState",
			Expression: query => query
		}
		this.bookmarks.push(defaultBookmark,
			{
				Category: window.Helper.String.getTranslatedString("LogState"),
				Name: "Error",
				Key: "Error",
				Expression: query => query.filter(it => it.Level === "ERROR")
			},
			{
				Category: window.Helper.String.getTranslatedString("LogState"),
				Name: "Info",
				Key: "Info",
				Expression: query => query.filter(it => it.Level === "INFO")
			},
			{
				Category: window.Helper.String.getTranslatedString("LogState"),
				Name: "Warn",
				Key: "Warn",
				Expression: query => query.filter(it => it.Level === "WARN")
			},
			{
				Category: window.Helper.String.getTranslatedString("LogState"),
				Name: "Debug",
				Key: "Debug",
				Expression: query => query.filter(it => it.Level === "DEBUG")
			});
		this.bookmark(defaultBookmark)
	}
	formattedLogMessage(message: KnockoutObservable<string>): string {
		const jsonStartPosition = message().indexOf("{\"logger\"");
		if (jsonStartPosition === -1)
			return message();

		const jsonString = message().substring(jsonStartPosition);
		if (jsonString.length === 0)
			return message();

		let messagePrefix = message().substring(0, jsonStartPosition - 1);
		let result = messagePrefix + "\n\n";
		try {
			const jsonObject = JSON.parse(jsonString);
			const jsonObjectKeys = Object.keys(jsonObject);
			result += "{\n";
			jsonObjectKeys.forEach(function(key, i){
				result += "  \"" + key + "\" : \"" + jsonObject[key] + "\"" + (i < jsonObjectKeys.length - 1 ? "," : "") + "\n"
			});
			result += "}";
		} catch (e) {
			result += jsonString;
		}

		return result;
	}
}

namespace("Main.ViewModels").LogListIndexViewModel = LogListIndexViewModel;