namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	using Sms.Scheduler.Model;

	[Migration(20241001132600)]
	public class RemoveAuthDataIdFromDispatchPersonAssignment : Migration
	{
		public override void Up()
		{
			var entityTypeId = Database.ExecuteScalar($"SELECT UId FROM EntityType WHERE IsDeleted = 0 AND [Name] = '{typeof(DispatchPersonAssignment).FullName}'");

			if (entityTypeId != null)
			{
				Database.RemoveForeignKeyIfExisting("SMS", "DispatchPersonAssignment", "FK_DispatchPersonAssignment_EntityAuthData");
				if (Database.IndexExists("[SMS].[DispatchPersonAssignment]", "IX_DispatchPersonAssignment_AuthDataId_IsActive"))
				{
					Database.ExecuteNonQuery(@"DROP INDEX [IX_DispatchPersonAssignment_AuthDataId_IsActive] ON [SMS].[DispatchPersonAssignment]");
				}
				Database.RemoveColumnIfExisting("[SMS].[DispatchPersonAssignment]", "AuthDataId");
				Database.ExecuteNonQuery($"DELETE FROM [GrantedEntityAccess] WHERE [TargetEntityTypeId] = '{entityTypeId}'");
				Database.ExecuteNonQuery($"DELETE FROM [EntityAccess] WHERE [TargetEntityTypeId] = '{entityTypeId}'");
				Database.ExecuteNonQuery($"UPDATE [EntityType] SET IsDeleted = 1 WHERE [UId] = '{entityTypeId}'");
			}
		}
	}
}
