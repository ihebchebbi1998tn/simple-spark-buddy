namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20191014150900)]
	public class AddIsTemplateToServiceOrder : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("IsTemplate", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
