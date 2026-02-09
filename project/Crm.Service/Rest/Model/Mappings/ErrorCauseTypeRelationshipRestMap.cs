namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Service.Model;

	public class ErrorCauseTypeRelationshipRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ErrorCauseTypeRelationship, ErrorCauseTypeRelationshipRest>();
		}
	}
}
