namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20210217161400)]
	public class AddIsBodyHtmlToCrmMessage : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Message", new Column("IsBodyHtml", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
