namespace Crm.Service.BackgroundServices
{
	using Crm.Library.BackgroundServices;
	using Quartz;
	using System;
	using System.Linq;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Service.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using System.Globalization;
	using System.Security.Principal;
	using System.Text;
	using System.Threading;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Services.Interfaces;
	using log4net;

	using Microsoft.Extensions.Hosting;

	//[DisallowConcurrentExecution]
	public class ReplenishmentOrderReportSenderAgent : ManualSessionHandlingJobBase, IDocumentGeneratorService
	{
		private readonly IReplenishmentOrderService replenishmentOrderService;
		private readonly IRepositoryWithTypedId<ReplenishmentOrder, Guid> replenishmentOrderRepository;
		private readonly ILog logger;
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		private readonly IUserService userService;
		public ReplenishmentOrderReportSenderAgent(
			ISessionProvider sessionProvider,
			IReplenishmentOrderService replenishmentOrderService,
			IRepositoryWithTypedId<ReplenishmentOrder, Guid> replenishmentOrderRepository,
			ILog logger,
			IHostApplicationLifetime hostApplicationLifetime,
			IClientSideGlobalizationService clientSideGlobalizationService,
			IUserService userService)
			: base(
				sessionProvider,
				logger,
				hostApplicationLifetime)
		{
			this.replenishmentOrderService = replenishmentOrderService;
			this.replenishmentOrderRepository = replenishmentOrderRepository;
			this.logger = logger;
			this.clientSideGlobalizationService = clientSideGlobalizationService;
			this.userService = userService;
		}

		protected override void Run(IJobExecutionContext context)
		{
			var replenishmentOrderIds = GetPendingDocuments().Cast<ReplenishmentOrder>().ToList().Select(x => x.Id);
			foreach(var replenishmentOrderId in replenishmentOrderIds)
			{
				try
				{
					BeginRequest();
					var replenishmentOrder = replenishmentOrderRepository.Get(replenishmentOrderId);
					var user = userService.GetUser(replenishmentOrder.ResponsibleUser);
					if (user != null)
					{
						Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user.GetIdentityString()), new string[0]);
						Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault());
						Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentCultureNameOrDefault());
					}
					else
					{
						logger.Warn($"could not set user '{replenishmentOrder.ResponsibleUser}' for localization");
					}
					if (replenishmentOrderService.SendOrderAsPdf(replenishmentOrder))
					{
						replenishmentOrder.IsSent = true;
					}
					else
					{
						var recipientEmails = replenishmentOrderService.GetDefaultReplenishmentOrderRecipientEmails(replenishmentOrder);
						var msg = new StringBuilder($"Could not send report {replenishmentOrder.Id}. ");
						if (!recipientEmails.Any())
						{
							msg.Append("No recipients found. Please contact your system administrator.");
						}
						else
						{
							msg.Append($"{recipientEmails.Length} recipients are present, reason probably wrong status.");
						}

						logger.Warn(msg.ToString());
						replenishmentOrder.SendingError = msg.ToString();
					}
					replenishmentOrderRepository.SaveOrUpdate(replenishmentOrder);
				}
				catch (Exception exception)
				{
					EndRequest();
					BeginRequest();
					var replenishmentOrder = replenishmentOrderRepository.Get(replenishmentOrderId);
					Logger.Error($"failed generating replenishment order {replenishmentOrder.Id}", exception);
					replenishmentOrder.SendingError = exception.ToString();
					replenishmentOrderRepository.SaveOrUpdate(replenishmentOrder);
				}
				finally
				{
					EndRequest();
				}
			}
		}
		public virtual IQueryable GetPendingDocuments()
		{
			return replenishmentOrderRepository.GetAll().Where(x => x.IsClosed && !x.IsSent && x.SendingError == null);
		}
		public virtual IQueryable GetFailedDocuments()
		{
			return replenishmentOrderRepository.GetAll().Where(x => x.IsClosed && !x.IsSent && x.SendingError != null);
		}
		public virtual void Retry(Guid id)
		{
			var replenishmentOrder = replenishmentOrderRepository.Get(id);
			replenishmentOrder.SendingError = null;
			replenishmentOrderRepository.SaveOrUpdate(replenishmentOrder);
		}
	}
}
