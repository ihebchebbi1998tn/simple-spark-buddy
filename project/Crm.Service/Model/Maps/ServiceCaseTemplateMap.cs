namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	using GuidCombGeneratorDef = LMobile.Unicore.NHibernate.GuidCombGeneratorDef;

	public class ServiceCaseTemplateMap : EntityClassMapping<ServiceCaseTemplate>
	{
		public ServiceCaseTemplateMap()
		{
			Schema("SMS");
			Table("ServiceCaseTemplate");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("ServiceCaseTemplateId");
					map.Generator(GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.CategoryKey);
			Property(x => x.Name);
			Property(x => x.PriorityKey);
			Property(x => x.UserGroupKey);
			Property(x => x.ResponsibleUser);
			ManyToOne(
				x => x.UserGroup,
				map =>
				{
					map.Column("UserGroupKey");
					map.Insert(false);
					map.Update(false);
				});
			ManyToOne(
				x => x.ResponsibleUserObject,
				m =>
				{
					m.Column("ResponsibleUser");
					m.Insert(false);
					m.Update(false);
				});
			Set(x => x.RequiredSkillKeys, map =>
			{
				map.Schema("SMS");
				map.Table("ServiceCaseTemplateSkill");
				map.Key(km => km.Column("ServiceCaseTemplateId"));
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Persist);
			}, r => r.Element(m => m.Column("Skill")));
			Set(x => x.RequiredAssetKeys, map =>
			{
				map.Schema("SMS");
				map.Table("ServiceCaseTemplateAsset");
				map.Key(km => km.Column("ServiceCaseTemplateId"));
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Persist);
			}, r => r.Element(m => m.Column("AssetKey")));
		}
	}
}
