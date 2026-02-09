;
(function($data: any) {
	$data.Queryable.prototype.specialFunctions = {
		expandAvatar: {
			"oData": function(urlSearchParams) {
				urlSearchParams.append("expandAvatar", "true");
			},
			"webSql": function(query) { return query; }
		},
		filterByOnlineStatus: {
			"oData": function(urlSearchParams) {
				urlSearchParams.append("filterByOnlineStatus", true);
			}
		},
		filterByPermissions: {
			"oData": function(urlSearchParams, permissions) {
				permissions = Array.isArray(permissions) ? permissions : [permissions];
				if (Array.isArray(permissions)) {
					permissions.forEach(function(permission, i) {
						urlSearchParams.append("filterByPermissions" + i, permission);
					});
				}
			}
		},
		filterByRoleId: {
			"oData": function (urlSearchParams, data) {
				urlSearchParams.append("filterByRoleId", data);
			},
			"webSql": function (query, data) {
				query = query
					.filter("$it.RoleIds.contains(this.roleId)", { roleId: data });
				return query;
			}
		},
		orderByArray: {
			"oData": function (urlSearchParams, data) {
				urlSearchParams.append("orderByArray" + data.property, data.values);
			},
			"webSql": function (query, data) {
				for (let i = 0; i < data.values.length; i++) {
					query = query.orderByDescending("it." + data.property + " === this.values[" + i + "]", { values: data.values });
				}
				return query;
			}
		}
	};
	document.addEventListener("DatabaseInitialized", function() {
		if (window.database.storageProvider.name !== "InMemory"){
			configureOdata();
		}
		if (window.database.storageProvider.name !== "oData") {
			configureSqlite();
		}
	});

	function configureSqlite() {
		function attachFunction(original, originalArguments) {
			var predicate = originalArguments[0];
			var thisArg = originalArguments[1];
			if (this.specialFunctions[predicate] && this.entityContext.storageProvider.name === "webSql") {
				return this.specialFunctions[predicate].webSql(this, thisArg);
			}
			return original.apply(this, originalArguments);
		}
		var originalQueryableFilter = $data.Queryable.prototype.filter;
		$data.Queryable.prototype.filter = function() {
			return attachFunction.call(this, originalQueryableFilter, arguments);
		};
		var originalQueryableOrderBy = $data.Queryable.prototype.orderBy;
		$data.Queryable.prototype.orderBy = function() {
			return attachFunction.call(this, originalQueryableOrderBy, arguments);
		};
		var originalQueryableOrderByDescending = $data.Queryable.prototype.orderByDescending;
		$data.Queryable.prototype.orderByDescending = function() {
			return attachFunction.call(this, originalQueryableOrderByDescending, arguments);
		};
		var originalQueryableInclude = $data.Queryable.prototype.include;
		$data.Queryable.prototype.include = function() {
			return attachFunction.call(this, originalQueryableInclude, arguments);
		};

		if (window.database.Main_Site) {
			if (window.database.Main_Site.GetCurrentSite) {
				throw "database.Main_Site.GetCurrentSite must be undefined at this point";
			}
			window.database.Main_Site.GetCurrentSite = function() {
				return window.database.Main_Site.filter("it.Id === this.id", { id: "00000000-0000-0000-0000-000000000000" });
			};
		}
	}

	function configureOdata() {
		var origCompile = $data.storageProviders.oData.oDataCompiler.prototype.compile;
		$data.storageProviders.oData.oDataCompiler.prototype.compile = function(query) {
			var result = origCompile.apply(this, arguments);
			if (query.expression) {
				var uri = result.queryText.split("?");
				var urlParams = new window.URLSearchParams(uri[1]);
				var specialFunctions = $data.Queryable.prototype.specialFunctions;
				Object.keys(specialFunctions).forEach(function(filterName) {
					var data = query.expression[filterName];
					if (data) {
						specialFunctions[filterName].oData(urlParams, data);
					}
				});
				result.queryText = uri[0] + "?" + urlParams.toString();
			}
			// fix jaydata forgetting posting the payload when calling actions in a $batch
			if (result.isBatchExecuteQuery === true) {
				for (let i = 0; i < result.subQueries.length; i++) {
					const query = result.subQueries[i];
					const payload = result.postData.__batchRequests[i];
					if (query.postData && !payload.data	&& query.expression.source instanceof $data.Expressions.ServiceOperationExpression) {
						payload.data = query.postData;
					}
				}
			}
			return result;
		};
		var origCreateQueryable = $data.Container.createQueryable;
		$data.Container.createQueryable = function(queryable, expression) {
			if (expression.expressionType !== $data.Expressions.EntitySetExpression
				&& expression.expressionType !== $data.Expressions.ServiceOperationExpression
				&& queryable && queryable.expression) {
				Object.keys(queryable.specialFunctions).forEach(function(filterName) {
					if (queryable.expression[filterName]) {
						expression[filterName] = queryable.expression[filterName]; //pass filter to the next (now outermost) expression when the queryable changes
					}
				});
			}
			return origCreateQueryable.apply(this, arguments);
		};

		function attachFunction(original, originalArguments) {
			var predicate = originalArguments[0];
			if (this.specialFunctions[predicate]) {
				var thisArg = originalArguments[1] || {};
				if (this.expression.expressionType === $data.Expressions.EntitySetExpression) {
					var queryableWithDummyFilter = this.filter("true");
					queryableWithDummyFilter.expression[predicate] = thisArg;
					return queryableWithDummyFilter;
				} else {
					this.expression[predicate] = thisArg;
				}
				return this;
			}
			return original.apply(this, originalArguments);
		}

		//special filter (e.g. for tags), expression is tagged to append the tags as parameter to the query string where ODataQueryTagsFilter will process them
		//do not touch EntitySetExpression as they are 'singletons', create a dummy filter instead
		var originalQueryableFilter = $data.Queryable.prototype.filter;
		$data.Queryable.prototype.filter = function() {
			return attachFunction.call(this, originalQueryableFilter, arguments);
		};
		var originalQueryableOrderBy = $data.Queryable.prototype.orderBy;
		$data.Queryable.prototype.orderBy = function() {
			return attachFunction.call(this, originalQueryableOrderBy, arguments);
		};
		var originalQueryableOrderByDescending = $data.Queryable.prototype.orderByDescending;
		$data.Queryable.prototype.orderByDescending = function() {
			return attachFunction.call(this, originalQueryableOrderByDescending, arguments);
		};
		var originalQueryableInclude = $data.Queryable.prototype.include;
		$data.Queryable.prototype.include = function() {
			return attachFunction.call(this, originalQueryableInclude, arguments);
		};
	}
})(window.$data);