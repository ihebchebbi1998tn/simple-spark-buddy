namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20200318131300)]
	public class AddCanBeAddedAfterCustomerSignatureToCrmArticle : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Article", new Column("CanBeAddedAfterCustomerSignature", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}