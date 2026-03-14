namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230202160000)]
	public class AddInstallationCompanyRelationshipType : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[InstallationCompanyRelationshipType]"))
			{
				Database.AddTable(
					"[LU].[InstallationCompanyRelationshipType]",
					new Column("InstallationCompanyRelationshipTypeId",
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
SET IDENTITY_INSERT [LU].[InstallationCompanyRelationshipType] ON
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(1,N'InvoiceRecipient',N'Rechnungsempfänger',0,0,N'de','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(2,N'InvoiceRecipient',N'Invoice recipient',0,0,N'en','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(3,N'InvoiceRecipient',N'Receptor de la factura',0,0,N'es','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(4,N'InvoiceRecipient',N'Destinataire des factures',0,0,N'fr','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(5,N'InvoiceRecipient',N'Számlafogadó',0,0,N'hu','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)

INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(6,N'Renter',N'Mieter',0,0,N'de','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(7,N'Renter',N'Renter',0,0,N'en','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(8,N'Renter',N'Inquilino',0,0,N'es','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(9,N'Renter',N'Loueur',0,0,N'fr','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(10,N'Renter',N'Bérlo',0,0,N'hu','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)

INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(11,N'Other',N'Sonstiges',0,0,N'de','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(12,N'Other',N'Other',0,0,N'en','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(13,N'Other',N'Otras',0,0,N'es','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(14,N'Other',N'Autres',0,0,N'fr','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
INSERT [LU].[InstallationCompanyRelationshipType]([InstallationCompanyRelationshipTypeId],[Value],[Name],[Favorite],[SortOrder],[Language],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive])
VALUES(15,N'Other',N'Egyéb',0,0,N'hu','2023-02-09T14:47:05.363',N'Setup','2023-02-09T14:47:05.363',N'Setup',1)
SET IDENTITY_INSERT [LU].[InstallationCompanyRelationshipType] OFF
			");
		}
	}
}
