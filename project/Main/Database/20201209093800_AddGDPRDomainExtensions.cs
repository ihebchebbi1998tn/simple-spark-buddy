namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20201209093800)]
	public class AddGDPRDomainExtensions : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("CompanyName", DbType.String, ColumnProperty.NotNull, "'L-mobile solutions GmbH & Co. KG'"));
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("ResponsibleAddress", DbType.String, ColumnProperty.NotNull, "'L-mobile solutions GmbH & Co. KG, Im Horben 7, D-71560 Sulzbach/Murr, E-Mail: support@l-mobile.com'"));
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("DataProtectionOfficer", DbType.String, ColumnProperty.NotNull, "'L-mobile solutions GmbH & Co. KG, Im Horben 7, D-71560 Sulzbach/Murr, E-Mail: support@l-mobile.com'"));
		}
	}
}
