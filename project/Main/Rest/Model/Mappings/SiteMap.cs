namespace Main.Rest.Model.Mappings
{
	using System.Collections.Generic;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.Model.Site;

	using LMobile;
	using LMobile.Unicore;

	public class SiteMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Domain, Site>()
				.BeforeMap((domain, site, context) => { site.UseDynamicExtensionRegistries(context.GetService<IEnumerable<DynamicExtensionRegistry>>()); });
			mapper.CreateMap<Site, Domain>()
				.ForMember(x => x.PersistenceExtension, opt => opt.Ignore());
		}
	}
}
