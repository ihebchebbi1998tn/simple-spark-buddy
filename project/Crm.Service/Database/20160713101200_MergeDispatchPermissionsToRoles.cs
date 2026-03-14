namespace Crm.Service.Database
{
	using Library.Data.MigratorDotNet.Framework;

	[Migration(20160713101200)]
    public class MergeDispatchPermissionsToRoles : Migration
    {
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF OBJECT_ID('tempdb..#RolePermissions') IS NOT NULL DROP TABLE #RolePermissions
					SELECT 
						p.PermissionId AS PermissionKey,
						(SELECT RoleId FROM CRM.[Role] WHERE Name = 'ServicePlanner') AS RoleKey,
						NULL AS TenantKey
					INTO #RolePermissions
					FROM CRM.Permission p 
					WHERE p.Name IN ('CreateDispatch', 'EditDispatch', 'SaveDispatch', 'RemoveDispatch')
					UNION ALL
					SELECT 
						p.PermissionId AS PermissionKey,
						(SELECT RoleId FROM CRM.[Role] WHERE Name = 'FieldService') AS RoleKey,
						NULL AS TenantKey
					FROM CRM.Permission p 
					WHERE p.Name IN ('CreateDispatch', 'EditDispatch', 'SaveDispatch')

					BEGIN
						MERGE CRM.RolePermission [target]
						USING #RolePermissions AS source
						ON [target].[RoleKey] = source.RoleKey
						AND [target].[PermissionKey] = source.PermissionKey
						WHEN NOT MATCHED
						THEN INSERT
							(
								[RoleKey],
								[PermissionKey]
							) VALUES (
								source.RoleKey,
								source.PermissionKey
							);
					END
					DROP TABLE #RolePermissions
			");
		}
	}
}