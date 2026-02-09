namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;

	using Crm.Article.Services.Interfaces;
	using Crm.Library.AutoFac;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Services.Interfaces;

	using log4net;

	using Main.Model;

	public class ServiceOrderDispatchService : IServiceOrderDispatchService
	{
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository;
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly Func<Message> messageFactory;
		private readonly IUserService userService;
		private readonly ILog logger;
		private readonly IDispatchCancelNotificationEmailConfiguration dispatchCancelNotificationEmailConfigurationEmail;

		public ServiceOrderDispatchService(IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository, IRepositoryWithTypedId<Message, Guid> messageRepository, Func<Message> messageFactory, IUserService userService, ILog logger, IDispatchCancelNotificationEmailConfiguration dispatchCancelNotificationEmailConfigurationEmail)
		{
			this.serviceOrderDispatchRepository = serviceOrderDispatchRepository;
			this.messageRepository = messageRepository;
			this.messageFactory = messageFactory;
			this.userService = userService;
			this.logger = logger;
			this.dispatchCancelNotificationEmailConfigurationEmail = dispatchCancelNotificationEmailConfigurationEmail;
		}

		public virtual IEnumerable<string> GetUsedComponents()
		{
			return serviceOrderDispatchRepository.GetAll().Select(c => c.ComponentKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceOrderDispatchRejectReasons()
		{
			return serviceOrderDispatchRepository.GetAll().Select(c => c.RejectReasonKey).Distinct();
		}

		public virtual void SendCancelNotificationEmail(ServiceOrderDispatch dispatch)
		{
			var dispatcher = userService.GetUser(dispatch.CreateUser);
			var message = messageFactory();
			message.Subject = dispatchCancelNotificationEmailConfigurationEmail.GetSubject(dispatch);
			message.Recipients = new List<string>() { dispatcher.Email };
			message.Body = dispatchCancelNotificationEmailConfigurationEmail.GetBody(dispatch);
			message.IsBodyHtml = true;
			messageRepository.SaveOrUpdate(message);
		}
	}

	public interface IDispatchCancelNotificationEmailConfiguration : IDependency
	{
		string GetBody(ServiceOrderDispatch dispatch);
		string GetSubject(ServiceOrderDispatch dispatch);
	}

	public class DispatchCancelNotificationEmailConfiguration : IDispatchCancelNotificationEmailConfiguration
	{
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		private readonly IResourceManager resourceManager;
		private readonly IUserService userService;
		private readonly IArticleService articleService;
		private readonly ILookupManager lookupManager;
		public DispatchCancelNotificationEmailConfiguration(IClientSideGlobalizationService clientSideGlobalizationService, IResourceManager resourceManager, IUserService userService, IArticleService articleService, ILookupManager lookupManager)
		{
			this.clientSideGlobalizationService = clientSideGlobalizationService;
			this.resourceManager = resourceManager;
			this.userService = userService;
			this.articleService = articleService;
			this.lookupManager = lookupManager;
		}

		public virtual string GetBody(ServiceOrderDispatch dispatch)
		{
			var sb = new StringBuilder();

			var itemNos = dispatch.ServiceOrderMaterial.Select(x => x.ItemNo)
				.Concat(dispatch.TimePostings.Select(x => x.ItemNo))
				.ToList();
			var articleDescriptions = articleService.GetDescriptionMap(itemNos);

			string reason = "";
			if (dispatch.CancellationReason.IsNotNull())
			{
				reason = $"{GetTranslation("Reason")}: {lookupManager.Get<ServiceOrderDispatchCancellationReason>(dispatch.CancellationReason.Key).Value}.";
			}

			string remark = "";
			if (dispatch.CancellationRemark.IsNotNull())
			{
				remark = $"<br>{GetTranslation("Remark")}: {dispatch.CancellationRemark}";
			}

			string requiredOperations = "";
			if (dispatch.RequiredOperations.IsNotNull())
			{
				requiredOperations = $"<br>{GetTranslation("FollowUpDispatchRequired")}: {dispatch.RequiredOperations}";
			}

			string materialTable = "";
			if (dispatch.ServiceOrderMaterial.Count > 0)
			{
				var materialTableBuilder = new StringBuilder();
				materialTableBuilder.AppendLine(@$"
					<table style=""border-collapse: collapse; width: 100%; max-width: 600px;"">
						 <thead>
							<tr>
							  <th style=""padding: 10px 15px;
							              text-align: left;
							              vertical-align: middle;
                                          border-bottom: 1px solid black;"">
									{GetTranslation("Position")}
								</th>
							  <th style=""padding: 10px 15px;
							              text-align: left;
							              vertical-align: middle;
                                          border-bottom: 1px solid black;"">
									{GetTranslation("Article")}
								</th>
							  <th style=""padding: 10px 15px;
							              text-align: left;
							              vertical-align: middle;
                                          border-bottom: 1px solid black;"">
									{GetTranslation("Description")}
								</th>
							  <th style=""padding: 10px 15px;
							              text-align: left;
							              vertical-align: middle;
                                          border-bottom: 1px solid black;"">
									{GetTranslation("Quantity")}
								</th>
							</tr>
						  </thead>
						<tbody>
				");
				foreach (var material in dispatch.ServiceOrderMaterial)
				{
					materialTableBuilder.AppendLine(@$"
						<tr>
							<td style=""padding: 10px 15px;"">
							{material.PosNo}
							</td>
							<td style=""padding: 10px 15px;"">
							{material.ItemNo}
							</td>
							<td style=""padding: 10px 15px;"">
							{articleDescriptions[material.ItemNo]}
							</td>
							<td style=""padding: 10px 15px;"">
							{material.Quantity} {material.QuantityUnit}
							</td>
						<tr>
					");
				}

				materialTableBuilder.AppendLine(@"
					</tbody>
					</table>
					<br>
				");

				materialTable = materialTableBuilder.ToString();
			}

			string timeTable = "";
			if (dispatch.TimePostings.Count > 0)
			{
				var timeTableBuilder = new StringBuilder();
				timeTableBuilder.AppendLine(@$"
				<table style=""border-collapse: collapse; width: 100%; max-width: 600px;"">
					 <thead>
						<tr>
						  <th style=""padding: 10px 15px;
							              text-align: left;
							              vertical-align: middle;
                                          border-bottom: 1px solid black;"">
								{GetTranslation("Date")}
							</th>
						  <th style=""padding: 10px 15px;
							              text-align: left;
							              vertical-align: middle;
                                          border-bottom: 1px solid black;"">
								{GetTranslation("Article")}
							</th>
						  <th style=""padding: 10px 15px;
							              text-align: left;
							              vertical-align: middle;
                                          border-bottom: 1px solid black;"">
							{GetTranslation("Activity")}
							</th>
						  <th style=""padding: 10px 15px;
							              text-align: left;
							              vertical-align: middle;
                                          border-bottom: 1px solid black;"">
								{GetTranslation("Duration")}
							</th>
						</tr>
					  </thead>
					 <tbody>
			");
				foreach (var timePosting in dispatch.TimePostings)
				{
					var cultureInfo = new CultureInfo(clientSideGlobalizationService.GetCurrentCultureNameOrDefault());
					var fromAsShortDate = timePosting.From!.Value.ToLocalTime().ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo);
					var fromAsShortTime = timePosting.From!.Value.ToLocalTime().ToString(cultureInfo.DateTimeFormat.ShortTimePattern, cultureInfo);
					var toAsShortTime = timePosting.To!.Value.ToLocalTime().ToString(cultureInfo.DateTimeFormat.ShortTimePattern, cultureInfo);
					var durationInHours = TimeSpan.FromMinutes((double)timePosting.DurationInMinutes!).Hours;

					timeTableBuilder.AppendLine(@$"
					<tr>
						<td style=""padding: 10px 15px;"">
							<div>
								<b>
									{fromAsShortDate}
								</b>
							</div>
							<div>
								{userService.GetDisplayName(timePosting.UserUsername)}
							</div>
							<div>
							{fromAsShortTime}<span> - </span> {toAsShortTime}
							</div>
						</td>
						<td style=""padding: 10px 15px;"">
							{timePosting.ItemNo}
						</td>
						<td style=""padding: 10px 15px;"">
							{articleDescriptions[timePosting.ItemNo]}
						</td>
						<td style=""padding: 10px 15px;"">
							{durationInHours} {GetTranslation("HourAbbreviation")}
						</td>
					<tr>
			");
				}

				timeTableBuilder.AppendLine(@"
					</tbody>
					</table>
					<br>
				");

				timeTable = timeTableBuilder.ToString();
			}

			sb.AppendLine(@"
				<p>
					{0},
				</p>
				<p>
					{1} {2}
					{3}
					{4}
				</p>
				<p>
					{5}
				</p>
				{6}
				<p>
					{7}
				</p>
				{8}
				{9}".Replace("{0}", $"{GetTranslation("Dear")} {dispatch.CreateUser}"))
				.Replace("{1}", GetTranslation("DispatchCancellationEmailIntroduction"))
				.Replace("{2}", reason)
				.Replace("{3}", requiredOperations)
				.Replace("{4}", remark)
				.Replace("{5}", GetTranslation(dispatch.ServiceOrderMaterial.Count > 0 ? "FollowingMaterialsHaveBeenUsed" : "NoMaterialsHaveBeenUsed"))
				.Replace("{6}", materialTable)
				.Replace("{7}", GetTranslation(dispatch.TimePostings.Count > 0 ? "FollowingTimesHaveBeenPosted" : "NoTimesHaveBeenPosted"))
				.Replace("{8}", timeTable)
				.Replace("{9}", GetTranslation("AutoGeneratedMail"));

			return sb.ToString();
		}

		public virtual string GetSubject(ServiceOrderDispatch dispatch)
		{
			return $"Cancelled dispatch: {GetTranslation("ServiceOrder")} {dispatch.OrderHead.OrderNo} - {GetTranslation("ServiceOrderDispatch")}"
			       + $" {GetTranslation("by")} {dispatch.DispatchedUser.FirstName} {dispatch.DispatchedUser.LastName}"
			       + $" {GetTranslation("OnDate")} {dispatch.Date.ToString("d", CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentCultureNameOrDefault()))}";
		}

		public virtual string GetTranslation(string resourceKey)
		{
			var language = clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault();
			return resourceManager.GetTranslation(resourceKey, language);
		}
	}
}
