
namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230516153800)]
	public class AddIsActiveToCrmMessage : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Message]", new Column("IsActive", DbType.Boolean, true));
		}
	}
}
