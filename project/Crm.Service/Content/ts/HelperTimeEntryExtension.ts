;(function () {
	const getLatestTimeEntry = window.Helper.TimeEntry.getLatestTimeEntry;
	window.Helper.TimeEntry.getLatestTimeEntry = async (username, date) => {
		const dateStart = moment(date).startOf("day").toDate();
		const dateEnd = moment(date).endOf("day").toDate();
		if (!window.database.CrmService_ServiceOrderTimePosting) {
			return getLatestTimeEntry(username, date);
		}
		let result = await getLatestTimeEntry(username, date);
		let results = await window.database.CrmService_ServiceOrderTimePosting.filter(function (it) {
				return it.Username === this.username &&
					it.To >= this.dateStart &&
					it.From <= this.dateEnd;
			},
			{username: username, dateStart: dateStart, dateEnd: dateEnd})
			.orderByDescending(function (it) {
				return it.To;
			})
			.take(1)
			.toArray();
		if (results.length === 0) {
			return result;
		}
		return result && result.To > results[0].To ? result : results[0];
	};
})();