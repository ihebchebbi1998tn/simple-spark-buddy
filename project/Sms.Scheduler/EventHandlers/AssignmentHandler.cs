namespace Sms.Scheduler.EventHandlers
{
	using System;
	using System.Linq;

	using Crm.Article.Model;
	using Crm.Article.Model.Relationships;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.PerDiem.Model;
	using Crm.Service.Model;

	using log4net;

	using Sms.Scheduler.Model;

	public class AssignmentHandler : IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>,
		IEventHandler<EntityCreatedEvent<Absence>>,
		IEventHandler<EntityCreatedEvent<UserTimeEntry>>,
		IEventHandler<EntityCreatedEvent<ArticleDowntime>>,
		IEventHandler<EntityCreatedEvent<ServiceOrderTimePosting>>,
		IEventHandler<EntityModifiedEvent<ServiceOrderDispatch>>,
		IEventHandler<EntityModifiedEvent<Absence>>,
		IEventHandler<EntityModifiedEvent<UserTimeEntry>>,
		IEventHandler<EntityModifiedEvent<ArticleDowntime>>,
		IEventHandler<EntityModifiedEvent<ServiceOrderTimePosting>>,
		IEventHandler<EntityDeletedEvent<ServiceOrderDispatch>>,
		IEventHandler<EntityDeletedEvent<Absence>>,
		IEventHandler<EntityDeletedEvent<UserTimeEntry>>,
		IEventHandler<EntityDeletedEvent<ArticleDowntime>>,
		IEventHandler<EntityDeletedEvent<ServiceOrderTimePosting>>
	{
		private readonly IRepositoryWithTypedId<DispatchPersonAssignment, Guid> personAssignmentRepository;
		private readonly IRepositoryWithTypedId<DispatchArticleAssignment, Guid> articleAssignmentRepository;
		private readonly Func<DispatchPersonAssignment> personAssignmentFactory;
		private readonly Func<DispatchArticleAssignment> articleAssignmentFactory;
		private readonly IRepositoryWithTypedId<ArticleUserRelationship, Guid> articleUserRelationshipRepository;
		private readonly ILog logger;

		// Constructor
		public AssignmentHandler(
			ILog logger,
			Func<DispatchPersonAssignment> personAssignmentFactory,
			IRepositoryWithTypedId<DispatchArticleAssignment, Guid> articleAssignmentRepository,
			IRepositoryWithTypedId<DispatchPersonAssignment, Guid> personAssignmentRepository,
			Func<DispatchArticleAssignment> articleAssignmentFactory,
			IRepositoryWithTypedId<ArticleUserRelationship, Guid> articleUserRelationshipRepository = null)
		{
			this.logger = logger;
			this.personAssignmentFactory = personAssignmentFactory;
			this.articleAssignmentRepository = articleAssignmentRepository;
			this.personAssignmentRepository = personAssignmentRepository;
			this.articleAssignmentFactory = articleAssignmentFactory;
			this.articleUserRelationshipRepository = articleUserRelationshipRepository;
		}

		protected virtual void CreateDispatchPersonAssignment(string method, Guid dispatchKey, string resourceKey)
		{
			if (personAssignmentRepository.GetAll().Any(p => p.DispatchKey == dispatchKey && p.ResourceKey == resourceKey))
				return;

			logger.Debug($"EventHandler {method} creating DispatchPersonAssignment for {dispatchKey}, {resourceKey}...");

			var assignment = personAssignmentFactory();
			assignment.DispatchKey = dispatchKey;
			assignment.ResourceKey = resourceKey;
			personAssignmentRepository.SaveOrUpdate(assignment);

			logger.Debug($"EventHandler {method} created DispatchPersonAssignment for {dispatchKey}, {resourceKey}.");
		}

		protected virtual void DeleteDispatchPersonAssignments(string method, Guid dispatchKey)
		{
			var assignments = personAssignmentRepository.GetAll().Where(p => p.DispatchKey == dispatchKey).ToArray();

			if (assignments.Length == 0)
				return;

			logger.Debug($"EventHandler {method} deleting {assignments.Length} DispatchPersonAssignment(s) of {dispatchKey}...");

			foreach (var assignment in assignments)
				personAssignmentRepository.Delete(assignment);

			logger.Debug($"EventHandler {method} deleted {assignments.Length} DispatchPersonAssignment(s) of {dispatchKey}.");
		}

		protected virtual void CreateArticlePersonAssignment(string method, Guid dispatchKey, Guid resourceKey)
		{
			if (articleAssignmentRepository.GetAll().Any(p => p.DispatchKey == dispatchKey && p.ResourceKey == resourceKey))
				return;

			logger.Debug($"EventHandler {method} creating ArticlePersonAssignment for {dispatchKey}, {resourceKey}...");

			var assignment = articleAssignmentFactory();
			assignment.DispatchKey = dispatchKey;
			assignment.ResourceKey = resourceKey;
			articleAssignmentRepository.SaveOrUpdate(assignment);

			logger.Debug($"EventHandler {method} created ArticlePersonAssignment for {dispatchKey}, {resourceKey}.");
		}

		protected virtual void DeleteArticlePersonAssignments(string method, Guid dispatchKey)
		{
			var assignments = articleAssignmentRepository.GetAll().Where(p => p.DispatchKey == dispatchKey).ToArray();

			if (assignments.Length == 0)
				return;

			logger.Debug($"EventHandler {method} deleting {assignments.Length} ArticlePersonAssignment(s) of {dispatchKey}...");

			foreach (var assignment in assignments)
				articleAssignmentRepository.Delete(assignment);

			logger.Debug($"EventHandler {method} deleted {assignments.Length} ArticlePersonAssignment(s) of {dispatchKey}.");
		}

		#region EntityCreatedEvents

		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e) =>
			CreateDispatchPersonAssignment("EntityCreatedEvent<ServiceOrderDispatch>", e.Entity.Id, e.Entity.DispatchedUsername);

		public virtual void Handle(EntityCreatedEvent<UserTimeEntry> e) =>
			CreateDispatchPersonAssignment("EntityCreatedEvent<UserTimeEntry>", e.Entity.Id, e.Entity.ResponsibleUser);

		public virtual void Handle(EntityCreatedEvent<Absence> e) =>
			CreateDispatchPersonAssignment("EntityCreatedEvent<Absence>", e.Entity.Id, e.Entity.ResponsibleUser);

		public virtual void Handle(EntityCreatedEvent<ArticleDowntime> e) =>
			CreateArticlePersonAssignment("EntityCreatedEvent<ArticleDowntime>", e.Entity.Id, e.Entity.ArticleKey);

		public virtual void Handle(EntityCreatedEvent<ServiceOrderTimePosting> e)
		{
			if (e.Entity.From == null || e.Entity.To == null || string.IsNullOrWhiteSpace(e.Entity.UserUsername))
				return;

			CreateDispatchPersonAssignment("EntityCreatedEvent<ServiceOrderTimePosting>", e.Entity.Id, e.Entity.UserUsername);
		}

		#endregion

		#region EntityModifiedEvents

		public virtual void Handle(EntityModifiedEvent<ServiceOrderDispatch> e)
		{
			if (e.EntityBeforeChange.DispatchedUsername == e.Entity.DispatchedUsername)
				return;

			CreateDispatchPersonAssignment("EntityModifiedEvent<ServiceOrderDispatch>", e.Entity.Id, e.Entity.DispatchedUsername);
		}

		public virtual void Handle(EntityModifiedEvent<UserTimeEntry> e)
		{
			if (e.EntityBeforeChange.ResponsibleUser == e.Entity.ResponsibleUser)
				return;

			CreateDispatchPersonAssignment("EntityModifiedEvent<UserTimeEntry>", e.Entity.Id, e.Entity.ResponsibleUser);
		}

		public virtual void Handle(EntityModifiedEvent<Absence> e)
		{
			if (e.EntityBeforeChange.ResponsibleUser == e.Entity.ResponsibleUser)
				return;

			CreateDispatchPersonAssignment("EntityModifiedEvent<Absence>", e.Entity.Id, e.Entity.ResponsibleUser);
		}

		public virtual void Handle(EntityModifiedEvent<ArticleDowntime> e)
		{
			if (e.EntityBeforeChange.ArticleKey == e.Entity.ArticleKey)
				return;

			CreateArticlePersonAssignment("EntityModifiedEvent<ArticleDowntime>", e.Entity.Id, e.Entity.ArticleKey);
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderTimePosting> e)
		{
			if (e.Entity.From == null || e.Entity.To == null || string.IsNullOrWhiteSpace(e.Entity.UserUsername) ||
				e.EntityBeforeChange.UserUsername == e.Entity.UserUsername)
				return;

			CreateDispatchPersonAssignment("EntityModifiedEvent<ServiceOrderTimePosting>", e.Entity.Id, e.Entity.UserUsername);
		}

		#endregion

		#region EntityDeletedEvents

		public virtual void Handle(EntityDeletedEvent<ServiceOrderDispatch> e)
		{
			var dispatchId = e.Entity.Id;
			DeleteDispatchPersonAssignments("EntityDeletedEvent<ServiceOrderDispatch>", dispatchId);
			if (articleUserRelationshipRepository != null && !string.IsNullOrEmpty(e.Entity.DispatchedUsername))
			{
				var articleUserRelationships = articleUserRelationshipRepository.GetAll()
					.Where(a => a.UserKey == e.Entity.DispatchedUsername)
					.ToList();

				foreach (var relationship in articleUserRelationships)
				{
					articleUserRelationshipRepository.Delete(relationship);
					logger.Debug($"Deleted ArticleUserRelationship for DispatchKey: {dispatchId}, ArticleKey: {relationship.ArticleKey}");
				}
			}

			logger.Debug($"Handled EntityDeletedEvent<ServiceOrderDispatch> for DispatchKey: {dispatchId}");
		}

		public virtual void Handle(EntityDeletedEvent<UserTimeEntry> e) =>
			DeleteDispatchPersonAssignments("EntityDeletedEvent<UserTimeEntry>", e.Entity.Id);

		public virtual void Handle(EntityDeletedEvent<Absence> e) =>
			DeleteDispatchPersonAssignments("EntityDeletedEvent<Absence>", e.Entity.Id);

		public virtual void Handle(EntityDeletedEvent<ArticleDowntime> e) =>
			DeleteArticlePersonAssignments("EntityDeletedEvent<ArticleDowntime>", e.Entity.Id);

		public virtual void Handle(EntityDeletedEvent<ServiceOrderTimePosting> e) =>
			DeleteDispatchPersonAssignments("EntityDeletedEvent<ServiceOrderTimePosting>", e.Entity.Id);

		#endregion
	}
}
