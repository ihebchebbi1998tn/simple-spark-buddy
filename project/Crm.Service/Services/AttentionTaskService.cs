namespace Crm.Service.Services
{
	using System;

	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Model.Lookups;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;
	using Crm.Services.Interfaces;

	using log4net;

	using Main;

	public class AttentionTaskService : IAttentionTaskService
	{
		private readonly Func<Task> taskFactory;
		private readonly ITaskService taskService;
		private readonly IUserService userService;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly ILookupManager lookupManager;
		private readonly ILog logger;

		public AttentionTaskService(Func<Task> taskFactory, ITaskService taskService, IUserService userService, IAppSettingsProvider appSettingsProvider, ILookupManager lookupManager, ILog logger)
		{
			this.taskFactory = taskFactory;
			this.taskService = taskService;
			this.userService = userService;
			this.appSettingsProvider = appSettingsProvider;
			this.lookupManager = lookupManager;
			this.logger = logger;
		}

		public virtual void CreateAttentionTaskForServiceOrder(ServiceOrderHead serviceOrder, User user, string text)
		{
			var attentionTaskTypeKey = appSettingsProvider.GetValue(MainPlugin.Settings.Task.AttentionTaskTypeKey);
			if (lookupManager.Instance.Get<TaskType>(attentionTaskTypeKey) == null)
			{
				logger.ErrorFormat("Could not create attention task because no attention task type exists for the provided key");
				return;
			}
			var attentionTask = taskFactory();
			attentionTask.ContactId = serviceOrder.Id;
			attentionTask.TypeKey = attentionTaskTypeKey;
			attentionTask.Text = text;
			attentionTask.ResponsibleGroupKey = serviceOrder.UserGroupKey;
			attentionTask.TaskCreateUser = userService.CurrentUser.DisplayName;
			attentionTask.DueDate = DateTime.Today;
			taskService.Save(attentionTask, user);
		}

		public virtual void CreateAttentionTaskForDispatch(ServiceOrderDispatch dispatch, User user, string text)
		{
			var attentionTaskTypeKey = appSettingsProvider.GetValue(MainPlugin.Settings.Task.AttentionTaskTypeKey);
			if (lookupManager.Instance.Get<TaskType>(attentionTaskTypeKey) == null)
			{
				logger.ErrorFormat("Could not create attention task because no attention task type exists for the provided key");
				return;
			}
			var attentionTask = taskFactory();
			attentionTask.ContactId = dispatch.OrderId;
			attentionTask.TypeKey = attentionTaskTypeKey;
			attentionTask.Text = text;
			attentionTask.ResponsibleUser = dispatch.DispatchedUsername;
			attentionTask.TaskCreateUser = userService.CurrentUser.DisplayName;
			attentionTask.DueDate = DateTime.Today;
			taskService.Save(attentionTask, user);
		}

	}
}
