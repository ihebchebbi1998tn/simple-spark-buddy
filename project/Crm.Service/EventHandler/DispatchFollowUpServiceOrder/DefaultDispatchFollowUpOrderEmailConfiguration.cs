namespace Crm.Service.EventHandler.DispatchFollowUpServiceOrder
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	public class DefaultDispatchFollowUpOrderEmailConfiguration : IDispatchFollowUpOrderEmailConfiguration
	{
		protected readonly IResourceManager resourceManager;
		protected readonly IAppSettingsProvider appSettingsProvider;
		protected readonly IUserService userService;

		public DefaultDispatchFollowUpOrderEmailConfiguration(IResourceManager resourceManager, IAppSettingsProvider appSettingsProvider, IUserService userService)
		{
			this.resourceManager = resourceManager;
			this.appSettingsProvider = appSettingsProvider;
			this.userService = userService;
		}

		public virtual string GetSubject(ServiceOrderDispatch dispatch)
		{
			return resourceManager.GetTranslation("DispatchFollowUpOrderEmailSubject")
				.WithArgs(dispatch.OrderHead.OrderNo, dispatch.DispatchedUser.DisplayName, dispatch.Date.ToShortDateString());
		}
		public virtual string GetEmailText(ServiceOrderDispatch dispatch)
		{
			var sb = new StringBuilder();
			sb.AppendFormatLine(resourceManager.GetTranslation("DispatchFollowUpOrderEmailText"), dispatch.DispatchedUser.DisplayName, dispatch.Date.ToShortDateString(), dispatch.OrderHead.OrderNo);
			sb.AppendLine();
			sb.AppendLine(dispatch.FollowUpServiceOrderRemark);
			return sb.ToString();
		}

		public virtual string[] GetRecipients(ServiceOrderDispatch dispatch)
		{
			var recipients = new List<string>();
				
			var dispatchReportRecipients = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.DispatchReportRecipients);
			recipients = recipients.Concat(dispatchReportRecipients).ToList();

			var sendDispatchReportToDispatcher = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchReportToDispatcher);
			if (sendDispatchReportToDispatcher)
			{
				var recipient = userService.GetUser(dispatch.CreateUser);
				recipients.Add(recipient?.Email);
			}
			
			return recipients.Where(x => x != null && x.IsValidEmailAddress()).Distinct().ToArray();
		}
	}
}