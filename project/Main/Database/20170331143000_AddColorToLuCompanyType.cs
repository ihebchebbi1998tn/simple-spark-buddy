namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20170331143000)]
	public class AddColorToLuCompanyType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("LU.CompanyType", new Column("Color", DbType.String, 20, ColumnProperty.NotNull, "'#9E9E9E'"));
		}
	}
}