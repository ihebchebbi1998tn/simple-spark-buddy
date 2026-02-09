namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240822155100)]
	public class AddVersionToServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderTimePostings", "Version"))
			{
				Database.AddColumn("SMS.ServiceOrderTimePostings",
					new Column("Version",
						DbType.Int64,
						ColumnProperty.NotNull,
						1));
			}
		}
	}
}
