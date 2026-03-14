namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140630141800)]
	public class AddObjectNoToSmsServiceObject : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceObject", new Column("ObjectNo", DbType.String, 20, ColumnProperty.Null));
		}
	}
}