namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20180702134200)]
	public class AddUnicoreTables : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);

			if (!Database.TableExists("[dbo].[EntityAuthData]"))
			{
				Database.AddTable(
					"dbo.EntityAuthData",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null, "'System'"),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null, "'System'"),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("EntityId", DbType.Guid, ColumnProperty.Null),
					new Column("EntityTypeId", DbType.Guid, ColumnProperty.NotNull),
					new Column("DomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("Circle", DbType.Guid, ColumnProperty.Null, $"'{UnicoreGlobals.AccessControl.PublicCircle}'"),
					new Column("CreatorUserId", DbType.Guid, ColumnProperty.Null),
					new Column("OwnerUserId", DbType.Guid, ColumnProperty.Null),
					new Column("SuperAuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("DomainType", DbType.Int32, ColumnProperty.NotNull, (int)DomainType.Normal),
					new Column("SuperEntityTypeId", DbType.Guid, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_EntityAuthData_EntityType", "EntityAuthData", "EntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_EntityAuthData_Domain", "EntityAuthData", "DomainId", "Domain", "UId");
				Database.AddForeignKey("FK_EntityAuthData_SuperEntityAuthData", "EntityAuthData", "SuperAuthDataId", "EntityAuthData", "UId");
				Database.AddForeignKey("FK_EntityAuthData_SuperEntityType", "EntityAuthData", "SuperEntityTypeId", "EntityType", "UId");
			}

			if (!Database.TableExists("[dbo].[DomainAuthorisedDomain]"))
			{
				Database.ExecuteNonQuery(
					@"	CREATE TABLE dbo.DomainAuthorisedDomain
											(
												DomainId UNIQUEIDENTIFIER NOT NULL,
												AuthorisedDomainId UNIQUEIDENTIFIER NOT NULL,
												CONSTRAINT [FK_DomainAuthorisedDomain_DomainId_Domain] FOREIGN KEY (DomainId) REFERENCES Domain([UId]),
												CONSTRAINT [FK_DomainAuthorisedDomain_AuthorisedDomainId_Domain] FOREIGN KEY (AuthorisedDomainId) REFERENCES Domain([UId])
											)");
				Database.ExecuteNonQuery(
					@"	INSERT INTO DomainAuthorisedDomain (DomainId, AuthorisedDomainId)
											SELECT [UId], [UId]
											FROM Domain
											WHERE IsDeleted = 0");
			}

			if (!Database.TableExists("dbo.EntityAccess"))
			{
				Database.AddTable(
					"dbo.EntityAccess",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("TargetDomainId", DbType.Guid, ColumnProperty.Null),
					new Column("TargetEntityTypeId", DbType.Guid, ColumnProperty.NotNull),
					new Column("TargetSuperEntityTypeId", DbType.Guid, ColumnProperty.Null),
					new Column("TargetEntityAuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("AccessType", DbType.Int32, ColumnProperty.NotNull),
					new Column("Operation", DbType.String, 256, ColumnProperty.NotNull),
					new Column("Allow", DbType.Boolean, ColumnProperty.NotNull),
					new Column("Template", DbType.Boolean, ColumnProperty.NotNull),
					new Column("ReferenceCount", DbType.Boolean, ColumnProperty.NotNull)
				);
				Database.AddForeignKey("FK_EntityAccess_Domain", "EntityAccess", "TargetDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_EntityAccess_EntityType", "EntityAccess", "TargetEntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_EntityAccess_SuperEntityType", "EntityAccess", "TargetSuperEntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_EntityAccess_EntityAuthData", "EntityAccess", "TargetEntityAuthDataId", "EntityAuthData", "UId");
			}

			if (!Database.TableExists("[dbo].[User]"))
			{
				var entityTypeId = helper.AddOrGetEntityTypeId<User>();
				Database.AddTable(
					"[dbo].[User]",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null, "'System'"),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null, "'System'"),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsDeleted", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("DeletedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("DomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("EntityTypeId", DbType.Guid, ColumnProperty.NotNull, $"'{entityTypeId}'"),
					new Column("AuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("UserName", DbType.String, 256, ColumnProperty.NotNull),
					new Column("Name", DbType.String, 256, ColumnProperty.Null),
					new Column("PasswordHash", DbType.String, 255, ColumnProperty.Null),
					new Column("LockoutStartDate", DbType.DateTime, ColumnProperty.Null),
					new Column("LockoutEndDate", DbType.DateTime, ColumnProperty.Null),
					new Column("LockoutEnabled", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("AccessFailedCount", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("Email", DbType.String, 256, ColumnProperty.Null),
					new Column("EmailConfirmed", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("LastLoginDate", DbType.DateTime, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_User_CrmUser", "[dbo].[User]", "UId", "[CRM].[User]", "UserID"); //TODO remove this after migration CRM.User -> dbo.User
				Database.AddForeignKey("FK_User_Domain", "[dbo].[User]", "DomainId", "Domain", "UId");
				Database.AddForeignKey("FK_User_EntityType", "[dbo].[User]", "EntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_User_EntityAuthData", "[dbo].[User]", "AuthDataId", "EntityAuthData", "UId");

				var tenantColumnExists = Database.ColumnExists("CRM.User", "TenantKeyOld");
				//TODO fully migrate CRM.User -> dbo.User
				Database.ExecuteNonQuery(
					$@"
					INSERT INTO [dbo].[User] (UId, UserName, DomainId)
					SELECT UserID, Username, COALESCE(domain.UId, '{UnicoreDefaults.CommonDomainId}')
					FROM [CRM].[User]
					LEFT JOIN Domain domain ON {(tenantColumnExists ? "[source].TenantKeyOld" : "-1")} = domain.TenantKeyOld
					WHERE IsActive = 1");
			}

			if (!Database.TableExists("dbo.GrantedEntityAccess"))
			{
				Database.AddTable(
					"dbo.GrantedEntityAccess",
					new Column("UId", DbType.String, 256, ColumnProperty.PrimaryKey),
					new Column("EntityAccessId", DbType.Guid, ColumnProperty.NotNull),
					new Column("UserId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AuthDomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("GrantTargetDomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("GrantTargetEntityAuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("TargetDomainId", DbType.Guid, ColumnProperty.Null),
					new Column("TargetEntityTypeId", DbType.Guid, ColumnProperty.NotNull),
					new Column("TargetSuperEntityTypeId", DbType.Guid, ColumnProperty.Null),
					new Column("TargetEntityAuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("Circle", DbType.Guid, ColumnProperty.Null, $"'{UnicoreGlobals.AccessControl.PublicCircle}'"),
					new Column("AccessType", DbType.Int32, ColumnProperty.NotNull),
					new Column("Operation", DbType.String, 256, ColumnProperty.NotNull),
					new Column("Allow", DbType.Boolean, ColumnProperty.NotNull),
					new Column("ReferenceCount", DbType.Boolean, ColumnProperty.NotNull)
				);
				Database.AddForeignKey("FK_GrantedEntityAccess_EntityAccess", "GrantedEntityAccess", "EntityAccessId", "EntityAccess", "UId");
				Database.AddForeignKey("FK_GrantedEntityAccess_User", "GrantedEntityAccess", "UserId", "[User]", "UId");
				Database.AddForeignKey("FK_GrantedEntityAccess_AuthDomain", "GrantedEntityAccess", "AuthDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_GrantedEntityAccess_GrantTargetDomain", "GrantedEntityAccess", "GrantTargetDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_GrantedEntityAccess_GrantTargetEntityAuthData", "GrantedEntityAccess", "GrantTargetEntityAuthDataId", "EntityAuthData", "UId");
				Database.AddForeignKey("FK_GrantedEntityAccess_TargetDomain", "GrantedEntityAccess", "TargetDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_GrantedEntityAccess_TargetEntityType", "GrantedEntityAccess", "TargetEntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_GrantedEntityAccess_TargetSuperEntityType", "GrantedEntityAccess", "TargetSuperEntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_GrantedEntityAccess_TargetEntityAuthData", "GrantedEntityAccess", "TargetEntityAuthDataId", "EntityAuthData", "UId");
			}

			if (!Database.TableExists("dbo.PermissionSchema"))
			{
				Database.AddTable(
					"dbo.PermissionSchema",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("IsDeleted", DbType.Boolean, ColumnProperty.Null),
					new Column("DeletedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("DomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("EntityTypeId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("Name", DbType.String, 256, ColumnProperty.NotNull),
					new Column("CompilationName", DbType.String, 256, ColumnProperty.Null),
					new Column("SourcePermissionSchemaId", DbType.Guid, ColumnProperty.Null),
					new Column("Usage", DbType.String, 256, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_PermissionSchema_SourcePermissionSchemaId", "PermissionSchema", "SourcePermissionSchemaId", "PermissionSchema", "UId");
				Database.AddForeignKey("FK_PermissionSchema_DomainId", "PermissionSchema", "DomainId", "Domain", "UId");
				Database.AddForeignKey("FK_PermissionSchema_EntityTypeId", "PermissionSchema", "EntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_PermissionSchema_AuthDataId", "PermissionSchema", "AuthDataId", "EntityAuthData", "UId");
			}

			if (!Database.TableExists("dbo.PermissionSchemaSettlement"))
			{
				Database.AddTable(
					"dbo.PermissionSchemaSettlement",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("PermissionSchemaId", DbType.Guid, ColumnProperty.NotNull),
					new Column("DomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("DomainAccessOptions", DbType.Int32, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_PermissionSchemaSettlement_PermissionSchemaId", "PermissionSchemaSettlement", "PermissionSchemaId", "PermissionSchema", "UId");
				Database.AddForeignKey("FK_PermissionSchemaSettlement_DomainId", "PermissionSchemaSettlement", "DomainId", "Domain", "UId");
			}

			if (!Database.TableExists("dbo.Permission"))
			{
				Database.AddTable(
					"dbo.Permission",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("IsDeleted", DbType.Boolean, ColumnProperty.Null),
					new Column("DeletedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("DomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("EntityTypeId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("Name", DbType.String, 256, ColumnProperty.NotNull),
					new Column("SchemaUsage", DbType.String, 256, ColumnProperty.Null),
					new Column("[Group]", DbType.String, 256, ColumnProperty.Null),
					new Column("DomainAccessOptions", DbType.Int32, ColumnProperty.Null),
					new Column("DomainAccessMergeType", DbType.Int32, ColumnProperty.Null),
					new Column("CircleConstraint", DbType.Int32, ColumnProperty.Null),
					new Column("CompilationName", DbType.String, 256, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_Permission_DomainId", "Permission", "DomainId", "Domain", "UId");
				Database.AddForeignKey("FK_Permission_EntityTypeId", "Permission", "EntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_Permission_AuthDataId", "Permission", "AuthDataId", "EntityAuthData", "UId");
			}

			if (!Database.TableExists("dbo.PermissionSchemaRoleAssignment"))
			{
				Database.AddTable(
					"dbo.PermissionSchemaRoleAssignment",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("PermissionSchemaId", DbType.Guid, ColumnProperty.NotNull),
					new Column("PermissionSchemaSettlementId", DbType.Guid, ColumnProperty.NotNull),
					new Column("PermissionSchemaRoleId", DbType.Guid, ColumnProperty.NotNull),
					new Column("EntityAuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("UserId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AuthDomainId", DbType.Guid, ColumnProperty.NotNull)
				);
				Database.AddForeignKey("FK_PermissionSchemaRoleAssignment_PermissionSchemaId", "PermissionSchemaRoleAssignment", "PermissionSchemaId", "PermissionSchema", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRoleAssignment_PermissionSchemaId", "PermissionSchemaRoleAssignment", "PermissionSchemaSettlementId", "PermissionSchemaSettlement", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRoleAssignment_EntityAuthDataId", "PermissionSchemaRoleAssignment", "EntityAuthDataId", "EntityAuthData", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRoleAssignment_UserId", "PermissionSchemaRoleAssignment", "UserId", "[User]", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRoleAssignment_AuthDomainId", "PermissionSchemaRoleAssignment", "AuthDomainId", "Domain", "UId");
			}

			if (!Database.TableExists("dbo.GrantedPermission"))
			{
				Database.AddTable(
					"dbo.GrantedPermission",
					new Column("UId", DbType.String, 256, ColumnProperty.PrimaryKey),
					new Column("PermissionId", DbType.Guid, ColumnProperty.NotNull),
					new Column("UserId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AuthDomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("TargetDomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("TargetEntityAuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("Circle", DbType.Guid, ColumnProperty.Null),
					new Column("ReferenceCount", DbType.Int64, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_GrantedPermission_PermissionId", "GrantedPermission", "PermissionId", "Permission", "UId", ForeignKeyConstraint.Cascade);
				Database.AddForeignKey("FK_GrantedPermission_UserId", "GrantedPermission", "UserId", "[User]", "UId");
				Database.AddForeignKey("FK_GrantedPermission_AuthDomainId", "GrantedPermission", "AuthDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_GrantedPermission_TargetDomainId", "GrantedPermission", "TargetDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_GrantedPermission_TargetEntityAuthDataId", "GrantedPermission", "TargetEntityAuthDataId", "EntityAuthData", "UId");
			}

			if (!Database.TableExists("dbo.GrantedRole"))
			{
				Database.AddTable(
					"dbo.GrantedRole",
					new Column("UId", DbType.String, 256, ColumnProperty.PrimaryKey),
					new Column("RoleId", DbType.Guid, ColumnProperty.NotNull),
					new Column("UserId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AuthDomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("TargetDomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("TargetEntityAuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("Circle", DbType.Guid, ColumnProperty.Null),
					new Column("ReferenceCount", DbType.Int64, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_GrantedRole_UserId", "GrantedRole", "UserId", "[User]", "UId");
				Database.AddForeignKey("FK_GrantedRole_AuthDomainId", "GrantedRole", "AuthDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_GrantedRole_TargetDomainId", "GrantedRole", "TargetDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_GrantedRole_TargetEntityAuthDataId", "GrantedRole", "TargetEntityAuthDataId", "EntityAuthData", "UId");
			}
			else
			{
				Database.RemoveForeignKeyIfExisting("dbo", "GrantedRole", "FK_GrantedRole_TargetDomainId");
				Database.RemoveForeignKeyIfExisting("dbo", "GrantedRole", "FK_GrantedRole_TargetEntityAuthDataId");
				Database.AddForeignKey("FK_GrantedRole_TargetDomainId", "GrantedRole", "TargetDomainId", "Domain", "UId");
				Database.AddForeignKey("FK_GrantedRole_TargetEntityAuthDataId", "GrantedRole", "TargetEntityAuthDataId", "EntityAuthData", "UId");
			}

			if (!Database.TableExists("dbo.AccessControlCompilation"))
			{
				Database.AddTable(
					"dbo.AccessControlCompilation",
					new Column("Name", DbType.String, 256, ColumnProperty.PrimaryKey),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("InstallationTime", DbType.DateTime, ColumnProperty.Null),
					new Column("HashCode", DbType.String, 256, ColumnProperty.NotNull)
				);
				Database.AddForeignKey("FK_Permission_CompilationName", "Permission", "CompilationName", "AccessControlCompilation", "Name");
				Database.AddForeignKey("FK_PermissionSchema_CompilationName", "PermissionSchema", "CompilationName", "AccessControlCompilation", "Name");
			}

			if (!Database.TableExists("dbo.PermissionSchemaRole"))
			{
				Database.AddTable(
					"dbo.PermissionSchemaRole",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("IsDeleted", DbType.Boolean, ColumnProperty.Null),
					new Column("DeletedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("DomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("EntityTypeId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("PermissionSchemaId", DbType.Guid, ColumnProperty.NotNull),
					new Column("Name", DbType.String, 256, ColumnProperty.NotNull),
					new Column("CompilationName", DbType.String, 256, ColumnProperty.Null),
					new Column("SourcePermissionSchemaRoleId", DbType.Guid, ColumnProperty.Null),
					new Column("[Group]", DbType.String, 256, ColumnProperty.Null),
					new Column("IgnoreCircles", DbType.Boolean, ColumnProperty.Null),
					new Column("FromConfiguration", DbType.Boolean, ColumnProperty.Null)
				);

				Database.AddForeignKey("FK_PermissionSchemaRole_PermissionSchemaId", "PermissionSchemaRole", "PermissionSchemaId", "PermissionSchema", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRole_CompilationName", "PermissionSchemaRole", "CompilationName", "AccessControlCompilation", "Name");
				Database.AddForeignKey("FK_PermissionSchemaRole_SourcePermissionSchemaRoleId", "PermissionSchemaRole", "SourcePermissionSchemaRoleId", "PermissionSchemaRole", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRole_DomainId", "PermissionSchemaRole", "DomainId", "Domain", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRole_EntityTypeId", "PermissionSchemaRole", "EntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRole_AuthDataId", "PermissionSchemaRole", "AuthDataId", "EntityAuthData", "UId");
				Database.AddForeignKey("FK_GrantedRole_RoleId", "GrantedRole", "RoleId", "PermissionSchemaRole", "UId");

				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaRole_PermissionSchema] ON [dbo].[PermissionSchemaRole] ([PermissionSchemaId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaRole_Compilation] ON [dbo].[PermissionSchemaRole] ([CompilationName] ASC)");
				Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_PermissionSchemaRole_Key] ON [dbo].[PermissionSchemaRole] ([PermissionSchemaId] ASC, [Name] ASC, [DeletedAt] ASC )");
			}

			if (!Database.TableExists("dbo.PermissionSchemaRolePermission"))
			{
				Database.AddTable(
					"dbo.PermissionSchemaRolePermission",
					new Column("PermissionSchemaRoleId", DbType.Guid, ColumnProperty.NotNull),
					new Column("PermissionId", DbType.Guid, ColumnProperty.NotNull)
				);

				Database.AddForeignKey("FK_PermissionSchemaRolePermission_PermissionId", "PermissionSchemaRolePermission", "PermissionId", "Permission", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRolePermission_PermissionSchemaRoleId", "PermissionSchemaRolePermission", "PermissionSchemaRoleId", "PermissionSchemaRole", "UId");
			}

			if (!Database.TableExists("dbo.PermissionGraphArc"))
			{
				Database.AddTable(
					"dbo.PermissionGraphArc",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("RootId", DbType.Guid, ColumnProperty.NotNull),
					new Column("HeadId", DbType.Guid, ColumnProperty.NotNull),
					new Column("TailId", DbType.Guid, ColumnProperty.NotNull),
					new Column("ReferenceCount", DbType.Int64, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_PermissionGraphArc_RootId", "PermissionGraphArc", "RootId", "Permission", "UId");
				Database.AddForeignKey("FK_PermissionGraphArc_HeadId", "PermissionGraphArc", "HeadId", "Permission", "UId");
				Database.AddForeignKey("FK_PermissionGraphArc_TailId", "PermissionGraphArc", "TailId", "Permission", "UId");

				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionGraphArc_Head] ON [dbo].[PermissionGraphArc] ([HeadId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionGraphArc_Root] ON [dbo].[PermissionGraphArc] ([RootId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionGraphArc_RootHead] ON [dbo].[PermissionGraphArc] ([RootId] ASC, [HeadId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionGraphArc_RootTail] ON [dbo].[PermissionGraphArc] ([RootId] ASC, [TailId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionGraphArc_Tail] ON [dbo].[PermissionGraphArc] ([TailId] ASC)");
				Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_PermissionGraphArc_Key] ON [dbo].[PermissionGraphArc] ([RootId] ASC, [HeadId] ASC, [TailId] ASC)");
			}

			if (!Database.TableExists("dbo.PermissionEntityAccess"))
			{
				Database.AddTable(
					"dbo.PermissionEntityAccess",
					new Column("PermissionId", DbType.Guid, ColumnProperty.NotNull),
					new Column("EntityAccessId", DbType.Guid, ColumnProperty.NotNull)
				);

				Database.AddForeignKey("FK_PermissionEntityAccess_EntityAccessId", "PermissionEntityAccess", "EntityAccessId", "EntityAccess", "UId");
				Database.AddForeignKey("FK_PermissionEntityAccess_PermissionId", "PermissionEntityAccess", "PermissionId", "Permission", "UId");
			}

			if (!Database.TableExists("dbo.PermissionSchemaCircle"))
			{
				Database.AddTable(
					"dbo.PermissionSchemaCircle",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("IsDeleted", DbType.Boolean, ColumnProperty.Null),
					new Column("DeletedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("DomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("EntityTypeId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("PermissionSchemaId", DbType.Guid, ColumnProperty.NotNull),
					new Column("Name", DbType.String, 256, ColumnProperty.NotNull),
					new Column("CompilationName", DbType.String, 256, ColumnProperty.Null),
					new Column("SourcePermissionSchemaCircleId", DbType.Guid, ColumnProperty.Null),
					new Column("[Group]", DbType.String, 256, ColumnProperty.Null),
					new Column("IgnoreCircles", DbType.Boolean, ColumnProperty.Null),
					new Column("FromConfiguration", DbType.Boolean, ColumnProperty.Null)
				);

				Database.AddForeignKey("FK_PermissionSchemaCircle_PermissionSchemaId", "PermissionSchemaCircle", "PermissionSchemaId", "PermissionSchema", "UId");
				Database.AddForeignKey("FK_PermissionSchemaCircle_CompilationName", "PermissionSchemaCircle", "CompilationName", "AccessControlCompilation", "Name");
				Database.AddForeignKey("FK_PermissionSchemaCircle_SourcePermissionSchemaCircleId", "PermissionSchemaCircle", "SourcePermissionSchemaCircleId", "PermissionSchemaCircle", "UId");
				Database.AddForeignKey("FK_PermissionSchemaCircle_DomainId", "PermissionSchemaCircle", "DomainId", "Domain", "UId");
				Database.AddForeignKey("FK_PermissionSchemaCircle_EntityTypeId", "PermissionSchemaCircle", "EntityTypeId", "EntityType", "UId");
				Database.AddForeignKey("FK_PermissionSchemaCircle_AuthDataId", "PermissionSchemaCircle", "AuthDataId", "EntityAuthData", "UId");

				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaCircle_Compilation] ON [dbo].[PermissionSchemaCircle] ([CompilationName] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaCircle_PermissionSchema] ON [dbo].[PermissionSchemaCircle] ([PermissionSchemaId] ASC)");
				Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_PermissionSchemaCircle_Key] ON [dbo].[PermissionSchemaCircle] ([PermissionSchemaId] ASC, [Name] ASC, [DeletedAt] ASC)");
			}

			if (!Database.TableExists("dbo.PermissionSchemaCircleRole"))
			{
				Database.AddTable(
					"dbo.PermissionSchemaCircleRole",
					new Column("PermissionSchemaCircleId", DbType.Guid, ColumnProperty.NotNull),
					new Column("PermissionSchemaRoleId", DbType.Guid, ColumnProperty.NotNull)
				);

				Database.AddForeignKey("FK_PermissionSchemaCircleRole_PermissionSchemaRoleId", "PermissionSchemaCircleRole", "PermissionSchemaRoleId", "PermissionSchemaRole", "UId");
				Database.AddForeignKey("FK_PermissionSchemaCircleRole_PermissionSchemaCircleId", "PermissionSchemaCircleRole", "PermissionSchemaCircleId", "PermissionSchemaCircle", "UId");
			}

			if (!Database.TableExists("dbo.PermissionSchemaRolePermission"))
			{
				Database.AddTable(
					"dbo.PermissionSchemaRolePermission",
					new Column("PermissionSchemaRoleId", DbType.Guid, ColumnProperty.NotNull),
					new Column("PermissionId", DbType.Guid, ColumnProperty.NotNull)
				);

				Database.AddForeignKey("FK_PermissionSchemaRolePermission_PermissionId", "PermissionSchemaRolePermission", "PermissionId", "Permission", "UId");
				Database.AddForeignKey("FK_PermissionSchemaRolePermission_PermissionSchemaRoleId", "PermissionSchemaRolePermission", "PermissionSchemaRoleId", "PermissionSchemaRole", "UId");
			}

			if (!Database.TableExists("dbo.PermissionSchemaTransaction"))
			{
				Database.AddTable(
					"dbo.PermissionSchemaTransaction",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("Type", DbType.Int32, ColumnProperty.Null),
					new Column("TargetPermissionSchemaId", DbType.Guid, ColumnProperty.NotNull),
					new Column("TargetPermissionSchemaRoleId", DbType.Guid, ColumnProperty.Null),
					new Column("TargetPermissionSchemaCircleId", DbType.Guid, ColumnProperty.Null),
					new Column("SourcePermissionId", DbType.Guid, ColumnProperty.Null),
					new Column("SourcePermissionSchemaRoleId", DbType.Guid, ColumnProperty.Null),
					new Column("SourcePermissionSchemaCircleId", DbType.Guid, ColumnProperty.Null)
				);

				Database.AddForeignKey("FK_PermissionSchemaTransaction_TargetPermissionSchemaId", "PermissionSchemaTransaction", "TargetPermissionSchemaId", "PermissionSchema", "UId");
				Database.AddForeignKey("FK_PermissionSchemaTransaction_TargetPermissionSchemaRoleId", "PermissionSchemaTransaction", "TargetPermissionSchemaRoleId", "PermissionSchemaRole", "UId");
				Database.AddForeignKey("FK_PermissionSchemaTransaction_TargetPermissionSchemaCircleId", "PermissionSchemaTransaction", "TargetPermissionSchemaCircleId", "PermissionSchemaCircle", "UId");
				Database.AddForeignKey("FK_PermissionSchemaTransaction_SourcePermissionSchemaRoleId", "PermissionSchemaTransaction", "SourcePermissionSchemaRoleId", "PermissionSchemaRole", "UId");
				Database.AddForeignKey("FK_PermissionSchemaTransaction_SourcePermissionSchemaCircleId", "PermissionSchemaTransaction", "SourcePermissionSchemaCircleId", "PermissionSchemaCircle", "UId");
				Database.AddForeignKey("FK_PermissionSchemaTransaction_SourcePermissionId", "PermissionSchemaTransaction", "SourcePermissionId", "Permission", "UId");

				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaTransaction_SourcePermission] ON [dbo].[PermissionSchemaTransaction] ([SourcePermissionId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaTransaction_SourcePermissionSchemaCircle] ON [dbo].[PermissionSchemaTransaction] ([SourcePermissionSchemaCircleId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaTransaction_SourcePermissionSchemaRole] ON [dbo].[PermissionSchemaTransaction] ([SourcePermissionSchemaRoleId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaTransaction_TargetPermissionSchema] ON [dbo].[PermissionSchemaTransaction] ([TargetPermissionSchemaId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaTransaction_TargetPermissionSchemaCircle] ON [dbo].[PermissionSchemaTransaction] ([TargetPermissionSchemaCircleId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaTransaction_TargetPermissionSchemaRole] ON [dbo].[PermissionSchemaTransaction] ([TargetPermissionSchemaRoleId] ASC)");
			}

			if (!Database.TableExists("dbo.UserSuperiority"))
			{
				Database.AddTable(
					"dbo.UserSuperiority",
					new Column("SuperiorId", DbType.Guid, ColumnProperty.NotNull),
					new Column("InferiorId", DbType.Guid, ColumnProperty.NotNull)
				);

				Database.AddForeignKey("FK_UserSuperiority_InferiorId", "UserSuperiority", "InferiorId", "[User]", "UId");
				Database.AddForeignKey("FK_UserSuperiority_SuperiorId", "UserSuperiority", "SuperiorId", "[User]", "UId");
			}

			if (!Database.TableExists("dbo.ExternalAuth"))
			{
				Database.AddTable(
					"dbo.ExternalAuth",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull),
					new Column("UserId", DbType.Guid, ColumnProperty.NotNull),
					new Column("Provider", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ProviderUserId", DbType.String, 256, ColumnProperty.NotNull)
				);

				Database.AddForeignKey("FK_ExternalAuth_UserId", "ExternalAuth", "UserId", "[User]", "UId");

				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ExternalAuth_User] ON [dbo].[ExternalAuth] ([UserId] ASC)");
				Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_ExternalAuth_Key] ON [dbo].[ExternalAuth] ([UserId] ASC, [Provider] ASC)");
				Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_ExternalAuth_ProviderUser] ON [dbo].[ExternalAuth] ([Provider] ASC, [ProviderUserId] ASC)");
			}

			if (!Database.TableExists("dbo.NameValueEntry"))
			{
				Database.AddTable(
					"dbo.NameValueEntry",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull),
					new Column("Name", DbType.String, 256, ColumnProperty.NotNull),
					new Column("DomainId", DbType.Guid, ColumnProperty.NotNull),
					new Column("EntityReferenceEntityId", DbType.Guid, ColumnProperty.Null),
					new Column("EntityReferenceEntityTypeId", DbType.Guid, ColumnProperty.Null),
					new Column("Category", DbType.String, 256, ColumnProperty.Null),
					new Column("Internal", DbType.Boolean, ColumnProperty.Null),
					new Column("ValueTypeName", DbType.String, 255, ColumnProperty.Null),
					new Column("BoolValue", DbType.Boolean, ColumnProperty.Null),
					new Column("ByteValue", DbType.Byte, ColumnProperty.Null),
					new Column("ShortValue", DbType.Int16, ColumnProperty.Null),
					new Column("IntValue", DbType.Int32, ColumnProperty.Null),
					new Column("LongValue", DbType.Int64, ColumnProperty.Null),
					new Column("FloatValue", DbType.Single, ColumnProperty.Null),
					new Column("DoubleValue", DbType.Double, ColumnProperty.Null),
					new Column("DecimalValue", DbType.Decimal, ColumnProperty.Null),
					new Column("DateTimeValue", DbType.DateTime, ColumnProperty.Null),
					new Column("TimeSpanValue", DbType.Int64, ColumnProperty.Null),
					new Column("GuidValue", DbType.Guid, ColumnProperty.Null),
					new Column("ByteArrayValue", DbType.Binary, ColumnProperty.Null),
					new Column("StringValue", DbType.String, ColumnProperty.Null),
					new Column("SerializedValueValue", DbType.Binary, ColumnProperty.Null),
					new Column("SerializedValueType", DbType.String, ColumnProperty.Null),
					new Column("SerializedValueSerializator", DbType.String, ColumnProperty.Null),
					new Column("SerializedValueVersion", DbType.String, ColumnProperty.Null)
				);

				Database.AddForeignKey("FK_NameValueEntry_DomainId", "NameValueEntry", "DomainId", "Domain", "UId");

				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_NameValueEntry_DomainEntity] ON [dbo].[NameValueEntry] ([DomainId] ASC, [EntityReferenceEntityId] ASC, [EntityReferenceEntityTypeId] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_NameValueEntry_Query] ON [dbo].[NameValueEntry] ([DomainId] ASC, [EntityReferenceEntityId] ASC, [EntityReferenceEntityTypeId] ASC, [Category] ASC, [Internal] ASC)");
				Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_NameValueEntry_Key] ON [dbo].[NameValueEntry] ([Name] ASC, [DomainId] ASC, [EntityReferenceEntityId] ASC, [EntityReferenceEntityTypeId] ASC, [Internal] ASC)");
			}

			if (!Database.TableExists("dbo.UserToken"))
			{
				Database.AddTable(
					"dbo.UserToken",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull),
					new Column("UserId", DbType.Guid, ColumnProperty.NotNull),
					new Column("Name", DbType.String, 256, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 2048, ColumnProperty.NotNull),
					new Column("ValidUntil", DbType.DateTime, ColumnProperty.Null)
				);

				Database.AddForeignKey("FK_UserToken_UserId ", "UserToken", "UserId", "[User]", "UId");

				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_UserToken_User] ON [dbo].[UserToken] ([UserId] ASC)");
				Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_UserToken_Key] ON [dbo].[UserToken] ([UserId] ASC, [Name] ASC)");
				Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_UserToken_Value] ON [dbo].[UserToken] ([Name] ASC, [Value] ASC)");
			}

			Database.AddForeignKey("FK_PermissionSchemaRoleAssignment_PermissionSchemaSettlementId", "PermissionSchemaRoleAssignment", "PermissionSchemaSettlementId", "PermissionSchemaSettlement", "UId");
			Database.AddForeignKey("FK_PermissionSchemaRoleAssignment_RoleId", "PermissionSchemaRoleAssignment", "PermissionSchemaRoleId", "PermissionSchemaRole", "UId");

			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_DomainAuthorisedDomain_AuthorisedDomain] ON [dbo].[DomainAuthorisedDomain] ([AuthorisedDomainId] ASC)");
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_DomainAuthorisedDomain_Domain] ON [dbo].[DomainAuthorisedDomain] ([DomainId] ASC)");

			Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_Domain_Name] ON [dbo].[Domain] ([Name] ASC, [DeletedAt] ASC)");

			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Permission_Compilation] ON [dbo].[Permission] ([CompilationName] ASC)");
			Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_Permission_Name] ON [dbo].[Permission] ([Name] ASC, [DeletedAt] ASC)");

			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchema_Compilation] ON [dbo].[PermissionSchema] ([CompilationName] ASC)");
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchema_SourcePermissionSchema] ON [dbo].[PermissionSchema] ([SourcePermissionSchemaId] ASC)");
			Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_PermissionSchema_Name] ON [dbo].[PermissionSchema] ([Name] ASC, [DeletedAt] ASC)");

			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaRoleAssignment_Role] ON [dbo].[PermissionSchemaRoleAssignment] ([PermissionSchemaRoleId] ASC)");
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaRoleAssignment_User] ON [dbo].[PermissionSchemaRoleAssignment] ([UserId] ASC)");
			Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_PermissionSchemaRoleAssignment_Key] ON [dbo].[PermissionSchemaRoleAssignment] ([PermissionSchemaSettlementId] ASC, [PermissionSchemaRoleId] ASC, [EntityAuthDataId] ASC, [UserId] ASC, [AuthDomainId] ASC)");

			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_PermissionSchemaSettlement_PermissionSchema] ON [dbo].[PermissionSchemaSettlement] ([PermissionSchemaId] ASC)");
			Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_PermissionSchemaSettlement_Key] ON [dbo].[PermissionSchemaSettlement] ([PermissionSchemaId] ASC, [DomainId] ASC)");

			Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX [UX_User_UserName] ON [dbo].[User] ([UserName] ASC, [DeletedAt] ASC)");

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Permission>("dbo", "Permission");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<PermissionSchema>("dbo", "PermissionSchema");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<PermissionSchemaCircle>("dbo", "PermissionSchemaCircle");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<PermissionSchemaRole>("dbo", "PermissionSchemaRole");
		}
	}
}
