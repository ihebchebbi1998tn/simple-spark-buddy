namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230802133000)]
	public class AddPasswordChangeRequiredToUser : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[User]", new Column("PasswordChangeRequired", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
