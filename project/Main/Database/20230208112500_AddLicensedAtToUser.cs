
namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230208112500)]
	public class AddLicensedAtToUser : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[User]", new Column("LicensedAt", DbType.DateTime, ColumnProperty.Null));
		}
	}
}
