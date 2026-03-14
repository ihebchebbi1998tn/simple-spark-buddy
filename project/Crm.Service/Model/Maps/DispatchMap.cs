namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping;
	using NHibernate.Mapping.ByCode;

	public class ServiceOrderDispatchMap : EntityClassMapping<ServiceOrderDispatch>
	{
		public ServiceOrderDispatchMap()
		{
			Schema("SMS");
			Table("ServiceOrderDispatch");

			Id(x => x.Id, map =>
			{
				map.Column("DispatchId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.DispatchNo);
			Property(x => x.OrderId);
			Property(x => x.IsActive);
			Property(x => x.CurrentServiceOrderTimeId);
			ManyToOne(x => x.CurrentServiceOrderTime, m =>
			{
				m.Column("CurrentServiceOrderTimeId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.TimeZone);
			Property(x => x.Date);
			Property(x => x.Remark);
			Property(x => x.RequiredOperations);
			Property(x => x.FollowUpServiceOrder);
			Property(x => x.FollowUpServiceOrderRemark);
			Property(x => x.RejectRemark);
			Property(x => x.CancellationRemark);
			Property(x => x.IsFixed);
			Property(x => x.EndDate);
			Property(x => x.NetWorkMinutes);

			Property(x => x.SignatureJson, map =>
			{
				map.Column("Signature");
				map.Length(524288);
			});
			Property(x => x.SignatureContactName);
			Property(x => x.SignatureDate);
			Property(x => x.SignPrivacyPolicyAccepted);

			Property(x => x.SignatureTechnicianJson, map =>
			{
				map.Column("SignatureTechnician");
				map.Length(524288);
			});
			Property(x => x.SignatureTechnicianName);
			Property(x => x.SignatureTechnicianDate);

			Property(x => x.SignatureOriginatorJson, map =>
			{
				map.Column("SignatureOriginator");
				map.Length(524288);
			});
			Property(x => x.SignatureOriginatorName);
			Property(x => x.SignatureOriginatorDate);
			Property(x => x.CloseDate);

			Property(x => x.ReportSendingError, m => m.Length(int.MaxValue));
			Property(x => x.ReportSent);
			Property(x => x.ReportSaved);
			Property(x => x.ReportSavingError, m => m.Length(int.MaxValue));
			Property(x => x.Diagnosis);
			Property(x => x.LatitudeOnDispatchStart);
			Property(x => x.LongitudeOnDispatchStart);

			Property(x => x.InfoForTechnician);

			Property(x => x.StatusKey, map => map.Column("Status"));
			Property(x => x.RejectReasonKey, map => map.Column("RejectReason"));
			Property(x => x.CancellationReasonKey, map => map.Column("CancellationReason"));
			Property(x => x.ComponentKey, map => map.Column("Component"));

			Property(x => x.DispatchedUsername, m =>
			{
				m.Column("Username");
				m.Insert(false);
				m.Update(false);
			});
			ManyToOne(x => x.DispatchedUser, map =>
			{
				map.Column("Username");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
			});
			ManyToOne(x => x.OrderHead, m =>
			{
				m.Column("OrderId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			this.EntitySet(x => x.ReportRecipients,
				m =>
				{
					m.Key(km => km.Column("DispatchId"));
					m.Inverse(true);
					m.Cascade(Cascade.Remove);
				},
				a => a.OneToMany());
			this.EntitySet(x => x.ServiceOrderTimeDispatches,
				m =>
				{
					m.Key(km => km.Column(nameof(ServiceOrderTimeDispatch.ServiceOrderDispatchId)));
					m.Inverse(true);
				},
				a => a.OneToMany());
			this.EntitySet(x => x.TimePostings,
			m =>
			{
					m.Fetch(CollectionFetchMode.Select);
					m.Lazy(CollectionLazy.Lazy);
					m.BatchSize(100);
					m.Key(km => km.Column("DispatchId"));
					m.Inverse(true);
					m.Cascade(Cascade.Remove);
				},
				a => a.OneToMany());
			this.EntitySet(x => x.ExpensePostings,
				m =>
				{
					m.Key(km => km.Column("DispatchId"));
					m.Inverse(true);
					m.Cascade(Cascade.Remove);
				},
				a => a.OneToMany());
			this.EntitySet(x => x.ServiceOrderMaterial,
				m =>
				{
					m.Key(km => km.Column("DispatchId"));
					m.Inverse(true);
					m.Cascade(Cascade.Remove);
				},
				a => a.OneToMany());

			this.EntitySet(x => x.ServiceOrderErrors,
				m =>
				{
					m.Key(km => km.Column("DispatchId"));
					m.Inverse(true);
					m.Cascade(Cascade.Remove);
				},
				a => a.OneToMany());
		}
	}
}
