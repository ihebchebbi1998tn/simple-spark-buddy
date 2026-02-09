namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120923113326)]
	public class DropSmsOldArticlesForeignKeys : Migration
	{
		public override void Up()
		{
			if (!Database.ConstraintExists("[SMS].[Old_Articles]", "FK_ServiceOrderMaterial_Articles"))
			{
				Database.RemoveConstraint("[SMS].[Old_Articles]", "FK_ServiceOrderMaterial_Articles");
			}
			if (!Database.ConstraintExists("[SMS].[Old_Articles]", "FK_ServiceOrderTimes_Articles"))
			{
				Database.RemoveConstraint("[SMS].[Old_Articles]", "FK_ServiceOrderTimes_Articles");
			}
		}
		public override void Down()
		{
		}
	}
}