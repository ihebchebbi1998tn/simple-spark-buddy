namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Data.NHibernateProvider.UserTypes;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;
	using NHibernate.Type;

	public class ServiceOrderHeadMap : SubclassMapping<ServiceOrderHead>, IDatabaseMapping
	{
		public ServiceOrderHeadMap()
		{
			DiscriminatorValue("ServiceOrder");

			Join("ServiceOrderHead", join =>
				{
					join.Schema("SMS");
					join.Key(x => x.Column("ContactKey"));
					join.Property(x => x.IsTemplate);
					join.Property(x => x.OrderNo);
					join.Property(x => x.ErrorCodeKey, map => map.Column("ErrorCode"));
					join.Property(x => x.ErrorMessage);
					join.Property(x => x.InstallationId);
					join.Property(x => x.Component);
					join.Property(x => x.Deadline);
					join.Property(x => x.Reported);
					join.Property(x => x.Planned);
					join.Property(x => x.PlannedTime, m => m.Type<TimeAsTimeSpanType>());
					join.Property(x => x.PrematureDate);
					join.Property(x => x.PrematureTime, m => m.Type<TimeAsTimeSpanType>());
					join.Property(x => x.PlannedDateFix);
					join.Property(x => x.Completed, map => map.Column("CompleteDate"));
					join.Property(x => x.Closed, map => map.Column("CloseDate"));
					join.Property(x => x.Latitude);
					join.Property(x => x.Longitude);
					join.Property(x => x.IsGeocoded);
					join.Property(x => x.Name1);
					join.Property(x => x.Name2);
					join.Property(x => x.Name3);
					join.Property(x => x.Street);
					join.Property(x => x.City);
					join.Property(x => x.ZipCode);
					join.Property(x => x.CountryKey);
					join.Property(x => x.RegionKey);
					join.Property(x => x.GeocodingRetryCounter);
					join.Property(x => x.InvoiceRemark);
					join.Property(x => x.TypeKey, map => map.Column("OrderType"));
					join.Property(x => x.PriorityKey, map => map.Column("Priority"));
					join.Property(x => x.IsCostLumpSum);
					join.Property(x => x.IsMaterialLumpSum);
					join.Property(x => x.IsTimeLumpSum);
					join.Property(x => x.InvoicingTypeKey, map => map.Column("InvoicingType"));
					join.Property(x => x.StatusKey, map => map.Column("Status"));
					join.Property(x => x.NoInvoiceReasonKey, map => map.Column("NoInvoiceReason"));
					join.Property(x => x.InvoiceReasonKey, map => map.Column("InvoiceReason"));
					join.Property(x => x.PurchaseOrderNo);
					join.Property(x => x.PurchaseDate);
					join.Property(x => x.CommissionNo);
					join.Property(x => x.MaintenancePlanId);
					join.Property(x => x.MaintenancePlanningRun);
					join.Property(x => x.ServiceContractId);
					join.Property(x => x.ServiceLocationPhone);
					join.Property(x => x.ServiceLocationMobile);
					join.Property(x => x.ServiceLocationFax);
					join.Property(x => x.ServiceLocationEmail);
					join.Property(x => x.ServiceLocationResponsiblePerson);
					join.Property(x => x.CommissioningStatusKey);
					join.Property(x => x.ReportRecipients, map => map.Type<DelimitedStringUserType>());
					join.Property(x => x.ReportSendingError, m => m.Length(int.MaxValue));
					join.Property(x => x.ReportSent);
					join.Property(x => x.ReportSaved);
					join.Property(x => x.ReportSavingError, m => m.Length(int.MaxValue));

					join.Property(p => p.UserGroupKey);
					join.ManyToOne(x => x.UserGroup, map =>
						{
							map.Column("UserGroupKey");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.PreferredTechnicianUsername, map => map.Column("PreferredTechnician"));
					join.ManyToOne(x => x.PreferredTechnician, map =>
						{
							map.Column("PreferredTechnician");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.PreferredTechnicianUsergroupKey, map =>
					{
						map.Column("PreferredTechnicianUsergroup");
					});
					join.ManyToOne(x => x.PreferredTechnicianUsergroup, map =>
						{
							map.Column("PreferredTechnicianUsergroup");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.CustomerContactId);
					join.ManyToOne(x => x.CustomerContact, map =>
						{
							map.Column("CustomerContactId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.PayerId);
					join.ManyToOne(x => x.Payer, map =>
						{
							map.Column("PayerId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.PayerAddressId);
					join.ManyToOne(x => x.PayerAddress, map =>
						{
							map.Column("PayerAddressId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.InvoiceRecipientId);
					join.ManyToOne(x => x.InvoiceRecipient, map =>
						{
							map.Column("InvoiceRecipientId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.InvoiceRecipientAddressId);
					join.ManyToOne(x => x.InvoiceRecipientAddress, map =>
						{
							map.Column("InvoiceRecipientAddressId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.InitiatorId);
					join.ManyToOne(x => x.Initiator, map =>
						{
							map.Column("InitiatorId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.InitiatorPersonId);
					join.ManyToOne(x => x.InitiatorPerson, map =>
						{
							map.Column("InitiatorPersonId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.ServiceCaseKey);
					join.ManyToOne(x => x.ServiceCase, map =>
						{
							map.Column("ServiceCaseKey");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.ServiceObjectId);
					join.ManyToOne(x => x.ServiceObject, map =>
						{
							map.Column("ServiceObjectId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.ServiceOrderTemplateId);
					join.ManyToOne(x => x.ServiceOrderTemplate, map =>
						{
							map.Column("ServiceOrderTemplateId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});

					join.ManyToOne(x => x.AffectedInstallation, map =>
					{
							map.Column("InstallationId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});

					join.ManyToOne(x => x.ServiceContract, map =>
						{
							map.Column("ServiceContractId");
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Cascade(Cascade.None);
							map.Insert(false);
							map.Update(false);
						});

					join.Property(x => x.StationKey);
					
					join.ManyToOne(x => x.Station, map =>
					{
						map.Column("StationKey");
						map.Fetch(FetchKind.Select);
						map.Lazy(LazyRelation.Proxy);
						map.Cascade(Cascade.None);
						map.Insert(false);
						map.Update(false);
					});

					join.EntitySet(x => x.Dispatches, map =>
						{
							map.Key(km => km.Column("OrderId"));
							map.Fetch(CollectionFetchMode.Select);
							map.Lazy(CollectionLazy.Lazy);
							map.BatchSize(100);
							map.Cascade(Cascade.Persist);
							map.Inverse(true);
						}, action => action.OneToMany());

					join.EntitySet(x => x.ServiceOrderStatisticsKeys, map =>
						{
							map.Key(km => km.Column("ServiceOrderId"));
							map.Fetch(CollectionFetchMode.Select);
							map.Lazy(CollectionLazy.Lazy);
							map.BatchSize(100);
							map.Cascade(Cascade.Persist);
							map.Inverse(true);
						}, action => action.OneToMany());

					join.EntitySet(x => x.ServiceOrderTimes, map =>
						{
							map.Key(km => km.Column("OrderId"));
							map.Fetch(CollectionFetchMode.Select);
							map.Lazy(CollectionLazy.Lazy);
							map.BatchSize(100);
							map.Cascade(Cascade.Persist);
							map.Inverse(true);
							map.OrderBy(x => x.PosNo);
						}, action => action.OneToMany());

					join.EntitySet(x => x.ServiceOrderMaterials, map =>
						{
							map.Key(km => km.Column("OrderId"));
							map.Fetch(CollectionFetchMode.Select);
							map.Lazy(CollectionLazy.Lazy);
							map.BatchSize(100);
							map.Cascade(Cascade.Persist);
							map.Inverse(true);
							map.OrderBy(x => x.PosNo);
						}, action => action.OneToMany());

					join.EntitySet(x => x.ServiceOrderTimePostings, map =>
						{
							map.Key(km => km.Column("OrderId"));
							map.Fetch(CollectionFetchMode.Select);
							map.Lazy(CollectionLazy.Lazy);
							map.BatchSize(100);
							map.Cascade(Cascade.Persist);
							map.Inverse(true);
						}, action => action.OneToMany());

					join.EntitySet(x => x.ServiceOrderExpensePostings, map =>
					{
						map.Key(km => km.Column("OrderId"));
						map.Fetch(CollectionFetchMode.Select);
						map.Lazy(CollectionLazy.Lazy);
						map.BatchSize(100);
						map.Cascade(Cascade.Persist);
						map.Inverse(true);
					}, action => action.OneToMany());

					join.EntitySet(x => x.ServiceOrderErrorTypes, map =>
					{
						map.Key(km => km.Column("OrderId"));
						map.Fetch(CollectionFetchMode.Select);
						map.Lazy(CollectionLazy.Lazy);
						map.BatchSize(100);
						map.Cascade(Cascade.Persist);
						map.Inverse(true);
					}, action => action.OneToMany());

					Set(x => x.RequiredSkillKeys, map =>
						{
							map.Schema("SMS");
							map.Table("ServiceOrderSkill");
							map.Key(km => km.Column("ServiceOrderId"));
							map.Fetch(CollectionFetchMode.Select);
							map.Lazy(CollectionLazy.Lazy);
							map.BatchSize(100);
							map.Cascade(Cascade.Persist);
						}, r => r.Element(m => m.Column("SkillKey")));
					Set(x => x.RequiredAssetKeys, map =>
						{
							map.Schema("SMS");
							map.Table("ServiceOrderAsset");
							map.Key(km => km.Column("ServiceOrderId"));
							map.Fetch(CollectionFetchMode.Select);
							map.Lazy(CollectionLazy.Lazy);
							map.BatchSize(100);
							map.Cascade(Cascade.Persist);
						}, r => r.Element(m => m.Column("AssetKey")));
					join.Property(x => x.CloseReason);
					join.Property(x => x.CurrencyKey);
				});
		}
	}
}
