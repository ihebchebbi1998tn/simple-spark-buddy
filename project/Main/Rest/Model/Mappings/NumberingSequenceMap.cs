namespace Main.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;

	using Main.Model;
	using Main.Rest.Controllers;

	public class NumberingSequenceMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<NumberingSequence, NumberingSequenceController.NumberingSequenceInfo>();
		}
	}
}
