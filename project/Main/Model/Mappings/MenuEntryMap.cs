namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using LMobile.Unicore.NHibernate;

	public class MenuEntryMap : EntityClassMapping<MenuEntry>
	{
		public MenuEntryMap()
		{
			Schema("Crm");
			Table("MenuEntry");

			Id(x => x.Id,
				m =>
				{
					m.Column("MenuEntryId");
					m.Generator(GuidCombGeneratorDef.Instance);
					m.UnsavedValue(Guid.Empty);
				});

			Property(x => x.Category);
			Property(x => x.Title);
			Property(x => x.Priority);
			Property(x => x.IconClass);
		}
	}
}
