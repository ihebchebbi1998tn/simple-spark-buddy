namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20221110113400)]
	public class AddIsDefaultForServiceOrderTimesToCrmArticle : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Article", new Column("IsDefaultForServiceOrderTimes", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
