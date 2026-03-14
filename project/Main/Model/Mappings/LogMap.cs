namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class LogMap : EntityClassMapping<Log>
	{
		public LogMap()
		{
			Schema("CRM");
			// Use db independent `instead of [], Nhibernate will convert it to the correct escaping
			// http://stackoverflow.com/questions/679279/nhibernate-force-escaping-on-table-names
			Table("`Log`");

			Id(x => x.Id, map =>
				{
					map.Column("Id");
					map.Generator(Generators.Identity);
				});

			Property(x => x.CreateDate,
				map =>
				{
					map.Column("Date");
					map.Update(false);
				});
			Property(x => x.Exception, m => m.Length(Int16.MaxValue));
			Property(x => x.Level);
			Property(x => x.Logger);
			Property(x => x.Thread);
			Property(x => x.Context);
			Property(x => x.Message, m => m.Length(Int16.MaxValue));
		}
	}
}
