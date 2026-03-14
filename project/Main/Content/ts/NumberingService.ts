export class NumberingService {
	
	static async getNextFormattedNumber(sequenceName: string): Promise<string> {
		const url = window.Helper.resolveUrl("~/Main/NumberingSequence/" + sequenceName + ".json");
		return $.ajax(url);
	};

	/**
	 * Expects the sequenceName, the table and the attribute
	 *
	 * @param sequenceName 	The name of the NumberingSequence
	 * @param table			The table where the function is looking for already used numbers
	 * @param number		The attribute of the entity which is compared to the new number
	 * @returns 			The next unique number of a NumberingSequence
	 */
	static async getNextUniqueNumber(sequenceName: string, table: $data.EntitySet<any, any>, number: string): Promise<string> {
		const nextNumber = await NumberingService.getNextFormattedNumber(sequenceName);

		let count = await table.filter("it." + number + " === this.number", { number: nextNumber }).count()
		let alreadyExists = count > 0;

		if (alreadyExists == false) {
			return nextNumber;
		} else {
			return await NumberingService.getNextUniqueNumber(sequenceName, table, number);
		}

	}

	/**
	 *
	 * @param noIsGenerated		Value of the appSetting if the number is generated
	 * @param noIsCreatable		Value of the appSetting if the number can be created by the user
	 * @param entityNo			The current value of the number
	 * @param sequenceName		The name of the NumberingSequence
	 * @param table				The table where the function is looking for already used numbers
	 * @param number			The attribute of the entity which is compared to the new number
	 * @returns 				New number based on the selected appSettings and based on the current value of the number
	 */
	static async createNewNumberBasedOnAppSettings(noIsGenerated: boolean, noIsCreatable: boolean, entityNo: string, sequenceName: string, table: $data.EntitySet<any, any>, number: string): Promise<string> {
		if (noIsGenerated) {
			if (noIsCreatable) {
				if (!entityNo) {
					return await NumberingService.getNextUniqueNumber(sequenceName, table, number);
				} else {
					return null;
				}
			} else {
				return await NumberingService.getNextUniqueNumber(sequenceName, table, number);
			}
		} else {
			return null;
		}
	}
	
	static async registerNumberingService(): Promise<void>{
	}
}

if (!window.NumberingService) {
	window.NumberingService = NumberingService;
}