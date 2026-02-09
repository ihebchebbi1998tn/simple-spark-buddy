namespace Main.Model.Mappings
{
	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class NumberingSequenceMap : ClassMapping<NumberingSequence>
	{
		public NumberingSequenceMap()
		{
			Schema("dbo");
			Table("NumberingSequence");
			Mutable(false);

			Id(x => x.SequenceName, map => map.Generator(Generators.Assigned));

			Property(x => x.SequenceName);
			Property(x => x.MaxLow);
			Property(x => x.LastNumber);
			Property(x => x.Prefix);
			Property(x => x.Format);
			Property(x => x.Suffix);
		}
	}
}
