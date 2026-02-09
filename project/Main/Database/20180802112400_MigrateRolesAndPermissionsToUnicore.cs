namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	[Migration(20180802112400)]
	public class MigrateRolesAndPermissionsToUnicore : Migration
	{
		public override void Up()
		{
			var migrationHelper = new UnicoreMigrationHelper(Database);

			Database.ExecuteNonQuery(
				@"INSERT INTO [AccessControlCompilation]
				(Version,
				 InstallationTime,
				 HashCode,
				 Name
				)
				VALUES 
				(1,
				 GETUTCDATE(),
				 0,
				 'DefaultCompilation'
				)"
			);

			var permissionEntityTypeId = migrationHelper.AddOrGetEntityTypeId<Permission>();
			Database.ExecuteNonQuery(
				$@"INSERT INTO [Permission]
				(Version,
				 IsDeleted,
				 DeletedAt,
				 Name,
				 SchemaUsage,
				 [Group],
				 DomainAccessOptions,
				 DomainAccessMergeType,
				 CircleConstraint,
				 CompilationName,
				 DomainId,
				 EntityTypeId,
				 AuthDataId,
				 CreatedBy,
				 CreatedAt,
				 ModifiedBy,
				 ModifiedAt
				)
				SELECT 
					1, 
					0, 
					NULL, 
					PGroup + '::' + Name, 
					'{UnicoreDefaults.DefaultPermissionSchemaUsage}',
					PluginName,
					1,
					0,
					0,
					'DefaultCompilation',
					'{UnicoreDefaults.CommonDomainId}',
					'{permissionEntityTypeId}',
					NULL,
					'Migration',
					GETUTCDATE(),
					'Migration',
					GETUTCDATE()
				FROM [CRM].[Permission]"
			);
			migrationHelper.AddOrUpdateEntityAuthDataColumn(permissionEntityTypeId, "dbo", "Permission", "UId", modifyDateColumn: "ModifiedAt", modifyUserColumn: "ModifiedBy");

			var permissioSchemaEntityTypeId = migrationHelper.AddOrGetEntityTypeId<PermissionSchema>();
			Database.ExecuteNonQuery(
				$@"INSERT INTO [PermissionSchema]
				(Version,
				 IsDeleted,
				 DeletedAt,
				 Name,
				 CompilationName,
				 SourcePermissionSchemaId,
				 Usage,
				 DomainId,
				 EntityTypeId,
				 AuthDataId,
				 CreatedBy,
				 CreatedAt,
				 ModifiedBy,
				 ModifiedAt
				)
				VALUES
				(1 ,
				 0 ,
				 NULL ,
				 '{UnicoreDefaults.DefaultPermissionSchema}',
				 'DefaultCompilation',
				 NULL,
				 '{UnicoreDefaults.DefaultPermissionSchemaUsage}',
				 '{UnicoreDefaults.CommonDomainId}',
				 '{permissioSchemaEntityTypeId}',
				 NULL,
				 'Migration',
				 GETUTCDATE(),
				 'Migration',
				 GETUTCDATE()
				)"
			);
			Database.ExecuteNonQuery(
				$@"INSERT INTO [PermissionSchema]
				(Version,
				 IsDeleted,
				 DeletedAt,
				 Name,
				 CompilationName,
				 SourcePermissionSchemaId,
				 Usage,
				 DomainId,
				 EntityTypeId,
				 AuthDataId,
				 CreatedBy,
				 CreatedAt,
				 ModifiedBy,
				 ModifiedAt)
				VALUES
				(1,
				 0,
				 NULL,
				 '{UnicoreDefaults.TemplatePermissionSchema}',
				 'DefaultCompilation',
				 NULL,
				 '{UnicoreDefaults.DefaultPermissionSchemaUsage}',
				 '{UnicoreDefaults.CommonDomainId}',
				 '{permissioSchemaEntityTypeId}',
				 NULL,
				 'Migration',
				 GETUTCDATE(),
				 'Migration',
				 GETUTCDATE()
				)"
			);
			migrationHelper.AddOrUpdateEntityAuthDataColumn(permissioSchemaEntityTypeId, "dbo", "PermissionSchema", "UId", modifyDateColumn: "ModifiedAt", modifyUserColumn: "ModifiedBy");

			var defaultPermissionSchemaId = Database.ExecuteScalar($"SELECT [UId] FROM [dbo].[PermissionSchema] WHERE [Name] = '{UnicoreDefaults.DefaultPermissionSchema}'");
			var permissionSchemaRoleEntityTypeId = migrationHelper.AddOrGetEntityTypeId<PermissionSchemaRole>();
			Database.ExecuteNonQuery(
				$@"
				INSERT INTO [PermissionSchemaRole]
				(Version,
				 IsDeleted,
				 DeletedAt,
				 PermissionSchemaId,
				 Name,
				 CompilationName,
				 SourcePermissionSchemaRoleId,
				 [Group],
				 IgnoreCircles,
				 FromConfiguration,
				 DomainId,
				 EntityTypeId,
				 AuthDataId,
				 CreatedBy,
				 CreatedAt,
				 ModifiedBy,
				 ModifiedAt
				)
				SELECT
					1,
					0,
					NULL,
					'{defaultPermissionSchemaId}',
					Name,
					CASE WHEN Name = '{RoleName.Administrator}' THEN 'DefaultCompilation' ELSE NULL END,
					NULL,
					NULL,
					0,
					0,
					'{UnicoreDefaults.CommonDomainId}',
					'{permissionSchemaRoleEntityTypeId}',
					NULL,
					'Migration',
					GETUTCDATE(),
					'Migration',
					GETUTCDATE()
				FROM [CRM].[Role]
				WHERE Name = '{RoleName.Administrator}' OR EXISTS (SELECT TOP 1 NULL FROM [CRM].[UserRole] WHERE [RoleKey] = [RoleId])"
			);
			migrationHelper.AddOrUpdateEntityAuthDataColumn(permissionSchemaRoleEntityTypeId, "dbo", "PermissionSchemaRole", "UId", modifyDateColumn: "ModifiedAt", modifyUserColumn: "ModifiedBy");

			Database.ExecuteNonQuery(
				@"
				INSERT INTO PermissionSchemaRolePermission
				(PermissionSchemaRoleId,
				 PermissionId)
				SELECT
					unicoreRole.[UId],
					unicorePermission.[UId]
				FROM [dbo].[PermissionSchemaRole] unicoreRole
				JOIN [CRM].[Role] crmRole ON unicoreRole.[Name] = crmRole.[Name]
				JOIN [CRM].[RolePermission] ON crmRole.[RoleId] = [CRM].[RolePermission].[RoleKey]
				JOIN [CRM].[Permission] crmPermission ON [CRM].[RolePermission].[PermissionKey] = crmPermission.[PermissionId]
				JOIN [dbo].[Permission] unicorePermission ON crmPermission.[PGroup] + '::' + crmPermission.[Name] = unicorePermission.[Name]"
			);

			Database.ExecuteNonQuery(
				$@"
				INSERT INTO [PermissionSchemaSettlement]
				(Version,
				 PermissionSchemaId,
				 DomainId,
				 DomainAccessOptions)
				VALUES
				(1,
				 '{defaultPermissionSchemaId}',
				 '{UnicoreDefaults.CommonDomainId}',
				 1
				)"
			);

			var permissionSchemaSettlementId = Database.ExecuteScalar("SELECT [UId] FROM [dbo].[PermissionSchemaSettlement]");
			Database.ExecuteNonQuery(
				$@"
				INSERT INTO [dbo].[PermissionSchemaRoleAssignment]
					([Version],
					 [PermissionSchemaId],
					 [PermissionSchemaSettlementId],
					 [PermissionSchemaRoleId],
					 [UserId],
					 [AuthDomainId]
					)
				SELECT
					1,
					'{defaultPermissionSchemaId}',
					'{permissionSchemaSettlementId}',
					[dbo].[PermissionSchemaRole].[UId],
					[dbo].[User].[UId],
					'{UnicoreDefaults.CommonDomainId}'
				FROM [CRM].[UserRole]
				JOIN [CRM].[Role] ON [CRM].[Role].[RoleId] = [CRM].[UserRole].[RoleKey]
				JOIN [dbo].[PermissionSchemaRole] ON [dbo].[PermissionSchemaRole].[Name] = [CRM].[Role].[Name]
				JOIN [dbo].[User] ON [dbo].[User].[UserName] = [CRM].[UserRole].[Username]"
			);

			Database.RemoveTable("[CRM].[UserRole]");
			Database.RemoveTable("[CRM].[UserPermission]");
			Database.RemoveTable("[CRM].[RolePermission]");
			Database.RemoveTable("[CRM].[Role]");
			Database.RemoveTable("[CRM].[Permission]");
		}
	}
}
