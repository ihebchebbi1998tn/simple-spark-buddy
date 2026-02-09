; (function () {
	const hasPerDiemReportEntries = window.Helper.PerDiem.hasPerDiemReportEntries;
	window.Helper.PerDiem.hasPerDiemReportEntries = async (id) => {
		const OtherEntries = await hasPerDiemReportEntries(id);
		if (OtherEntries) {
			return true;
		}
		let serviceOrderTimePostings = await window.database.CrmService_ServiceOrderTimePosting
			.filter(function (expense) {
				return expense.PerDiemReportId === this.id;
			},
				{
					id: id
				})
			.count();
		let serviceOrderExpensePostings = await window.database.CrmService_ServiceOrderExpensePosting
			.filter(function (expense) {
				return expense.PerDiemReportId === this.id;
			},
				{
					id: id
				})
			.count();

		if (serviceOrderTimePostings === 0 && serviceOrderExpensePostings === 0) {
			return false;
		}
		else {
			return true;
		}
	};
})();