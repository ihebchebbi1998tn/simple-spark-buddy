namespace Sms.Scheduler.Model.Mappings
{
	using Crm.Library.Model;
	using LMobile.Unicore.NHibernate;
	using NHibernate.Mapping.ByCode.Conformist;

	public class UserExtensionMap : ComponentMapping<UserExtension>, INHibernateMappingExtension<User, UserExtension>
	{
		public UserExtensionMap()
		{
			Property(x => x.ActiveProfileId);
		}
	}
}
