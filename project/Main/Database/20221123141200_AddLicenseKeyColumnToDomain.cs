namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20221123141200)]
	public class AddLicenseKeyColumnToDomain : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("LicenseKey", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}
