namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714111103)]
	public class UpdateRolePermissionAndUserPermissionAndRemoveDuplicatePermissions : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();
			query.AppendLine(@"
					select 
						Name, PluginName, PGroup, count(*) as [Count]
					into 
						CRM.tmpDuplicatedPermissions
					from
						crm.Permission d2
					group by Name, PluginName, PGroup
					having count(*) > 1

					declare @name nvarchar(1000);
					declare @pluginName nvarchar(1000);
					declare @pGroup nvarchar(1000);
					declare @permissionId int;
					while exists (select * from CRM.tmpDuplicatedPermissions)
					begin
	
						BEGIN TRY
							-- get first permission from temporary duplicate permission table
							select top 1
								@name = Name
								,@pluginName = PluginName
								,@pGroup = PGroup
							from
								CRM.tmpDuplicatedPermissions
								order by 
								Name, PluginName, PGroup;

							print 'Current: Name=' + @name
									+ ' | PluginName=' + @pluginName
									+ ' | PGroup=' + @pGroup;

							-- get one PermissionId which is used as remaining one
							SELECT TOP 1
								@permissionId = PermissionId
							FROM
								CRM.Permission
							WHERE
								Name = @name
								and PluginName = @pluginName
								and (PGroup = @pGroup or PGroup is null);

							print 'Current PermissionId: ' + cast(@permissionId as nvarchar);

							-- get duplicated roles
							SELECT
								r2.RoleKey
								,r2.PermissionKey
							INTO
								CRM.tmpRolePermissionToDelete
							FROM
								CRM.RolePermission r
							JOIN 
								CRM.RolePermission r2
							ON 
								r.RoleKey = r2.RoleKey
							WHERE 
								r.PermissionKey = @permissionId
							AND 
								r2.PermissionKey in
								(
									SELECT
										PermissionId
									FROM
										CRM.Permission
									WHERE
										Name = @name
										and PluginName = @pluginName
										and (PGroup = @pGroup or PGroup is null)
										and PermissionId <> @permissionId
								)

							-- delete duplicated roles
							DELETE
								r
							FROM
								CRM.RolePermission r
							JOIN 
								CRM.tmpRolePermissionToDelete d
							ON 
								r.RoleKey = d.RoleKey
								AND	
								r.PermissionKey = d.PermissionKey;

							-- update PermissionKeys to remaining PermissionId
							UPDATE
								CRM.RolePermission
							SET
								PermissionKey = @permissionId
							WHERE PermissionKey in
							(
								SELECT
									PermissionId
								FROM
									CRM.Permission
								WHERE
									Name = @name
									and PluginName = @pluginName
									and (PGroup = @pGroup or PGroup is null)
									and PermissionId <> @permissionId
							)

							-- get duplicated UserPermissions
							SELECT
								u2.PermissionKey
								,u2.Username
							INTO
								CRM.tmpUserPermissionToDelete
							FROM
								CRM.UserPermission u
							JOIN 
								CRM.UserPermission u2
							ON 
								u.Username = u2.Username
							WHERE 
								u.PermissionKey = @permissionId
							AND 
								u2.PermissionKey in
								(
									SELECT
										PermissionId
									FROM
										CRM.Permission
									WHERE
										Name = @name
										and PluginName = @pluginName
										and (PGroup = @pGroup or PGroup is null)
										and PermissionId <> @permissionId
								)

							-- delete duplicated UserPermissions
							DELETE
								u
							FROM
								CRM.UserPermission u
							JOIN 
								CRM.tmpUserPermissionToDelete d
							ON 
								u.Username = d.Username
								AND	
								u.PermissionKey = d.PermissionKey;

							-- update PermissionKeys to remaining PermissionId
							UPDATE
								CRM.UserPermission
							SET
								PermissionKey = @permissionId
							WHERE PermissionKey in
							(
								SELECT
									PermissionId
								FROM
									CRM.Permission
								WHERE
									Name = @name
									and PluginName = @pluginName
									and (PGroup = @pGroup or PGroup is null)
									and PermissionId <> @permissionId
							)

							-- delate duplicated permissions
							DELETE 
								CRM.Permission
							WHERE
								Name = @name
								and PluginName = @pluginName
								and (PGroup = @pGroup or PGroup is null)
								and PermissionId <> @permissionId

							IF @@ROWCOUNT = 1
							BEGIN
								print 'Dataset deleted: Name=' + @name
									+ ' | PluginName=' + @pluginName
									+ ' | PGroup=' + @pGroup;
							END
							ELSE
							BEGIN
								print 'Dataset could NOT be deleted: Name=' + @name
									+ ' | PluginName=' + @pluginName
									+ ' | PGroup=' + @pGroup;
							END;

							DELETE 
								CRM.tmpDuplicatedPermissions
							WHERE
								Name = @name
								and PluginName = @pluginName
								and (PGroup = @pGroup or PGroup is null);

							IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
									WHERE TABLE_SCHEMA = 'CRM' AND TABLE_NAME = N'tmpRolePermissionToDelete')
							BEGIN
								DROP TABLE CRM.tmpRolePermissionToDelete;
							END;

							IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
									WHERE TABLE_SCHEMA = 'CRM' AND TABLE_NAME = N'tmpUserPermissionToDelete')
							BEGIN
								DROP TABLE CRM.tmpUserPermissionToDelete;
							END;
						END TRY
						BEGIN CATCH
							
							IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
									WHERE TABLE_SCHEMA = 'CRM' AND TABLE_NAME = N'tmpRolePermissionToDelete')
							BEGIN
								DROP TABLE CRM.tmpRolePermissionToDelete;
							END;

							IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
									WHERE TABLE_SCHEMA = 'CRM' AND TABLE_NAME = N'tmpUserPermissionToDelete')
							BEGIN
								DROP TABLE CRM.tmpUserPermissionToDelete;
							END;
						END CATCH
					end			
			");
			Database.ExecuteNonQuery(query.ToString());
			Database.RemoveTable("CRM.tmpDuplicatedPermissions");
		}
	}
}