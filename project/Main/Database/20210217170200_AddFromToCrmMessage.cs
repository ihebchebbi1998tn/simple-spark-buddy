namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20210217170200)]
	public class AddFromToCrmMessage : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Message", new Column("[From]", DbType.String, ColumnProperty.Null));
		}
	}
}
