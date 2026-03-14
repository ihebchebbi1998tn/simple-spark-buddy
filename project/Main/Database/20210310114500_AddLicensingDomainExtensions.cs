namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20210310114500)]
	public class AddLicensingDomainExtensions : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("ContractNo", DbType.String, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("ProjectId", DbType.Guid, ColumnProperty.Null));
		}
	}
}
