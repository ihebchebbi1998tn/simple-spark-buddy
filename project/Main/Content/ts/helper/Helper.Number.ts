export class HelperNumber {
	static countDecimals(num: number): number {
		if (Number.isNaN(num)) {
			return 0;
		}
		if (Math.floor(num) === num) {
			return 0;
		}
		return num.toString().split(".")[1].length || 0;
	}
}
