namespace Main.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;

	public class DocumentGeneratorEntryMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<object, DocumentGeneratorEntry>()
				.ForMember(x => x.GeneratorService, m => m.MapFrom((source, dest, member, context) => context.Items.ContainsKey("DocumentGenerator") ? context.Items["DocumentGenerator"] : null))
				.ForMember(x => x.Type, opt => opt.MapFrom(src => src.GetType().Name));
		}
	}
}
