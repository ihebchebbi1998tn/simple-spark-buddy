namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240822155200)]
	public class AddParentVersionToServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderTimePostings", "ParentServiceOrderTimePostingVersion"))
			{
				Database.AddColumn("SMS.ServiceOrderTimePostings",
					new Column("ParentServiceOrderTimePostingVersion",
						DbType.Int64,
						ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE t SET t.ParentServiceOrderTimePostingVersion = p.Version from SMS.ServiceOrderTimePostings t join SMS.ServiceOrderTimePostings p on t.ParentServiceOrderTimePostingId = p.id");
			}
		}
	}
}
