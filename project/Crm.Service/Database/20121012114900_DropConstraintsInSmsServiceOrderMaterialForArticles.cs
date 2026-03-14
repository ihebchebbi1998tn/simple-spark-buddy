namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20121012114900)]
	public class DropConstraintsInSmsServiceOrderMaterialForArticles : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF EXISTS (SELECT 1 from sys.objects where name = 'FK_ServiceOrderMaterial_Articles') ALTER TABLE [SMS].[ServiceOrderMaterial] DROP CONSTRAINT FK_ServiceOrderMaterial_Articles");
		}
		public override void Down()
		{
			if (!Database.ConstraintExists("[SMS].[ServiceOrderMaterial]", "FK_ServiceOrderMaterial_Articles"))
			{
				var sb = new StringBuilder();
				sb.AppendLine("ALTER TABLE [SMS].[ServiceOrderMaterial]  WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderMaterial_Articles] FOREIGN KEY([ItemNo])");
				sb.AppendLine("REFERENCES [SMS].[Old_Articles] ([ItemNo])");
				sb.AppendLine("GO");

				sb.AppendLine("ALTER TABLE [SMS].[ServiceOrderMaterial] CHECK CONSTRAINT [FK_ServiceOrderMaterial_Articles]");
				sb.AppendLine("GO");

				Database.ExecuteNonQuery(sb.ToString());
			}
		}
	}
}