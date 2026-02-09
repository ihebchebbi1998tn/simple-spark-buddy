namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230202161000)]
	public class AddInstallationPersonRelationshipType : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[InstallationPersonRelationshipType]"))
			{
				Database.AddTable(
					"[LU].[InstallationPersonRelationshipType]",
					new Column("InstallationPersonRelationshipTypeId",
						DbType.Int32,
						ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value",
						DbType.String,
						20,
						ColumnProperty.NotNull),
					new Column("Name",
						DbType.String,
						100,
						ColumnProperty.NotNull),
					new Column("Favorite",
						DbType.Boolean,
						ColumnProperty.NotNull,
						false),
					new Column("SortOrder",
						DbType.Int32,
						ColumnProperty.NotNull,
						0),
					new Column("Language",
						DbType.String,
						2,
						ColumnProperty.NotNull),
					new Column("CreateDate",
						DbType.DateTime,
						ColumnProperty.NotNull,
						"GETUTCDATE()"),
					new Column("CreateUser",
						DbType.String,
						ColumnProperty.NotNull,
						"'Setup'"),
					new Column("ModifyDate",
						DbType.DateTime,
						ColumnProperty.NotNull,
						"GETUTCDATE()"),
					new Column("ModifyUser",
						DbType.String,
						ColumnProperty.NotNull,
						"'Setup'"),
					new Column("IsActive",
						DbType.Boolean,
						ColumnProperty.NotNull,
						true)
				);
			}

			Database.ExecuteNonQuery(@"
SET IDENTITY_INSERT [LU].[InstallationPersonRelationshipType] ON
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(1,N'Responsible',N'Sachbearbeiter',0,0,N'de','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(2,N'Responsible',N'Responsible person',0,0,N'en','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(3,N'Responsible',N'Persona responsable',0,0,N'es','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(4,N'Responsible',N'Responsable du dossier',0,0,N'fr','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(5,N'Responsible',N'Ügyintéző',0,0,N'hu','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)

INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(6,N'Other',N'Sonstiges',0,0,N'de','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(7,N'Other',N'Other',0,0,N'en','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(8,N'Other',N'Otras',0,0,N'es','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(9,N'Other',N'Autres',0,0,N'fr','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
INSERT [LU].[InstallationPersonRelationshipType]([InstallationPersonRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(10,N'Other',N'Egyéb',0,0,N'hu','2023-02-09T14:47:05.377',N'Setup','2023-02-09T14:47:05.377',N'Setup',1)
SET IDENTITY_INSERT [LU].[InstallationPersonRelationshipType] OFF
");
		}
	}
}
