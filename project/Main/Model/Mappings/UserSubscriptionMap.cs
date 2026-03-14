namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	public class UserSubscriptionMap : EntityClassMapping<UserSubscription>
	{
		public UserSubscriptionMap()
		{
			Schema("CRM");
			Table("UserSubscription");

			Id(a => a.Id, m =>
			{
				m.Column("UserSubscriptionId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.Username);
			Property(x => x.EntityType);
			Property(x => x.EntityKey);
			Property(x => x.IsSubscribed);
		}
	}
}
