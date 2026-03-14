namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	
	[Migration(20240531094200)]
	public class AddPrematureDateToServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead",
				new Column("PrematureDate",
				DbType.DateTime,
				ColumnProperty.Null)
			);

			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead",
				new Column("PrematureTime",
				DbType.DateTime,
				ColumnProperty.Null)
			);
		}
	}
}
