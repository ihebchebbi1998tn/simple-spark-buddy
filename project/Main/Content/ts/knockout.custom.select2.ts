
; (function (ko) {
	ko.bindingHandlers.select2autocompleter = {

		getAllIds: function (arr) {
			const ids = arr.reduce((acc, el, i) => {
				acc[el.id] = i;
				return acc;
			}, {});

			return ids
		},

		getParents: async function (mappedResult, parameters) {
			let queryResults = [];
			const mappedResultId = ko.bindingHandlers.select2autocompleter.getAllIds(mappedResult);

			for (let id in mappedResultId) {
				if (!queryResults.some(el => el.Id === id)) {
					await ko.bindingHandlers.select2autocompleter.executeParentsQuery(queryResults, id, parameters, mappedResult);
				}
			}

			return queryResults
		},

		executeParentsQuery: async function (queryResults, id, parameters, mappedResult) {
			if (queryResults.some(el => el.Id === id)) {
				return;
			}

			const result = mappedResult.find(x => x.id === id)?.item || await window.database[parameters.table].find(id);
			let nestedProperty = parameters.nestedProperty;
			queryResults.push(result);

			if (result[nestedProperty] !== null && typeof result[nestedProperty] !== "undefined") {
				await ko.bindingHandlers.select2autocompleter.executeParentsQuery(queryResults, result[nestedProperty], parameters, mappedResult);
			}
		},

		addChildsToParent: function (arr, ids, property) {
			arr.map(el => {
				if (property in el.item) {
					if (el.item[property] === null) {
						return
					}
					const parent = arr[ids[el.item[property]]];

					if (typeof parent === "undefined") {
						throw new DOMException(`Property ${property} it's not nestable`);
					}
					if (parent.id === el.id) {
						throw new DOMException(`Property ${property} it's not nestable`);
					}

					parent.childs = [...(parent.childs || []), el];
				}
			});
		},

		addDepthLevel: function (arr, depth = 0) {
			arr.forEach(obj => {
				obj.depth = depth;
				if (obj.childs) {
					ko.bindingHandlers.select2autocompleter.addDepthLevel(obj.childs, depth + 1);
				}
			})
		},

		treeToFlat: function (arr) {
			const flatArray = [];

			arr.forEach(item => {
				flatArray.push(item);
				if (item.childs) {
					flatArray.push(...ko.bindingHandlers.select2autocompleter.treeToFlat(item.childs));
					delete item.childs;
				}
			})

			return flatArray;
		},

		nestObjects: function (mappedResult, nestedProperty) {
			const ids = ko.bindingHandlers.select2autocompleter.getAllIds(mappedResult);
			ko.bindingHandlers.select2autocompleter.addChildsToParent(mappedResult, ids, nestedProperty);
			mappedResult = mappedResult.filter(obj => obj.item[nestedProperty] === null);
			ko.bindingHandlers.select2autocompleter.addDepthLevel(mappedResult);
			mappedResult = ko.bindingHandlers.select2autocompleter.treeToFlat(mappedResult);

			return mappedResult
		},

		init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
			var $modal = $(".modal.in .modal-body");
			var container = $modal.length === 1 ? $modal : $("body");
			var options = ko.unwrap(valueAccessor());
			var parameters = options.autocompleteOptions;
			if (!parameters) {
				var data = $.map(ko.unwrap(options.data),
					function (o) {
						var id, text;
						if (o.Value !== undefined) {
							id = o.Value;
						} else if (o.id !== undefined) {
							id = o.id;
						} else {
							id = o;
						}
						if (o.Text !== undefined) {
							text = o.Text;
						} else if (o.text !== undefined) {
							text = o.text;
						} else {
							text = o;
						}

						return {
							id: ko.unwrap(o[options.optionsValue]) || id || "",
							text: ko.unwrap(o[options.optionsText] || options.optionsText(o)) || text
						};
					});
				$(element).select2({
					data: data,
					allowClear: options.allowClear !== false,
					dropdownParent: container,
					width: '100%',
					placeholder: ko.unwrap(options.placeholder) || window.Helper.String.getTranslatedString("PleaseSelect"),
					tags: options.tags,
					language: {
						noResults: function () {
							return ' <img class=\"img-responsive waves-effect p-10 i-logo\" src="' + window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/lmobile-block-sad.svg') + '"\>'
								+ '<h4 class="p-l-10 p-r-10">' + window.Helper.String.getTranslatedString("NothingFound") + '</h4>';
						},
						searching: function () {
							return window.Helper.String.getTranslatedString("loading");
						},
						errorLoading: function () {
							return ' <img class=\"img-responsive waves-effect p-10 i-logo\" src="' + window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/lmobile-block-excited.svg') + '"\>'
								+ '<h4 class="p-l-10 p-r-10">' + window.Helper.String.getTranslatedString("UnknownError") +
								window.Helper.String.getTranslatedString("Error_InternalServerError") + '</h4>';
						}
					},
					escapeMarkup: function (markup) {
						return markup;
					}
				});

			} else {
				if (parameters.templateResultId && !parameters.templateResult) {
					parameters.templateResult = function (result) {
						if (!result.item) {
							return result.text;
						}
						const template = `<div data-bind="template: { name: '${parameters.templateResultId}', data: item }"></div>`;
						const $template = $(template);
						const resultBindingContext = bindingContext.extend(result);
						ko.applyBindings(resultBindingContext, $template[0]);
						return $template;
					};
				}
				var db = typeof parameters.table === "function" ? parameters.table() : window.database[parameters.table];
				var orderBy = parameters.orderBy;
				var customFilter = parameters.customFilter;
				var count = null;
				var oldTerm = null;
				var latestQuery;
				var results = [];
				var queryFunc = async function (query) {
					$(element).next(".select2").addClass("select2-active");
					latestQuery = query;
					var pageSize = 25;
					var pageNum = query.page || 1;
					var dbquery = db;
					var joins = parameters.joins;
					if (!!joins) {
						joins.forEach(function (join) {
							if (typeof join === "string") {
								dbquery = dbquery.include(join);
							} else {
								dbquery = dbquery.include2(join.Selector + "." + join.Operation);
							}
						});
					}
					query.term = query.term ? query.term.trim() : query.term;
					if (!!customFilter) {
						dbquery = await customFilter(dbquery, query.term);
					} else {
						if (typeof query.term != "undefined") {
							dbquery = window.Helper.String.contains(dbquery, query.term, ["Name.toLowerCase()"]);
						}
					}
					if (!!orderBy) {
						(orderBy || []).forEach(function (o) {
							dbquery = dbquery.orderBy("it." + o);
						});
					}

					if (oldTerm !== query.term) {
						oldTerm = query.term;
						count = null;
						let withInlineCount = true;
						if (dbquery.expression){
							let source = dbquery.expression;
							while (source){
								// @ts-ignore
								if (source instanceof window.$data.Expressions.InlineCountExpression) {
									withInlineCount = false;
								}
								source = source.source;
							}
						}
						if (withInlineCount) {
							dbquery = dbquery.withInlineCount();
						}
					}

					try {
						let result = await dbquery.take(pageSize).skip((pageNum - 1) * pageSize).toArray();
						if (parameters.onResult) {
							result = await parameters.onResult(result);
						}
						count = result.totalCount || count;
						var mappedResult = result.map(function (object) {
							return parameters.mapDisplayObject(object);
						});
						if (parameters.tags && query.term && !mappedResult.some(x => x.text.toLowerCase() === query.term.toLowerCase())){
							mappedResult.unshift({id:query.term, text:query.term});
						}
						if (query.term === undefined && ko.unwrap(parameters.noneOption) && pageNum === 1) {
							mappedResult.unshift({id:ko.unwrap(parameters.noneOption), text:'<i>'+Helper.String.getTranslatedString('None')+'</i>'});
						}
						const property = parameters.nestedProperty ? parameters.nestedProperty.trim() : parameters.nestedProperty
						if (
							parameters.nested === true &&
							parameters.nestedProperty &&
							mappedResult.length > 0 &&
							(property in mappedResult[0]?.item)) {

							if (pageNum === 1){
								results = [];
							}

							result = await ko.bindingHandlers.select2autocompleter.getParents(mappedResult, parameters);
							result = result.map(function (object) {
								return parameters.mapDisplayObject(object);
							})

							result.map(obj => {
								if (mappedResult.some(el => el.id == obj.id)) {
									obj.disabled = false;
								} else {
									obj.disabled = true;
								}
							})

							mappedResult = ko.bindingHandlers.select2autocompleter.nestObjects(result, property);
							mappedResult = mappedResult.filter(x => !results.some(y => x.id == y.id));
							results.push(...mappedResult);
						}

						if (query === latestQuery) {
							$(element).next(".select2").removeClass("select2-active");
							query.callback({
								results: mappedResult,
								pagination: {
									more: (((pageNum - 1) * pageSize) <= count) && (pageSize < count)
								}
							});
						}
					} catch (e: any) {
						window.Log.error(e.data ?? e);
						var data = { hasError: true, results: [] };
						if (query === latestQuery) {
							$(element).next(".select2").removeClass("select2-active");
							query.callback(data);
						}
					}
				};

				var debounce;
				$(element).select2({
					ajax: db ? {} : null,
					allowClear: options.allowClear !== false,
					data: options.data,
					dropdownParent: container,
					language: {
						noResults: function () {
							var text;
							if ($.isFunction(parameters.noResults)) {
								text = parameters.noResults();
							} else if (parameters.noResults) {
								text = parameters.noResults;
							} else {
								text = window.Helper.String.getTranslatedString("NothingFound");
							}
							return '<div class="text-center"> <img class=\"img-responsive waves-effect p-10 i-logo\" src="' + window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/lmobile-block-sad.svg') + '"\>'
								+ '<h4 class="p-l-10 p-r-10">' + text + '</h4></div>';
						},
						searching: function () {
							return '<div class="text-center">'
								+ '<div class="autocompleter-preloader preloader pls-blue">'
								+ '<svg class="pl-circular" viewBox="25 25 50 50"><circle class="plc-path" cx="50" cy="50" r="20"></circle></svg>'
								+ "</div>"
								+ "</div>";
						},
						errorLoading: function () {
							return '<div class="text-center"> <img class=\"img-responsive waves-effect p-10 i-logo\" src="' + window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/lmobile-block-excited.svg') + '"\>'
								+ '<h4 class="p-l-10 p-r-10">' + window.Helper.String.getTranslatedString("UnknownError") + '</h4></div>';
						}
					},
					escapeMarkup: function (markup) {
						return markup;
					},
					placeholder: ko.unwrap(options.placeholder) || ko.unwrap(parameters.placeholder) || window.Helper.String.getTranslatedString("PleaseSelect"),
					tags: parameters.tags,
					templateResult: parameters.templateResult,
					// @ts-ignore
					query: db ? function(query) {
						var context = this;
						var args = arguments;
						var later = function () {
							debounce = null;
							queryFunc(query);
						};
						clearTimeout(debounce);
						if (!query.term) {
							later();
						} else {
							debounce = setTimeout(later, 500);
						}
					} : undefined,
					width: "100%"
				});
			}
			var select2: any = $(element).data("select2");
			var select2Input = select2.selection.$search || select2.dropdown.$search;
			select2Input.off('keyup');
			select2Input.on('keyup', null, select2, function (evt) {
				var select2 = evt.data;
				var key = evt.which;
				var tabKeyCode = 9;
				if (key !== tabKeyCode && select2.dropdown.handleSearch) {
					select2.dropdown.handleSearch(evt);
				}
			});
			
			$(element).on('select2:open', function () {
				// @ts-ignore
				if (window.Modernizr && window.Modernizr.touch) {
					$('.select2-container').click(function (e) {
						var searchField = $(e.currentTarget).find('input.select2-search__field');
						if (searchField.length) {
							searchField.focus();
						}
					});

				}

			});
			select2.on('results:all', function (params) {
				if (params && params.data.hasError) {
					this.trigger('results:message', {
						message: 'errorLoading'
					});
				}
			});
			select2.on('query', function (params) {
				if (this.options.options.query) {
					if (!this.results) {
						return;
					}
					this.$results.empty();
					var loadingMore = this.options.get('translations').get('searching');
					var loading = {
						disabled: true,
						loading: true,
						text: loadingMore(params)
					};
					var $loading = this.results.option(loading);
					$loading.className += ' loading-results';

					this.$results.prepend($loading);
				}

			});
			var keypress = select2.listeners.keypress[1];
			if (keypress) {
				select2.listeners.keypress[1] = function (evt) {
					var key = evt.which;
					var tabKeyCode = 9;
					if (key === tabKeyCode) {
						select2.close();
					} else {
						keypress.apply(this, arguments);
					}
				};
			}
			$(element).parent().on('focus', '.select2-selection.select2-selection--single', function (e) {
				$(this).closest(".select2-container").siblings('select:enabled').select2('open');
			});
			var observable = allBindings().value || allBindings().selectedOptions || valueAccessor().data;
			if (!ko.isObservable(observable)) {
				observable = ko.observable(observable);
			}
			$(element).on("select2:select",
				function (e) {
					if (observable() instanceof Array) {
						var values = observable().filter(function (x) { return x !== e.params.data.id; });
						values.push(e.params.data.id);
						observable(values);
					} else if (parameters && parameters.confirmChange) {
						// @ts-ignore
						var value = parameters.key ? e.params.data.item[parameters.key] : e.params.data.item;
						var currentValue = valueAccessor().data();
						var changedValue;
						if (!!value.asKoObservable && (!currentValue || currentValue.innerInstance !== value)) {
							changedValue = value.asKoObservable();
						}
						if (!value.asKoObservable && currentValue !== value) {
							changedValue = value;
						}
						parameters.confirmChange(changedValue).fail(function () {
							$(element).val(null).trigger('change');
						}).done(function () {
							observable(e.params.data.id);
						});
					} else {
						observable(e.params.data.id);
					}
					if (parameters && parameters.onSelect) {
						var selectedValue = e.params.data.id;
						// @ts-ignore
						if (e.params.data.item) {
							// @ts-ignore
							if (e.params.data.item.innerInstance) {
								// @ts-ignore
								selectedValue = e.params.data.item.innerInstance;
							} else {
								// @ts-ignore
								selectedValue = e.params.data.item;
							}
						}
						if (ko.isObservable(parameters.onSelect) && Array.isArray(parameters.onSelect.peek())) {
							parameters.onSelect.push(selectedValue);
						} else {
							parameters.onSelect(selectedValue);
						}
					}
				});
			$(element).on("select2:unselect",
				function (e) {
					$(this).data('unselecting', true);
					if (observable() instanceof Array) {
						observable(observable().filter(function (value) { return value.toString() !== e.params.data.id; }));
						if (parameters && parameters.onSelect && ko.isObservable(parameters.onSelect)) {
							parameters.onSelect(parameters.onSelect().filter(function (value) {
								return parameters.mapDisplayObject(value).id.toString() !== e.params.data.id;
							}));
						}
					} else {
						observable(null);
						if (parameters && parameters.onSelect) {
							parameters.onSelect(null);
						}
					}
				}).on('select2:opening', function (e) {
					if ($(this).data('unselecting')) {
						$(this).removeData('unselecting');
						e.preventDefault();
					}
				}).on('select2:clearing', function (e) {
					//@ts-ignore
					if (e.params.args.data && e.params.args.data.length > 1) {
						observable([]);
					}
				});
			ko.utils.domNodeDisposal.addDisposeCallback(element,
				function () {
					$(element).select2('destroy');
				});
			if (options.default) {
				var valueUnwrapped = ko.unwrap(options.data);
				var item = { id: valueUnwrapped, text: options.default };
				if ($(`option[value='${$.escapeSelector(item.id)}']`, element).length === 0) {
					var option = new Option(item.text, item.id, true, true);
					$(element).append(option).trigger("change");
				} else {
					$(element).val(valueUnwrapped);
					$(element).trigger("change");
				}
			}
		},
		update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
			var bindings = allBindings();
			var valueUnwrapped = ko.utils.unwrapObservable(bindings.value || ko.unwrap(bindings.selectedOptions) || ko.unwrap(valueAccessor().data));
			if ("selectedOptions" in bindings && valueUnwrapped) {
				var converted = [];
				$.each(bindings.selectedOptions(),
					function (key, value) {
						converted.push(value);
					});
				converted.sort().forEach(function (id) {
					if ($(`option[value='${$.escapeSelector(id)}']`, element).length === 0) {
						var option = new Option(id, id, true, true);
						$(element).append(option).trigger("change");
					}
				});
				if (!window._.isEqual($(element).val(), converted)) {
					$(element).val(converted);
					$(element).trigger("change");
				}
			} else if ((Array.isArray(valueUnwrapped) || typeof valueUnwrapped !== "object") &&
				!window._.isEqual(valueUnwrapped, $(element).val()) &&
				valueAccessor().autocompleteOptions) {
				$(element).val(valueUnwrapped);
				$(element).trigger("change");
				var select2data: Select2AutoCompleterResult | Select2AutoCompleterResult[] = {
					id: valueUnwrapped,
					text: valueUnwrapped
				};
				var options = ko.unwrap(valueAccessor());
				var parameters = options.autocompleteOptions;
				var dbQuery = typeof parameters.table === "function" ? parameters.table() : window.database[parameters.table];
				var joins = parameters.joins;
				if (!!joins) {
					joins.forEach(function (join) {
						if (typeof join === "string") {
							dbQuery = dbQuery.include(join);
						} else {
							dbQuery = dbQuery.include2(join.Selector + "." + join.Operation);
						}
					});
				}
				if (ko.unwrap(parameters.noneOption) && valueUnwrapped === ko.unwrap(parameters.noneOption)) {
					var option = new Option("<i>"+Helper.String.getTranslatedString("None")+"</i>", ko.unwrap(parameters.noneOption), true, true);
					$(element).append(option).trigger("change");
					return;
				} else if ($.isFunction(parameters.getElementByIdQuery) && valueUnwrapped !== undefined) {
					let value = Array.isArray(valueUnwrapped) ? (valueUnwrapped.map(x => x === ko.unwrap(parameters.noneOption) ? null : x)) : (valueUnwrapped === ko.unwrap(parameters.noneOption) ? null : valueUnwrapped);
					dbQuery = parameters.getElementByIdQuery(dbQuery, value);
					dbQuery = Array.isArray(value) ? dbQuery.toArray() : dbQuery.first().catch(() => null);
				} else if (valueUnwrapped && !Array.isArray(valueUnwrapped)) {
					var defaultKey = dbQuery.defaultType && dbQuery.defaultType.memberDefinitions.getKeyProperties().length === 1 ? dbQuery.defaultType.memberDefinitions.getKeyProperties()[0].defaultValue : undefined;
					if (valueUnwrapped === defaultKey || (valueUnwrapped === (defaultKey || "").toString())) {
						return;
					}
					dbQuery = dbQuery.find(valueUnwrapped);
				} else {
					return;
				}

				$(element).next(".select2").addClass("select2-active");
				dbQuery.then(function (result) {
					let mapResult;
					if (result instanceof Array) {
						mapResult = (parameters.onResult ? parameters.onResult(result) : $.Deferred().resolve(result).promise()).then(function (result) {
							select2data = $.map(result,
								function (object) {
									return parameters.mapDisplayObject(object);
								});
							if (ko.unwrap(parameters.noneOption) && valueUnwrapped.indexOf(ko.unwrap(parameters.noneOption)) !== -1){
								select2data.unshift({id:ko.unwrap(parameters.noneOption), text:'<i>'+Helper.String.getTranslatedString('None')+'</i>'})
							}
							const select2dataIds = select2data.map(item => item.id);
							let removableElements = [];
							$(element).children('option').each(function (index, val) {
								if (select2dataIds.indexOf(val.value) === -1) {
									removableElements.push(this);
								}
							})
							removableElements.forEach(child => {
								//@ts-ignore
								$(child, element).remove().trigger("change");
							});
							//@ts-ignore
							select2data.forEach(function (item) {
								if ($(`option[value='${$.escapeSelector(item.id)}']`, element).length === 0) {
									const option = new Option(item.text, item.id, true, true);
									$(element).append(option).trigger("change");
								}
							});
						});
					} else {
						if (!result) {
							$(element).next(".select2").removeClass("select2-active")
							return;
						}
						mapResult = (parameters.onResult ? parameters.onResult([result]) : $.Deferred().resolve([result]).promise()).then(function (result) {
							select2data = parameters.mapDisplayObject(result[0])
							//@ts-ignore
							const option = new Option(select2data.text, select2data.id, true, true);
							$(element).append(option).trigger("change");
						});
					}
					mapResult.then(function () {
						if (parameters && parameters.onSelect) {
							var selectedValue = (Array.isArray(select2data) ? select2data : [select2data]).map(function (select2data) {
								if (select2data.item) {
									if (select2data.item.innerInstance) {
										return select2data.item.innerInstance;
									} else {
										return select2data.item;
									}
								} else {
									return select2data.id;
								}
							});
							parameters.onSelect(Array.isArray(select2data) ? selectedValue : selectedValue[0]);
						}
						$(element).next(".select2").removeClass("select2-active");
					});
				});
			} else {
				$(element).val(valueUnwrapped);
				$(element).trigger("change");
			}
		}
	}
})(window.ko);
