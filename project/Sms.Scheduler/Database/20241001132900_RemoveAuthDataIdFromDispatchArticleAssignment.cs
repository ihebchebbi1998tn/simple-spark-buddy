namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	using Sms.Scheduler.Model;

	[Migration(20241001132900)]
	public class RemoveAuthDataIdFromDispatchArticleAssignment : Migration
	{
		public override void Up()
		{
			var entityTypeId = Database.ExecuteScalar($"SELECT UId FROM EntityType WHERE IsDeleted = 0 AND [Name] = '{typeof(DispatchArticleAssignment).FullName}'");

			if (entityTypeId != null)
			{
				Database.RemoveForeignKeyIfExisting("SMS", "DispatchArticleAssignment", "FK_DispatchArticleAssignment_EntityAuthData");
				if (Database.IndexExists("[SMS].[DispatchArticleAssignment]", "IX_DispatchArticleAssignment_AuthDataId_IsActive"))
				{
					Database.ExecuteNonQuery(@"DROP INDEX [IX_DispatchArticleAssignment_AuthDataId_IsActive] ON [SMS].[DispatchArticleAssignment]");
				}
				Database.RemoveColumnIfExisting("[SMS].[DispatchArticleAssignment]", "AuthDataId");
				Database.ExecuteNonQuery($"DELETE FROM [GrantedEntityAccess] WHERE [TargetEntityTypeId] = '{entityTypeId}'");
				Database.ExecuteNonQuery($"DELETE FROM [EntityAccess] WHERE [TargetEntityTypeId] = '{entityTypeId}'");
				Database.ExecuteNonQuery($"UPDATE [EntityType] SET IsDeleted = 1 WHERE [UId] = '{entityTypeId}'");
			}
		}
	}
}
