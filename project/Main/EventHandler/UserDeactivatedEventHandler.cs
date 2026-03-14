namespace Main.EventHandler
{
	using Crm.Library.Extensions;
	using Crm.Library.Licensing;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Events;

	public class UserDeactivatedEventHandler : IEventHandler<EntityModifiedEvent<User>>
	{
		private readonly ILicensingService licensingService;

		public virtual void Handle(EntityModifiedEvent<User> e)
		{
			if (e.Entity.Discharged && !e.EntityBeforeChange.Discharged || e.Entity.LicensedAt.IsNull() && e.EntityBeforeChange.LicensedAt.IsNotNull())
			{
				licensingService.ExitUserLicenses(e.Entity);
			}
		}

		public UserDeactivatedEventHandler(ILicensingService licensingService)
		{
			this.licensingService = licensingService;
		}
	}
}
