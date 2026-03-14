namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class ServiceContractMap : SubclassMapping<ServiceContract>, IDatabaseMapping
	{
		public ServiceContractMap()
		{
			DiscriminatorValue("ServiceContract");

			ManyToOne(x => x.ParentCompany,
				m =>
				{
					m.Column("ParentKey");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
					m.Lazy(LazyRelation.Proxy);
				});

			Join("ServiceContract", join =>
				{
					join.Schema("SMS");
					join.Key(x => x.Column("ContactKey"));

					join.Property(x => x.ExternalReference);
					join.Property(x => x.ContractNo);
					join.Property(x => x.ContractTypeKey);

					join.Property(x => x.StatusKey, m => m.Column("StatusKey"));
					join.Property(x => x.ValidFrom);
					join.Property(x => x.ValidTo);
					join.Property(x => x.InvoiceSpecialConditions, m => m.Length(Int32.MaxValue));
					join.Property(x => x.InternalInvoiceInformation, m => m.Length(Int32.MaxValue));
					join.Property(x => x.Price);
					join.Property(x => x.PriceModificationDate);
					join.Property(x => x.NoPaymentsUntil);
					join.Property(x => x.IncreasedPrice);
					join.Property(x => x.IncreaseByPercent);
					join.Property(x => x.PriceGuaranteedUntil);
					join.Property(x => x.InvoicedUntil);
					join.Property(x => x.LastInvoiceNo);

					join.Property(x => x.FirstAnswerValue, m => m.Column("ReactionTimeFirstAnswerValue"));
					join.Property(x => x.FirstAnswerUnitKey, m => m.Column("ReactionTimeFirstAnswerUnitKey"));
					join.Property(x => x.ServiceCompletedValue, m => m.Column("ReactionTimeServiceCompletedValue"));
					join.Property(x => x.ServiceCompletedUnitKey, m => m.Column("ReactionTimeServiceCompletedUnitKey"));
					join.Property(x => x.ServiceProvisionValue, m => m.Column("BudgetServiceProvisionValue"));
					join.Property(x => x.ServiceProvisionUnitKey, m => m.Column("BudgetServiceProvisionUnitKey"));
					join.Property(x => x.ServiceProvisionPerTimeSpanUnitKey, m => m.Column("BudgetServiceProvisionPerTimeSpanUnitKey"));
					join.Property(x => x.SparePartsValue, m => m.Column("BudgetSparePartsValue"));
					join.Property(x => x.SparePartsUnitKey, m => m.Column("BudgetSparePartsUnitKey"));
					join.Property(x => x.SparePartsPerTimeSpanUnitKey, m => m.Column("BudgetSparePartsPerTimeSpanUnitKey"));
					join.Property(x => x.SparePartsBudgetInvoiceTypeKey, m => m.Column("SparePartsBudgetInvoiceType"));
					join.Property(x => x.LimitTypeKey, m => m.Column("LimitType"));
					join.Property(x => x.PaymentConditionKey, m => m.Column("PaymentCondition"));
					join.Property(x => x.PaymentIntervalKey, m => m.Column("PaymentInterval"));
					join.Property(x => x.PaymentTypeKey, m => m.Column("PaymentType"));
					join.Property(x => x.PriceCurrencyKey, m => m.Column("PriceCurrency"));
					join.Property(x => x.IncreasedPriceCurrencyKey, m => m.Column("IncreasedPriceCurrency"));
					
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
					join.Property(x => x.InvoiceAddressKey);
					join.ManyToOne(x => x.InvoiceAddress, map =>
					{
						map.Column("InvoiceAddressKey");
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

					join.EntitySet(x => x.Installations, m =>
						{
							m.Key(k =>
								{
									k.Column("ServiceContractKey");
									k.NotNullable(true);
								});
							m.Where("IsActive = 1");
							m.Inverse(true);
							m.Lazy(CollectionLazy.Lazy);
							m.Fetch(CollectionFetchMode.Select);
							m.BatchSize(100);
							m.Cascade(Cascade.None);
						}, action => action.OneToMany());

					join.Set(x => x.MaintenancePlans, m =>
						{
							m.Key(k =>
								{
									k.Column("ServiceContractKey");
									k.NotNullable(true);
								});
							m.Fetch(CollectionFetchMode.Select);
							m.BatchSize(100);
							m.Lazy(CollectionLazy.Lazy);
							m.Cascade(Cascade.None);
							m.Inverse(true);
							m.Schema("SMS");
							m.Table("MaintenancePlan");
						}, action => action.OneToMany());

					join.EntitySet(x => x.AddressRelationships, m =>
					{
						m.Key(k =>
						{
							k.Column("ServiceContractKey");
							k.NotNullable(true);
						});
						m.Inverse(true);
						m.Lazy(CollectionLazy.Lazy);
						m.Fetch(CollectionFetchMode.Select);
						m.BatchSize(100);
						m.Cascade(Cascade.None);
					}, action => action.OneToMany());
				});
		}
	}
}