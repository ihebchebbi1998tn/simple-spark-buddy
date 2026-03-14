namespace Main.BackgroundServices
{
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Services.Interfaces;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	public class UserDischargeAgent : JobBase
	{
		private readonly IUserDischargeService userDischargeService;

		protected override void Run(IJobExecutionContext context)
		{
			userDischargeService.DischargeExpiredUsers();
		}
		public override void Stop()
		{
			userDischargeService.StopExcecution();
			base.Stop();
		}
		public UserDischargeAgent(ISessionProvider sessionProvider, ILog logger, IHostApplicationLifetime hostApplicationLifetime, IUserDischargeService userDischargeService)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.userDischargeService = userDischargeService;
		}
	}
}
