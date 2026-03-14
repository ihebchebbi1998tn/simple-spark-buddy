namespace Main.Services
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;

	using log4net;

	public class UserDischargeService : IUserDischargeService
	{
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<User, string> userRepository;
		private readonly IAuthenticationService authenticationService;
		private readonly ILog logger;
		private static bool receivedShutdownSignal;

		public UserDischargeService(IUserService userService, IRepositoryWithTypedId<User, string> userRepository, ILog logger, IAuthenticationService authenticationService)
		{
			this.userService = userService;
			this.userRepository = userRepository;
			this.logger = logger;
			this.authenticationService = authenticationService;
		}

		public virtual void DischargeExpiredUsers()
		{
			var usersToDischarge = userService.GetUsers().Where(x => x.Discharged == false && x.DischargeDate.IsNotNull() && x.DischargeDate <= DateTime.UtcNow).ToList();
			if (usersToDischarge.Any())
			{
				logger.InfoFormat("Trying to discharge {0} Users.", usersToDischarge.Count);
				foreach (var user in usersToDischarge)
				{
					if (receivedShutdownSignal)
					{
						break;
					}
					try
					{
						user.Discharged = true;
						userRepository.SaveOrUpdate(user);
						authenticationService.SignOutDevices(user);
						logger.InfoFormat("User: {0} is successfully discharged", user.DisplayName);
					}
					catch (Exception ex)
					{
						logger.ErrorFormat("An error occured while discharging user: {0},\n\n {1}", user.DisplayName, ex);
					}
				}
				return;
			}
			logger.Debug("There were no users to discharge");
		}
		public virtual void StopExcecution()
		{
			receivedShutdownSignal = true;
		}
	}
}
