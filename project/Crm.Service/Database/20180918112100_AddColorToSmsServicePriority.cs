namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180918112100)]
	public class AddColorToSmsServicePriority : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServicePriority", new Column("Color", DbType.String, 20, ColumnProperty.NotNull, "'#9E9E9E'"));
		}
	}
}
