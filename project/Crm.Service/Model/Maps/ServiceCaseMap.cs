namespace Crm.Service.Model.Maps
{
	using Crm.Library.BaseModel.Interfaces;
		using Crm.Library.BaseModel.Mappings;

		using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;
	
	public class ServiceCaseMap : SubclassMapping<ServiceCase>, IDatabaseMapping
	{
		public ServiceCaseMap()
		{
			DiscriminatorValue("ServiceCase");
			
			Join(
				"ServiceNotifications",
				join =>
				{
					join.Schema("SMS");

					join.Key(x => x.Column("ContactKey"));
					join.Property(x => x.AffectedCompanyKey);
					join.Property(x => x.AffectedInstallationKey);
					join.Property(x => x.CompletionDate);
					join.Property(x => x.CompletionServiceOrderId);
					join.Property(x => x.CompletionUser);
					join.Property(x => x.ContactPersonId, map => map.Column("ContactPersonKey"));
					join.Property(x => x.OriginatingServiceOrderId);
					join.Property(x => x.OriginatingServiceOrderTimeId);
					join.Property(x => x.ServiceObjectId);
					join.Property(x => x.ServiceOrderTimeId);
					join.Property(x => x.UserGroupKey);
					join.Property(x => x.ServiceCaseNo);
					join.Property(x => x.ServiceCaseTemplateId);
					join.Property(x => x.LegacyId);
					join.Property(x => x.ErrorMessage);
					join.Property(x => x.Reported);
					join.Property(x => x.Planned);
					join.Property(x => x.Executed);
					join.Property(x => x.PickedUpDate);
					join.Property(x => x.ServiceCaseCreateDate);
					join.Property(x => x.ServiceCaseCreateUser);
					join.Property(x => x.ErrorCodeKey, map => map.Column("ErrorCode"));
					join.Property(x => x.StatusKey, map => map.Column("Status"));
					join.Property(x => x.PriorityKey, map => map.Column("Priority"));
					join.Property(x => x.CategoryKey, map => map.Column("Category"));
					join.Property(x => x.StatisticsKeyProductTypeKey);
					join.Property(x => x.StatisticsKeyMainAssemblyKey);
					join.Property(x => x.StatisticsKeySubAssemblyKey);
					join.Property(x => x.StatisticsKeyAssemblyGroupKey);
					join.Property(x => x.StatisticsKeyFaultImageKey);
					join.Property(x => x.StatisticsKeyRemedyKey);
					join.Property(x => x.StatisticsKeyCauseKey);
					join.Property(x => x.StatisticsKeyWeightingKey);
					join.Property(x => x.StatisticsKeyCauserKey);

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

					join.ManyToOne(
						x => x.CompletionServiceOrder,
						map =>
						{
							map.Column("CompletionServiceOrderId");
							map.Insert(false);
							map.Update(false);
						});
					join.ManyToOne(x => x.CompletionUserObject,
						m =>
						{
							m.Column("CompletionUser");
							m.Insert(false);
							m.Update(false);
						});
					join.ManyToOne(
						x => x.ServiceOrderTime,
						map =>
						{
							map.Column("ServiceOrderTimeId");
							map.Insert(false);
							map.Update(false);
						});
					join.ManyToOne(
						x => x.UserGroup,
						map =>
						{
							map.Column("UserGroupKey");
							map.Insert(false);
							map.Update(false);
						});
					join.ManyToOne(
						x => x.AffectedCompany,
						map =>
						{
							map.Column("AffectedCompanyKey");
							map.Insert(false);
							map.Update(false);
						});
					join.ManyToOne(
						x => x.ContactPerson,
						map =>
						{
							map.Column("ContactPersonKey");
							map.Insert(false);
							map.Update(false);
						});
					join.ManyToOne(
						x => x.AffectedInstallation,
						map =>
						{
							map.Column("AffectedInstallationKey");
							map.Insert(false);
							map.Update(false);
						});
					join.ManyToOne(
						x => x.OriginatingServiceOrder,
						map =>
						{
							map.Column("OriginatingServiceOrderId");
							map.Insert(false);
							map.Update(false);
						});
					join.ManyToOne(
						x => x.OriginatingServiceOrderTime,
						map =>
						{
							map.Column("OriginatingServiceOrderTimeId");
							map.Insert(false);
							map.Update(false);
						});
					join.ManyToOne(
						x => x.ServiceObject,
						map =>
						{
							map.Column("ServiceObjectId");
							map.Insert(false);
							map.Update(false);
						});
					Set(x => x.RequiredSkillKeys, map =>
					{
						map.Schema("SMS");
						map.Table("ServiceNotificationsSkill");
						map.Key(km => km.Column("ContactKey"));
						map.Fetch(CollectionFetchMode.Select);
						map.Lazy(CollectionLazy.Lazy);
						map.Cascade(Cascade.Persist);
					}, r => r.Element(m => m.Column("Skill")));
					Set(x => x.RequiredAssetKeys, map =>
					{
						map.Schema("SMS");
						map.Table("ServiceNotificationsAsset");
						map.Key(km => km.Column("ContactKey"));
						map.Fetch(CollectionFetchMode.Select);
						map.Lazy(CollectionLazy.Lazy);
						map.Cascade(Cascade.Persist);
					}, r => r.Element(m => m.Column("AssetKey")));
					this.EntitySet(x => x.ServiceOrderErrorTypes,
						m =>
						{
							m.Key(km => km.Column("ServiceCaseId"));
							m.Inverse(true);
						},
						a => a.OneToMany());
						});
			
		}
	}
}
