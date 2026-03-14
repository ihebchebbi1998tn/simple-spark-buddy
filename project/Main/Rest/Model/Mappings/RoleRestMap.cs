namespace Main.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;

	using LMobile.Unicore;
	using LMobile.Unicore.NHibernate;

	public class RoleRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<PermissionSchemaRole, PermissionSchemaRoleRest>()
				.ForMember(x => x.SourcePermissionSchemaRole, m => m.MapFrom(x => x.PersistenceExtension<PermissionSchemaRolePersistenceExtension, PermissionSchemaRole>(p => p.SourcePermissionSchemaRole)))
				;
			mapper.CreateMap<PermissionSchemaRoleRest, PermissionSchemaRole>()
				;
		}
	}
}
