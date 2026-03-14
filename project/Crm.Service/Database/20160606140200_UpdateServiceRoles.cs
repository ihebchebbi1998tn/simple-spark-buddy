namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160606140200)]
	public class UpdateServiceRoles : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE [CRM].[Role] SET [Type] = 10, [ModifyDate] = GETUTCDATE() WHERE [Type] = 100 AND [Name] IN ('FieldService', 'ServicePlanner')");
		}
	}
}