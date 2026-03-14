
; (function (ko) {
	ko.bindingHandlers.select2autocompleter2 = {

		getAllIds: function (arr) {
			const ids = arr.reduce((acc, el, i) => {
				acc[el.id] = i;
				return acc;
			}, {});

			return ids
		},

		getParents: async function (mappedResult, parameters) {
			let queryResults = [];
			const mappedResultId = ko.bindingHandlers.select2autocompleter2.getAllIds(mappedResult);

			for (let id in mappedResultId) {
				if (!queryResults.some(el => el.Id === id)) {
					await ko.bindingHandlers.select2autocompleter2.executeParentsQuery(queryResults, id, parameters);
				}
			}

			return queryResults
		},

		executeParentsQuery: async function (queryResults, id, parameters) {
			const result = await window.database[parameters.table].find(id);
			let nestedProperty = parameters.nestedProperty;

			if (queryResults.some(el => el.Id === result.Id)) {
				return
			}
			else if (result[nestedProperty] !== null && typeof result[nestedProperty] !== "undefined") {
				queryResults.push(result);
				await ko.bindingHandlers.select2autocompleter2.executeParentsQuery(queryResults, result[nestedProperty], parameters);
			}
			else {
				queryResults.push(result);
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
					ko.bindingHandlers.select2autocompleter2.addDepthLevel(obj.childs, depth + 1);
				}
			})
		},

		treeToFlat: function (arr) {
			const flatArray = [];

			arr.forEach(item => {
				flatArray.push(item);
				if (item.childs) {
					flatArray.push(...ko.bindingHandlers.select2autocompleter2.treeToFlat(item.childs));
					delete item.childs;
				}
			})

			return flatArray;
		},

		nestObjects: function (mappedResult, nestedProperty) {
			const ids = ko.bindingHandlers.select2autocompleter2.getAllIds(mappedResult);
			ko.bindingHandlers.select2autocompleter2.addChildsToParent(mappedResult, ids, nestedProperty);
			mappedResult = mappedResult.filter(obj => obj.item[nestedProperty] === null);
			ko.bindingHandlers.select2autocompleter2.addDepthLevel(mappedResult);
			mappedResult = ko.bindingHandlers.select2autocompleter2.treeToFlat(mappedResult);

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
							return new String(' <img class=\"img-responsive waves-effect p-10 i-logo\" src="' + window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/lmobile-block-sad.svg') + '"\>'
								+ '<h4 class="p-l-10 p-r-10">' + window.Helper.String.getTranslatedString("NothingFound") + '</h4>');
						},
						searching: function () {
							return window.Helper.String.getTranslatedString("loading");
						},
						removeAllItems: function () {
							return window.Helper.String.getTranslatedString("Delete");
						},
						errorLoading: function () {
							return new String(' <img class=\"img-responsive waves-effect p-10 i-logo\" src="' + window.Helper.Url.resolveUrl('~/Content/img/lmobile-block-excited.svg') + '"\>'
								+ '<h4 class="p-l-10 p-r-10">' + window.Helper.String.getTranslatedString("UnknownError") +
								window.Helper.String.getTranslatedString("Error_InternalServerError") + '</h4>');
						}
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
						dbquery = customFilter(dbquery, query.term);
					} else {
						if (typeof query.term != "undefined") {
							dbquery = dbquery.filter(function (x) {
								return x.Name.toLowerCase().contains(this.term);
							},
								{ term: query.term.toLowerCase() });
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
						dbquery = dbquery.withInlineCount();
					}

					try {
						let result = await dbquery.take(pageSize).skip((pageNum - 1) * pageSize).toArray();
						if (parameters.onResult) {
							result = await parameters.onResult(result);
						}
						count = result.totalCount || count;
						var mappedResult = result.map(function (object) {
							return parameters.mapDisplayObject(object);
						})
						const property = parameters.nestedProperty ? parameters.nestedProperty.trim() : parameters.nestedProperty
						if (
							parameters.nested === true &&
							parameters.nestedProperty &&
							mappedResult.length > 0 &&
							(property in mappedResult[0]?.item)) {

							if (typeof query.term != "undefined" && query.term !== "") {
								result = await ko.bindingHandlers.select2autocompleter2.getParents(mappedResult, parameters);
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

								mappedResult = ko.bindingHandlers.select2autocompleter2.nestObjects(result, property);
							} else {
								mappedResult = ko.bindingHandlers.select2autocompleter2.nestObjects(mappedResult, property);
							}
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
					} catch (e) {
						window.Log.error(e.data);
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
							return new String('<div class="text-center"> <img class=\"img-responsive waves-effect p-10 i-logo\" src="' + window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/lmobile-block-sad.svg') + '"\>'
								+ '<h4 class="p-l-10 p-r-10">' + text + '</h4></div>');
						},
						searching: function () {
							return new String('<div class="text-center">'
								+ '<div class="autocompleter-preloader preloader pls-blue">'
								+ '<svg class="pl-circular" viewBox="25 25 50 50"><circle class="plc-path" cx="50" cy="50" r="20"></circle></svg>'
								+ "</div>"
								+ "</div>");
						},
						removeAllItems: function () {
							return window.Helper.String.getTranslatedString("Delete");
						},
						errorLoading: function () {
							return new String('<div class="text-center"> <img class=\"img-responsive waves-effect p-10 i-logo\" src="' + window.Helper.Url.resolveUrl('~/Content/img/lmobile-block-excited.svg') + '"\>'
								+ '<h4 class="p-l-10 p-r-10">' + window.Helper.String.getTranslatedString("UnknownError") + '</h4></div>');
						}
					},
					placeholder: ko.unwrap(options.placeholder) || ko.unwrap(parameters.placeholder) || window.Helper.String.getTranslatedString("PleaseSelect"),
					tags: parameters.tags,
					templateResult: parameters.templateResult,
					query: db ? function(query) {
						var context = this;
						var args = arguments;
						var later = function () {
							debounce = null;
							queryFunc.apply(context, args);
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
			var select2 = $(element).data("select2");
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

			//#region select2 bug workaround: Pasting from clipboard when the data is empty
			const dataItems = ko.unwrap(options.data);
			if (dataItems && Array.isArray(dataItems) && dataItems.length == 0) {
				select2Input.on('paste', null, select2, function (e) {
					//Clipboard data
					let pastedText = '';
					if (window.clipboardData && window.clipboardData.getData) {
						pastedText = window.clipboardData.getData('Text');
					} else if (e.originalEvent.clipboardData && e.originalEvent.clipboardData.getData) {
						pastedText = e.originalEvent.clipboardData.getData('text/plain');
					}
					else {
						return;
					}
					e.preventDefault();
					e.stopPropagation();

					//Paste positioning
					const $this = $(this);

					const selectionStart = $this.prop('selectionStart');
					const selectionEnd = $this.prop('selectionEnd');
					const v = $this.val() ?? "";
					const textBefore = v.substring(0, selectionStart);
					const textAfter = v.substring(selectionEnd, v.length);

					//triggering
					$this.val(window.Helper.String.getTranslatedString("PleaseSelect")).trigger("input").val(textBefore + pastedText + textAfter).trigger("input");

					//Fixing cursor position
					const cursorPosition = textBefore.length + pastedText.length;
					$this.prop({
						'selectionStart': cursorPosition,
						'selectionEnd': cursorPosition
					});
				});
			}
			//#endregion
			
			$(element).on('select2:open', function () {
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
			};
			$(element).on("select2:select",
				function (e) {
					if (observable() instanceof Array) {
						if(observable().every(x => x instanceof $data.Entity)) {
							let values = observable().filter(function (x) { return x.Id !== e.params.data.id; });
							// @ts-ignore
							values.push(e.params.data.item);
							observable(values);
						} else {
							var values = observable().filter(function (x) {
								return x !== e.params.data.id;
							});
							values.push(e.params.data.id);
							observable(values);
						}
					} else if (parameters && parameters.confirmChange) {
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
						if (e.params.data.item) {
							if (e.params.data.item.innerInstance) {
								selectedValue = e.params.data.item.innerInstance;
							} else {
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
					if (Array.isArray(observable())) {
						if(observable().length > 0 && observable().every(x => x instanceof $data.Entity)) {
							observable(observable().filter(function (value) { return value.Id !== e.params.data.id; }));
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
				});
			ko.utils.domNodeDisposal.addDisposeCallback(element,
				function () {
					$(element).select2('destroy');
				});
			if (options.default) {
				var valueUnwrapped = ko.unwrap(options.data);
				var item = { id: valueUnwrapped, text: options.default };
				if ($("option[value=" + item.id + "]", element).length === 0) {
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
			if(Array.isArray(valueUnwrapped) && valueUnwrapped.length > 0 && valueUnwrapped.every(x => x instanceof $data.Entity)) {
				let converted = [];
				$.each(valueUnwrapped,
					function (key, value) {
						let mappedItem = valueAccessor().autocompleteOptions.mapDisplayObject ? valueAccessor().autocompleteOptions.mapDisplayObject(value) : value;
						converted.push(mappedItem);
					});
				converted.sort().forEach(function (item) {
					if ($(`option[value='${item.id}']`, element).length === 0) {
						let option = new Option(item.text, item.id, true, true);
						$(element).append(option).trigger("change");
					}
				});
				if (!window._.isEqual($(element).val(), converted.map(x => x.id))) {
					$(element).val(converted.map(x => x.id));
					$(element).trigger("change");
				}
			} else if ("selectedOptions" in bindings && valueUnwrapped) {
				var converted = [];
				$.each(bindings.selectedOptions(),
					function (key, value) {
						converted.push(value);
					});
				//converted.sort().forEach(function (id) {
				//	if ($(`option[value='${id}']`, element).length === 0) {
				//		var option = new Option(id, id, true, true);
				//		$(element).append(option).trigger("change");
				//	}
				//});

				let old = $(element).val();
				let options = ko.unwrap(valueAccessor());
				let parameters = options.autocompleteOptions;

				$(element).empty();

				if (parameters) {
					converted.forEach(function (id) {
						let text = parameters.mapDisplayObject ? parameters.mapDisplayObject(id) : id;
						text = text.text ?? text;
						let option = new Option(text, id, true, true);
						$(element).append(option).trigger("change");
					});
				}
				else if (valueAccessor().data) {
					//this block needs to be changed for non string array data
					var items = ko.unwrap(valueAccessor().data);
					let mapDisplayObject = valueAccessor().mapDisplayObject ?? valueAccessor().data["mapDisplayObject"];

					converted.forEach(function (id) {
						let text = mapDisplayObject ? mapDisplayObject(id) : id;
						let option = new Option(text, id, true, true);
						$(element).append(option).trigger("change");
					});

					//the rest
					items.filter(x => !converted.includes(x)).forEach(function (id) {
						let text = mapDisplayObject ? mapDisplayObject(id) : id;
						var option = new Option(text, id, false, false);
						$(element).append(option).trigger("change");
					});

				}

				//if (!window._.isEqual($(element).val(), converted)) {
				//	$(element).val(converted);
				//	$(element).trigger("change");
				//}

				if (!window._.isEqual(old, converted)) {
					$(element).val(converted);
					$(element).trigger("change");
				}

			} else if ((Array.isArray(valueUnwrapped) || typeof valueUnwrapped !== "object") &&
				!window._.isEqual(valueUnwrapped, $(element).val()) &&
				valueAccessor().autocompleteOptions) {
				var select2data = {
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
				if ($.isFunction(parameters.getElementByIdQuery) && valueUnwrapped !== undefined) {
					dbQuery = parameters.getElementByIdQuery(dbQuery, valueUnwrapped);
					dbQuery = Array.isArray(valueUnwrapped) ? dbQuery.toArray() : dbQuery.first();
				} else if (valueUnwrapped && !Array.isArray(valueUnwrapped)) {
					var defaultKey = dbQuery.defaultType && dbQuery.defaultType.memberDefinitions.getKeyProperties().length === 1 ? dbQuery.defaultType.memberDefinitions.getKeyProperties()[0].defaultValue : undefined;
					if (valueUnwrapped === defaultKey) {
						return;
					}
					dbQuery = dbQuery.find(valueUnwrapped);
				} else {
					return;
				}

				$(element).next(".select2").addClass("select2-active");
				dbQuery.then(function (result) {
					var mapResult;
					if (result instanceof Array) {
						mapResult = (parameters.onResult ? parameters.onResult(result) : new $.Deferred().resolve(result).promise()).then(function (result) {
							select2data = $.map(result,
								function (object) {
									return parameters.mapDisplayObject(object);
								});
							//@ts-ignore
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
							select2data.forEach(function (item) {
								if ($("option[value=" + item.id + "]", element).length === 0) {
									var option = new Option(item.text, item.id, true, true);
									$(element).append(option).trigger("change");
								}
							});
						});
					} else {
						mapResult = (parameters.onResult ? parameters.onResult([result]) : new $.Deferred().resolve([result]).promise()).then(function (result) {
							select2data = parameters.mapDisplayObject(result[0]);
							var option = new Option(select2data.text, select2data.id, true, true);
							$(element).append(option).trigger("change");
						});
					};
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
