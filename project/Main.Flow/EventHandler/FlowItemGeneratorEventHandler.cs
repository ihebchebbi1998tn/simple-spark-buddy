using Crm.Library.Api.Mapping;
using Crm.Library.BaseModel.Interfaces;
using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Library.Extensions;
using Crm.Library.Modularization.Events;
using Crm.Library.Rest;

using Main.Flow.Model;
using Quartz;
using System;
using System.Linq;

namespace Main.Flow.EventHandler
{
	public class FlowItemGeneratorEventHandler : IEventHandler<EntityCreatedEvent<IEntity>>, IEventHandler<EntityModifiedEvent<IEntity>>, IEventHandler<EntityDeletedEvent<IEntity>>
	{
		private readonly IScheduler scheduler;
		private readonly IRepositoryWithTypedId<FlowItem, Guid> flowItemRepository;
		private readonly IRepositoryWithTypedId<FlowRule, Guid> flowRuleRepository;
		private readonly Func<FlowItem> flowItemFactory;
		private readonly IODataMapper mapper;
		private readonly RestTypeProviderCache restTypeProviderCache;

		public virtual void Handle(EntityCreatedEvent<IEntity> e)
		{
			var entityTypeName = GetEntityTypeName(e.Entity);
			var rules = flowRuleRepository.GetAll().Where(x => x.EntityType == entityTypeName && x.Action == Actions.Created);

			if (!rules.Any())
				return;

			rules.ToList().ForEach(rule => GenerateFlowItem(e.Entity, rule));
		}
		public virtual void Handle(EntityModifiedEvent<IEntity> e)
		{
			var entityTypeName = GetEntityTypeName(e.Entity);
			var rules = flowRuleRepository.GetAll().Where(x => x.EntityType == entityTypeName && x.Action == Actions.Modified);

			if (!rules.Any())
				return;

			rules.ToList().ForEach(rule => GenerateFlowItem(e.Entity, rule));
		}
		public virtual void Handle(EntityDeletedEvent<IEntity> e)
		{
			var entityTypeName = GetEntityTypeName(e.Entity);
			var rules = flowRuleRepository.GetAll().Where(x => x.EntityType == entityTypeName && x.Action == Actions.Deleted);

			if (!rules.Any())
				return;

			rules.ToList().ForEach(rule => GenerateFlowItem(e.Entity, rule));
		}

		protected virtual string GetEntityTypeName(IEntity entity)
		{
			return entity.ActualType.FullName;
		}

		protected virtual void GenerateFlowItem(IEntity entity, FlowRule rule)
		{
			var flowItem = flowItemFactory();

			flowItem.EntityId = entity.Id.ToString();
			flowItem.EntityTypeName = rule.EntityType;
			flowItem.RuleKey = rule.Id;
			flowItem.SerializedEntity = mapper.Map(entity, entity.GetType(), restTypeProviderCache.GetRestTypeFor(entity.GetType())).SerializeToJson();
			flowItem.PostingType = Crm.Library.Model.PostingType.Save;

			flowItemRepository.SaveOrUpdate(flowItem);
		}

		public FlowItemGeneratorEventHandler(IScheduler scheduler, IRepositoryWithTypedId<FlowItem, Guid> flowItemRepository, IRepositoryWithTypedId<FlowRule, Guid> flowRuleRepository, Func<FlowItem> flowItemFactory, IODataMapper mapper, RestTypeProviderCache restTypeProviderCache)
		{
			this.scheduler = scheduler;
			this.flowItemRepository = flowItemRepository;
			this.flowRuleRepository = flowRuleRepository;
			this.flowItemFactory = flowItemFactory;
			this.mapper = mapper;
			this.restTypeProviderCache = restTypeProviderCache;
		}
	}
}
