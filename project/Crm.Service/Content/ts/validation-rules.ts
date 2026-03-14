window.ko.validationRules.add("CrmService_InstallationPos", function (entity: Crm.Service.Rest.Model.ObservableCrmService_InstallationPos) {
	entity.Quantity.extend({
		validation:
			[
				{
					rule: "required",
					message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Quantity")),
					params: true
				},
				{
					rule: "min",
					params: 0,
					message: window.Helper.String.getTranslatedString("RuleViolation.Greater").replace("{0}", window.Helper.String.getTranslatedString("Quantity")).replace("{1}", "0")
				}
			]
	});
	entity.RemoveDate.extend({
		validation:
			{
				message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
					.replace("{0}", window.Helper.String.getTranslatedString("InstallDate"))
					.replace("{1}", window.Helper.String.getTranslatedString("RemoveDate")),
				onlyIf: function () {
					return !!entity.InstallDate();
				},
				validator: function (val) {
					return !window.moment(entity.InstallDate()).isAfter(val);
				}
			}
	});
	entity.WarrantyEndSupplier.extend({
		validation:
			{
				message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
					.replace("{0}", window.Helper.String.getTranslatedString("WarrantyStartSupplier"))
					.replace("{1}", window.Helper.String.getTranslatedString("WarrantyEndSupplier")),
				onlyIf: function () {
					return !!entity.WarrantyStartSupplier();
				},
				validator: function (val) {
					return !window.moment(entity.WarrantyStartSupplier()).isAfter(val);
				}
			}
	});
	entity.WarrantyEndCustomer.extend({
		validation:
			{
				message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
					.replace("{0}", window.Helper.String.getTranslatedString("WarrantyStartCustomer"))
					.replace("{1}", window.Helper.String.getTranslatedString("WarrantyEndCustomer")),
				onlyIf: function () {
					return !!entity.WarrantyStartCustomer();
				},
				validator: function (val) {
					return !window.moment(entity.WarrantyStartCustomer()).isAfter(val);
				}
			}
	});
	entity.SerialNo.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("T_SerialNr")),
			onlyIf: function (val) {
				return entity.IsSerial();
			}
		}
	});
	entity.ArticleId.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Material")),
			onlyIf: function() {
				return entity.ItemNo() == null || entity.ItemNo().length === 0;
			}
		}
	});
});
window.ko.validationRules.add("CrmService_Installation",
	function (entity: Crm.Service.Rest.Model.ObservableCrmService_Installation) {
		entity.InstallationNo.extend({
			unique: {
				params: [window.database.CrmService_Installation, 'InstallationNo', entity.Id],
				onlyIf: function () {
					return entity.innerInstance.entityState === $data.EntityState.Added;
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Unique")
					.replace("{0}", window.Helper.String.getTranslatedString("InstallationNo"))
			}
		});
		entity.ManufactureDate.extend({
			validation: [
				{
					message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
						.replace("{0}", window.Helper.String.getTranslatedString("ManufactureDate"))
						.replace("{1}", window.Helper.String.getTranslatedString("KickoffDate")),
					onlyIf: function () {
						return !!entity.KickOffDate();
					},
					validator: function (val) {
						return !window.moment(val).isAfter(entity.KickOffDate());
					}
				},
				{
					message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
						.replace("{0}", window.Helper.String.getTranslatedString("ManufactureDate")).replace("{1}",
							window.Helper.String.getTranslatedString("WarrantyFrom")),
					onlyIf: function () {
						return !!entity.WarrantyFrom();
					},
					validator: function (val) {
						return !window.moment(val).isAfter(entity.WarrantyFrom());
					}
				},
				{
					message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
						.replace("{0}", window.Helper.String.getTranslatedString("ManufactureDate")).replace("{1}",
							window.Helper.String.getTranslatedString("WarrantyUntil")),
					onlyIf: function () {
						return !!entity.WarrantyUntil();
					},
					validator: function (val) {
						return !window.moment(val).isAfter(entity.WarrantyUntil());
					}
				}
			]
		});
		entity.WarrantyFrom.extend({
			validation:
				{
					message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
						.replace("{0}", window.Helper.String.getTranslatedString("WarrantyFrom"))
						.replace("{1}", window.Helper.String.getTranslatedString("WarrantyUntil")),
					onlyIf: function () {
						return !!entity.WarrantyUntil();
					},
					validator: function (val) {
						return !window.moment(val).isAfter(entity.WarrantyUntil());
					}
				}

		});
	});
window.ko.validationRules.add("CrmService_ReplenishmentOrderItem", function (entity: Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrderItem) {
	entity.ArticleId.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Material"))
		}
	});
	entity.Quantity.extend({
		validation: {
			validator: function (val) {
				return val >= 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
				.replace("{0}", window.Helper.String.getTranslatedString("Quantity")),
		}
	});
	entity.Quantity.extend({
		validation: {
			validator: function (val) {
				return val > 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.Required")
				.replace("{0}", window.Helper.String.getTranslatedString("Quantity")),
		}
	});
});
window.ko.validationRules.add("CrmService_ServiceOrderDispatch", function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch) {
	entity.DispatchedUser.extend({
		required: false,
		maxLength: {
			onlyIf: () => false
		}
	});
	entity.SignatureContactName.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required")
				.replace("{0}", window.Helper.String.getTranslatedString("SignatureContactName")),
			onlyIf: function () {
				return !!entity.SignatureJson();
			}
		}
	});
	entity.SignatureJson.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("MissingCustomerSignature"),
			onlyIf: function () {
				return window.Crm.Service.Settings.Service.Dispatch.Requires.CustomerSignature && ["ClosedNotComplete", "ClosedComplete"].indexOf(entity.StatusKey()) !== -1;
			}
		}
	});
	entity.SignPrivacyPolicyAccepted.extend({
		validation: {
			validator: function (val) {
				return val === true;
			},
			message: window.Helper.String.getTranslatedString("PleaseAcceptDataPrivacyPolicy"),
			onlyIf: function () {
				return window.Crm.Service.Settings.Service.Dispatch.Show.PrivacyPolicy && !!entity.SignatureJson();
			}
		}
	});
	entity.SignatureOriginatorName.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required")
				.replace("{0}", window.Helper.String.getTranslatedString("SignatureOriginatorName")),
			onlyIf: function () {
				return !!entity.SignatureOriginatorJson();
			}
		}
	});
	entity.StatusKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Status"))
		}
	});
	entity.RejectReasonKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("RejectReason")),
			onlyIf: function () {
				return entity.StatusKey() === "Rejected";
			}
		}
	});
	entity.Username.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required")
				.replace("{0}", window.Helper.String.getTranslatedString("Technician"))
		}
	});
	entity.DispatchNo.extend({
		unique: {
			params: [window.database.CrmService_ServiceOrderDispatch, 'DispatchNo', entity.Id],
			onlyIf: function () {
				return entity.innerInstance.entityState === $data.EntityState.Added;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.Unique")
				.replace("{0}", window.Helper.String.getTranslatedString("DispatchNo"))
		}
	});
	if (!!entity.ServiceOrder() && !!entity.ServiceOrder().Installation()) {
		ko.validation.addRule(entity.ServiceOrder().Installation().StatusKey,
			{
				rule: "required",
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("InstallationStatus")),
				params: true
			});
	}
	entity.EndDate.extend({
		validation: {
			message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeBeforeDate")
				.replace("{0}", window.Helper.String.getTranslatedString("EndDateCaption"))
				.replace("{1}", window.Helper.String.getTranslatedString("StartDate").toLowerCase()),
			validator: function (val) {
				if (!!entity.EndDate() && !!entity.Date())
					return window.moment(val).isAfter(entity.Date());
				else
					return true;
			}
		}
	});
});
window.ko.validationRules.add("CrmService_ServiceOrderHead", function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead) {
	entity.CustomerContactId.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Customer")),
			onlyIf: function () {
				return !entity.IsTemplate();
			}
		}
	});
	entity.OrderNo.extend({
		unique: {
			params: [window.database.CrmService_ServiceOrderHead, 'OrderNo', entity.Id],
			message: window.Helper.String.getTranslatedString("RuleViolation.Unique")
				.replace("{0}", window.Helper.String.getTranslatedString("OrderNo"))
		},
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("OrderNo")),
			onlyIf: function() {
				return !window.Crm.Service.Settings.ServiceOrder.OrderNoIsGenerated && !entity.IsTemplate();
			}
		}
	});
	entity.ErrorMessage.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ErrorMessage")),
			onlyIf: function () {
				return !entity.IsTemplate();
			}
		}
	});
	entity.Reported.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Reported")),
			onlyIf: function () {
				return !entity.IsTemplate();
			}
		}
	});
	entity.StatusKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Status"))
		}
	});
	entity.TypeKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ServiceOrderType"))
		}
	});
	entity.PrematureDate.extend({
		validation:
		{
			message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
				.replace("{0}", window.Helper.String.getTranslatedString("PrematureDate"))
				.replace("{1}", window.Helper.String.getTranslatedString("Planned")),
			onlyIf: function () {
				return !!entity.Planned() && !!entity.PrematureDate();
			},
			validator: function (val) {
				return window.moment(entity.Planned()).isAfter(val);
			}
		}
	});
});
window.ko.validationRules.add("CrmService_ServiceOrderMaterial",
	function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) {
		const serialQuantityValidator = (serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, value: number): boolean => {
			if (!serviceOrderMaterial.IsSerial()) {
				return true;
			}
			return value === 1 || value === 0;
		};
		const serialNo = {
			requiresOneOf: {
				params: [entity.SerialNo, entity.SerialId],
				message: window.Helper.String.getTranslatedString("SerialNoRequired"),
				onlyIf: function () {
					return entity.Quantity() > 0 && entity.IsSerial() && entity.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used;
				}
			}
		};
		entity.SerialNo.extend(serialNo);
		entity.SerialId.extend(serialNo);
		const previousSerialNo = {
			requiresOneOf: {
				params: [entity.PreviousSerialNo, entity.NoPreviousSerialNoReasonKey],
				message: window.Helper.String.getTranslatedString("RuleViolation.PreviousSerialNoRequired"),
				onlyIf: function () {
					return entity.Quantity() > 0 && entity.IsSerial() && entity.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used;
				}
			}
		};
		entity.NoPreviousSerialNoReasonKey.extend(previousSerialNo);
		entity.PreviousSerialNo.extend(previousSerialNo);
		entity.BatchNo.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("BatchNo")),
				onlyIf: function () {
					return entity.IsBatch();
				}
			}
		});
		entity.Quantity.extend({
			validation: {
				validator: function (val) {
					return val >= 0;
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
					.replace("{0}", window.Helper.String.getTranslatedString("Quantity"))
			}
		});
		entity.Quantity.extend({
			validation: {
				validator: function (val) {
					if (entity.ParentServiceOrderMaterialId() === null) {
						return val > 0;
					}
					return true;
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("Quantity"))
			}
		});
		entity.Quantity.extend({
			validation: {
				validator: function (val) {
					return val < 10000000000000000;
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.MaxValue").replace("{0}", window.Helper.String.getTranslatedString("Quantity"))
			}
		});
		entity.Quantity.extend({
			validation: {
				validator: val => window.Helper.ServiceOrderMaterial.quantityValidator(val, entity),
				message: () => window.Helper.String.getTranslatedString("RuleViolation.RespectQuantityStep")
					.replace("{0}", window.Helper.String.getTranslatedString("Quantity"))
					.replace("{1}", (entity.QuantityUnitEntry() ?? entity.Article()?.QuantityUnitEntry())?.QuantityStep() + ""),
				onlyIf: () => entity.Article()
			}
		});
		entity.Quantity.extend({
			validation: {
				validator: val => serialQuantityValidator(entity, val),
				message: window.Helper.String.getTranslatedString("RuleViolation.QuantityForArticleWithSerialOneOrZero"),
				onlyIf: () => {
					return entity.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used;
				}
			}
		});
		entity.QuantityUnitEntryKey.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("QuantityUnit")),
				onlyIf: () => entity.Article() && entity.Article().QuantityUnitEntryKey()
			}
		});
		entity.ArticleId.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Material")),
				onlyIf: function () {
					return entity.ItemNo() == null || entity.ItemNo().length === 0;
				}
			}
		});
		entity.Discount.extend({
			validation: {
				validator: function (val) {
					return val >= 0;
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
					.replace("{0}", window.Helper.String.getTranslatedString("Discount")),
			}
		});
		entity.Discount.extend({
			validation: {
				validator: function (val) {
					return entity.DiscountType() === window.Crm.Article.Model.Enums.DiscountType.Percentage ? val <= 100 : val <= entity.Price();
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.DiscountBiggerThanPrice")
			}
		});
	});
window.ko.validationRules.add("CrmService_ServiceOrderTimePosting",
	function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting) {
		entity.ArticleId.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Article"))
			}
		});
		entity.Distance.extend({
			validation: {
				validator: function (val: number) {
					return val >= 0;
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
					.replace("{0}", window.Helper.String.getTranslatedString("Distance")),
				onlyIf: () => entity.Distance() !== null
			}
		});
		entity.Duration.extend({
			validation: {
				validator: function (val) {
					return val && window.moment.duration(val).isValid() && window.moment.duration(val).asMinutes() > 0;
				},
				message: `${window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Duration"))} ${window.Helper.String.getTranslatedString("RuleViolation.Greater").replace("{0}", window.Helper.String.getTranslatedString("Duration")).replace("{1}", "0")}`
			}
		});
		entity.From.extend({
			validation: {
				async: true,
				validator: window.Helper.TimeEntry.OverlappingTimeEntryValidator.getValidationFunction(entity),
				onlyIf: function () {
					return entity.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used;
				}
			}
		});
		entity.Username.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Technician")),
				onlyIf: function () {
					return entity.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used;
				}
			}
		});
	});

window.ko.validationRules.add("CrmService_ServiceOrderExpensePosting",
	function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting) {
		entity.ArticleId.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Article")),
				onlyIf: function () {
					return window.Crm.Service.Settings.ServiceOrderExpensePosting.UseArticleAsExpenseType;
				}
			}
		});
		entity.ExpenseTypeKey.extend({
			 required: {
			 params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ExpenseType")),
			onlyIf: function () {
				return window.Crm.Service.Settings.ServiceOrderExpensePosting.UseArticleAsExpenseType === false;
			}
						}
				});
		entity.Date.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Date"))
			}
		});

		entity.ResponsibleUser.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ResponisbleUser"))
			}
		});
		entity.Amount.extend({
			validation:
				[
					{
						validator: function (val) {
							return val && val !== 0;
						},
						message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Amount"))
					},
					{
						validator: function (val) {
							return val >= 0;
						},
						message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative").replace("{0}", window.Helper.String.getTranslatedString("Amount"))
					},
					{
						rule: "max",
						params: 10000000,
						message: window.Helper.String.getTranslatedString("RuleViolation.MaxValue").replace("{0}", window.Helper.String.getTranslatedString("Amount"))
					}
				]
		});
	});

window.ko.validationRules.add("CrmService_ServiceOrderErrorType",
	function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType) {
		entity.StatisticsKeyFaultImageKey.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("StatisticsKeyFaultImage"))
			}
		});
		entity.StatisticsKeyFaultImageKey.extend({
			validation: {
				async: true,
				validator: function (statisticsKeyFaultImage, params, callback) {
					if (!statisticsKeyFaultImage) {
						callback(true);
						return;
					}
					window.database.CrmService_ServiceOrderErrorType
						.filter(function (serviceOrderErrorType) {
							return serviceOrderErrorType.StatisticsKeyFaultImageKey === this.statisticsKeyFaultImage && serviceOrderErrorType.DispatchId === this.DispatchId && ((serviceOrderErrorType.OrderId === this.OrderId && this.OrderId != null) || (serviceOrderErrorType.ServiceOrderTimeId === this.ServiceOrderTimeId && this.ServiceOrderTimeId != null) || (serviceOrderErrorType.ServiceCaseId == this.ServiceCaseId && this.ServiceCaseId !== null));
						}, { statisticsKeyFaultImage: statisticsKeyFaultImage, DispatchId: entity.DispatchId, OrderId: entity.OrderId, ServiceOrderTimeId: entity.ServiceOrderTimeId, ServiceCaseId: entity.ServiceCaseId })
						.toArray()
						.then(function (serviceOrderErrorTypes) {
							callback(serviceOrderErrorTypes.length === 0 || serviceOrderErrorTypes[0].Id === entity.Id());
						});
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("StatisticsKeyFaultImage"))
			}
		});
		entity.IsMainErrorType.extend({
			validation: {
				async: true,
				validator: function (mainError, params, callback) {
					if (!mainError) {
						callback(true);
						return;
					}
					window.database.CrmService_ServiceOrderErrorType
						.filter(function (serviceOrderErrorType) {
							return serviceOrderErrorType.IsMainErrorType === this.mainError && ((serviceOrderErrorType.OrderId === this.OrderId && this.OrderId != null) || (serviceOrderErrorType.ServiceCaseId == this.ServiceCaseId && this.ServiceCaseId != null));
						}, { mainError: mainError, OrderId: entity.OrderId, ServiceCaseId: entity.ServiceCaseId })
						.toArray()
						.then(function (serviceOrderErrorTypes) {
							callback(serviceOrderErrorTypes.length === 0 || serviceOrderErrorTypes[0].Id === entity.Id());
						});
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("IsMainErrorType"))
			}
		});
	});


window.ko.validationRules.add("CrmService_ServiceOrderErrorCause",
	function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause) {
		entity.StatisticsKeyCauseKey.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("StatisticsKeyCause"))
			}
		});
		entity.StatisticsKeyCauseKey.extend({
			validation: {
				async: true,
				validator: function (errorCause, params, callback) {
					if (!errorCause) {
						callback(true);
						return;
					}
					window.database.CrmService_ServiceOrderErrorCause
						.filter(function (serviceOrderErrorCause) {
							return this.errorCauseKey === serviceOrderErrorCause.StatisticsKeyCauseKey && this.errorType === serviceOrderErrorCause.ServiceOrderErrorTypeId;
						}, { errorCauseKey: entity.StatisticsKeyCauseKey, errorType: entity.ServiceOrderErrorTypeId })
						.toArray()
						.then(function (serviceOrderErrorTypes) {
							callback(serviceOrderErrorTypes.length === 0 || serviceOrderErrorTypes[0].Id === entity.Id());
						});
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("StatisticsKeyCause"))
			}
		});
	});
window.ko.validationRules.add("CrmService_ServiceContract", function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceContract) {
	entity.Name.extend({
		validation: {
			async: true,
			validator: function (contractNo, params, callback) {
				if (!contractNo) {
					callback(true);
					return;
				}
				window.database.CrmService_ServiceContract
					.filter(function (serviceContract) {
						return serviceContract.ContractNo === this.contractNo;
					}, {contractNo: contractNo})
					.toArray()
					.then(function (serviceContracts) {
						callback(serviceContracts.length === 0 || serviceContracts[0].Id === entity.Id());
					});
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("ContractNo"))
		}
	});
	entity.FirstAnswerUnitKey.extend({
		validation: {
			validator: function (val) {
				return val;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("FirstAnswerUnitKey")),
			onlyIf: function () {
				return entity.FirstAnswerValue() && entity.FirstAnswerValue() != 0;
			}
		}
	});
	entity.ServiceCompletedUnitKey.extend({
		validation: {
			validator: function (val) {
				return val;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ServiceCompletedUnitKey")),
			onlyIf: function () {
				return entity.ServiceCompletedValue() && entity.ServiceCompletedValue() != 0;
			}
		}
	});
	entity.Price.extend({
		validation: {
			validator: function (val) {
				return val >= 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
				.replace("{0}", window.Helper.String.getTranslatedString("Price")),
		}
	});
	entity.ServiceCompletedValue.extend({
		validation: {
			validator: function (val) {
				return val >= 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
				.replace("{0}", window.Helper.String.getTranslatedString("ServiceCompletedValue")),
		}
	});
	entity.FirstAnswerValue.extend({
		validation: {
			validator: function (val) {
				return val >= 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
				.replace("{0}", window.Helper.String.getTranslatedString("FirstAnswerValue")),
		}
	});
	entity.ServiceProvisionValue.extend({
		validation: {
			validator: function (val) {
				return val >= 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
				.replace("{0}", window.Helper.String.getTranslatedString("ServiceProvisionValue")),
		}
	});
	entity.SparePartsValue.extend({
		validation: {
			validator: function (val) {
				return val >= 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
				.replace("{0}", window.Helper.String.getTranslatedString("SparePartsValue")),
		}
	});
	entity.PriceCurrencyKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("PriceCurrencyKey")),
			onlyIf: function () {
				return !!entity.Price();
			}
		}
	});
	entity.ServiceProvisionUnitKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ServiceProvisionUnitKey")),
			onlyIf: function () {
				return !!entity.ServiceProvisionValue();
			}
		}
	});
	entity.ServiceProvisionPerTimeSpanUnitKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ServiceProvisionPerTimeSpanUnitKey")),
			onlyIf: function () {
				return !!entity.ServiceProvisionValue();
			}
		}
	});
	entity.SparePartsUnitKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("SparePartsUnitKey")),
			onlyIf: function () {
				return !!entity.SparePartsValue();
			}
		}
	});
	entity.SparePartsBudgetInvoiceTypeKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("SparePartsBudgetInvoiceTypeKey")),
			onlyIf: function () {
				return !!entity.SparePartsValue();
			}
		}
	});
	entity.SparePartsPerTimeSpanUnitKey.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("SparePartsPerTimeSpanUnitKey")),
			onlyIf: function () {
				return !!entity.SparePartsValue();
			}
		}
	});
	entity.ValidFrom.extend({
		validation:
			{
				message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
					.replace("{0}", window.Helper.String.getTranslatedString("ValidFrom"))
					.replace("{1}", window.Helper.String.getTranslatedString("ValidTo")),
				onlyIf: function () {
					return !!entity.ValidTo();
				},
				validator: function (val) {
					return !window.moment(val).isAfter(entity.ValidTo());
				}
			}
	});
	entity.Price.extend({
		validation: {
			validator: function (val) {
				return val >= 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
				.replace("{0}", window.Helper.String.getTranslatedString("Price")),
		}
	});
	entity.ValidTo.extend({
		validation: {
			message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
				.replace("{0}", window.Helper.String.getTranslatedString("ValidFrom"))
				.replace("{1}", window.Helper.String.getTranslatedString("ValidTo").toLowerCase()),
			validator: function (val) {
				if (!!entity.ValidTo() && !!entity.ValidFrom())
					return window.moment(val).add(1, "day").isAfter(entity.ValidFrom());
				else
					return true;
			}
		}
	});
});
window.ko.validationRules.add("CrmService_MaintenancePlan", function (entity: Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan) {
	entity.RhythmValue.extend({
		validation: {
			validator: function (val) {
				return val >= 0;
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative")
				.replace("{0}", window.Helper.String.getTranslatedString("Interval"))
		}
	});
});
window.ko.validationRules.add("CrmService_ServiceObject", function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceObject) {
	entity.ObjectNo.extend({
		unique: {
			params: [window.database.CrmService_ServiceObject, 'ObjectNo', entity.Id],
			message: window.Helper.String.getTranslatedString("RuleViolation.Unique")
				.replace("{0}", window.Helper.String.getTranslatedString("ObjectNo"))
		}
	});
});
window.ko.validationRules.add("CrmService_ServiceCase", function (entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase) {
	entity.ServiceCaseNo.extend({
		unique: {
			params: [window.database.CrmService_ServiceCase, 'ServiceCaseNo', entity.Id],
			onlyIf: function () {
				return entity.innerInstance.entityState === $data.EntityState.Added && !!entity.ServiceCaseNo();
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.Unique")
				.replace("{0}", window.Helper.String.getTranslatedString("ServiceCaseNo"))
		},
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ServiceCaseNo")),
			onlyIf: function() {
				return !window.Crm.Service.Settings.ServiceCase.ServiceCaseNoIsGenerated;
			}
		}
	});

});
window.ko.validationRules.add("Main_User", function (entity: Main.Rest.Model.ObservableMain_User) {
	let extensionValues = ko.unwrap(entity.ExtensionValues);
	if (extensionValues) {
		extensionValues.DefaultLocationNo.extend({
			unique: {
				params: [window.database.Main_User, 'ExtensionValues.DefaultLocationNo', entity.Id, "DefaultLocationNo"],
				onlyIf: function () {
					return window.Crm.Service.Settings.UserExtension.OnlyUnusedLocationNosSelectable;
				}
			}
		});
	}
});
window.ko.validationRules.add("CrmService_InstallationCompanyRelationship", function (entity: Crm.Service.Rest.Model.ObservableCrmService_InstallationCompanyRelationship) {
	entity.ParentId.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Installation")),
			onlyIf: function () {
				return !!entity.ParentId();
			}
		}
	});
	entity.ChildId.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Company")),
			onlyIf: function () {
				return !!entity.ChildId();
			}
		}
	})
});
window.ko.validationRules.add("CrmService_InstallationPersonRelationship", function (entity: Crm.Service.Rest.Model.ObservableCrmService_InstallationPersonRelationship) {
	entity.ParentId.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Installation")),
			onlyIf: function () {
				return !!entity.ParentId();
			}
		}
	});
	entity.ChildId.extend({
		required: {
			params: true,
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Person")),
			onlyIf: function () {
				return !!entity.ChildId();
			}
		}
	})
});

window.ko.validationRules.add("CrmService_ServiceContractType", function (entity: Crm.Service.Rest.Model.Lookups.ObservableCrmService_ServiceContractType) {
	entity.GracePeriodInDays.extend({
		validation:
			[
				{
					rule: "min",
					params: 0,
					message: window.Helper.String.getTranslatedString("RuleViolation.Greater").replace("{0}", window.Helper.String.getTranslatedString("GracePeriodInDays")).replace("{1}", "-1")
				}
			]
	});
});