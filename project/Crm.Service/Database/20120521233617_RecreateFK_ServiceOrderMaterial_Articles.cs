namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120521233617)]
	public class RecreateFK_ServiceOrderMaterial_Articles : Migration
	{
		public override void Up()
		{
			if (Database.ConstraintExists("[SMS].[ServiceOrderMaterial]", "FK_ServiceOrderMaterial_Articles") &&
					Database.TableExists("[CRM].[Article]"))
			{
				Database.RemoveConstraint("[SMS].[ServiceOrderMaterial]", "FK_ServiceOrderMaterial_Articles");
				
				StringBuilder sb = new StringBuilder();

				sb.AppendLine("ALTER TABLE [SMS].[ServiceOrderMaterial]");
				sb.AppendLine("WITH NOCHECK");
				sb.AppendLine("ADD CONSTRAINT [FK_ServiceOrderMaterial_Articles]");
				sb.AppendLine("FOREIGN KEY([ItemNo]) REFERENCES [CRM].[Article] ([ItemNo])");

				Database.ExecuteNonQuery(sb.ToString());
			}
		}
		public override void Down()
		{
		}
	}
}