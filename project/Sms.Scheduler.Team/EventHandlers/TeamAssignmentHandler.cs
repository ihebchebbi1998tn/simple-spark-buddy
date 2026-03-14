namespace Sms.Scheduler.Team.EventHandlers
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Team.Model;

	using log4net;

	using Sms.Scheduler.Model;

	public class TeamAssignmentHandler : IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>,
		IEventHandler<EntityModifiedEvent<ServiceOrderDispatch>>,
		IEventHandler<EntityDeletedEvent<ServiceOrderDispatch>>
	{
		private readonly IRepositoryWithTypedId<DispatchPersonAssignment, Guid> dispatchPersonAssignmentRepository;
		private readonly IRepositoryWithTypedId<Usergroup, Guid> usergroupRepository;
		private readonly Func<DispatchPersonAssignment> dispatchPersonAssignmentFactory;
		private readonly IRepositoryWithTypedId<ArticleUserGroupRelationship, Guid> articleUserGroupRelationshipRepository;
		private readonly ILog logger;

		// Constructor
		public TeamAssignmentHandler(
			ILog logger,
			IRepositoryWithTypedId<DispatchPersonAssignment, Guid> dispatchPersonAssignmentRepository,
			IRepositoryWithTypedId<Usergroup, Guid> usergroupRepository,
			Func<DispatchPersonAssignment> dispatchPersonAssignmentFactory,
			IRepositoryWithTypedId<ArticleUserGroupRelationship, Guid> articleUserGroupRelationshipRepository)
		{
			this.logger = logger;
			this.dispatchPersonAssignmentRepository = dispatchPersonAssignmentRepository;
			this.dispatchPersonAssignmentFactory = dispatchPersonAssignmentFactory;
			this.articleUserGroupRelationshipRepository = articleUserGroupRelationshipRepository;
			this.usergroupRepository = usergroupRepository;
		}

		protected virtual void CreateDispatchPersonAssignment(string method, Guid dispatchKey, string resourceKey)
		{
			if (dispatchPersonAssignmentRepository.GetAll().Any(p => p.DispatchKey == dispatchKey && p.ResourceKey == resourceKey))
			{
				return;
			}

			logger.Debug($"EventHandler {method} creating DispatchArticleAssignment for {dispatchKey}, {resourceKey}...");

			var assignment = dispatchPersonAssignmentFactory();
			assignment.DispatchKey = dispatchKey;
			assignment.ResourceKey = resourceKey;
			dispatchPersonAssignmentRepository.SaveOrUpdate(assignment);

			logger.Debug($"EventHandler {method} created DispatchArticleAssignment for {dispatchKey}, {resourceKey}.");
		}

		protected virtual void DeleteDispatchPersonAssignments(string method, Guid dispatchKey)
		{
			var assignments = dispatchPersonAssignmentRepository.GetAll().Where(p => p.DispatchKey == dispatchKey).ToArray();

			if (assignments.Length == 0)
			{
				return;
			}

			logger.Debug($"EventHandler {method} deleting {assignments.Length} DispatchArticleAssignment(s) of {dispatchKey}...");

			foreach (var assignment in assignments)
			{
				dispatchPersonAssignmentRepository.Delete(assignment);
			}

			logger.Debug($"EventHandler {method} deleted {assignments.Length} DispatchArticleAssignment(s) of {dispatchKey}.");
		}

		#region EntityCreatedEvents

		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e)
		{
			var dispatchExtension = e.Entity.GetExtension<Crm.Service.Team.Model.ServiceOrderDispatchExtension>();
			if (dispatchExtension.TeamId.HasValue)
			{
				var team = usergroupRepository.Get(dispatchExtension.TeamId.Value);
				foreach (var member in team.Members)
				{
					CreateDispatchPersonAssignment("EntityCreatedEvent<ServiceOrderDispatch>", e.Entity.Id, member.Username);
				}
			}
		}

		#endregion

		#region EntityModifiedEvents

		public virtual void Handle(EntityModifiedEvent<ServiceOrderDispatch> e)
		{
			var dispatchExtension = e.Entity.GetExtension<Crm.Service.Team.Model.ServiceOrderDispatchExtension>();
			var dispatchBeforeChangeExtension = e.EntityBeforeChange.GetExtension<Crm.Service.Team.Model.ServiceOrderDispatchExtension>();
			if (!dispatchExtension.TeamId.HasValue ||
				(dispatchBeforeChangeExtension.TeamId.HasValue && dispatchExtension.TeamId.Value == dispatchBeforeChangeExtension.TeamId.Value))
			{
				return;
			}
			if (dispatchBeforeChangeExtension.TeamId.HasValue)
			{
				DeleteDispatchPersonAssignments("EntityDeletedEvent<ServiceOrderDispatch>", e.Entity.Id);
			}

			var team = usergroupRepository.Get(dispatchExtension.TeamId.Value);
			foreach (var member in team.Members)
			{
				CreateDispatchPersonAssignment("EntityCreatedEvent<ServiceOrderDispatch>", e.Entity.Id, member.Username);
			}
		}

		#endregion

		#region EntityDeletedEvents

		public virtual void Handle(EntityDeletedEvent<ServiceOrderDispatch> e)
		{
			var dispatchId = e.Entity.Id;
			DeleteDispatchPersonAssignments("EntityDeletedEvent<ServiceOrderDispatch>", dispatchId);
			var extension = e.Entity.GetExtension<Crm.Service.Team.Model.ServiceOrderDispatchExtension>();
			if (articleUserGroupRelationshipRepository != null && extension != null && extension.TeamId.HasValue)
			{
				var articleUserGroupRelationships = articleUserGroupRelationshipRepository.GetAll()
					.Where(a => a.UserGroupKey == extension.TeamId.Value)
					.ToList();

				foreach (var relationship in articleUserGroupRelationships)
				{
					articleUserGroupRelationshipRepository.Delete(relationship);
					logger.Debug($"Deleted ArticleUserGroupRelationship for DispatchKey: {dispatchId}, ArticleKey: {relationship.ArticleKey}");
				}
			}
		}

		#endregion
	}
}
