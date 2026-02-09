namespace Crm.Service.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ErrorCauseTypeRelationshipSyncService : DefaultSyncService<ErrorCauseTypeRelationship, Guid>
	{
		public ErrorCauseTypeRelationshipSyncService(IRepositoryWithTypedId<ErrorCauseTypeRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
	: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(StatisticsKeyCause) }; }
		}
		public override ErrorCauseTypeRelationship Save(ErrorCauseTypeRelationship entity)
		{
			repository.SaveOrUpdate(entity);
			return entity;
		}
		public override IQueryable<ErrorCauseTypeRelationship> GetAll(User user)
		{
			return repository.GetAll();
		}
	}
}
