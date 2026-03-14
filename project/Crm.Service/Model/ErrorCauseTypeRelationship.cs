namespace Crm.Service.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Service.Model.Lookup;

	public class ErrorCauseTypeRelationship : EntityBase<Guid>, ISoftDelete, INoAuthorisedObject
	{
		public virtual Guid StatisticsKeyCauseKey { get; set; }
		public virtual Guid ErrorTypeKey { get; set; }
		public virtual StatisticsKeyCause StatisticsKeyCause { get; set; }
	}
}
