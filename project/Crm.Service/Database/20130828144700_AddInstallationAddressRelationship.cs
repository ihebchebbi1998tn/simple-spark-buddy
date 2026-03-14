namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130828144700)]
	public class AddInstallationAddressRelationship : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("SMS.InstallationAddressRelationship"))
			{
				Database.AddTable("SMS.InstallationAddressRelationship",
					new Column("InstallationAddressRelationshipId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("InstallationKey", DbType.Int32, ColumnProperty.NotNull),
					new Column("AddressKey", DbType.Int32, ColumnProperty.NotNull),
					new Column("RelationshipType", DbType.String, 50, ColumnProperty.NotNull),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("Information", DbType.String, Int32.MaxValue, ColumnProperty.Null),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null));
			}
			if (!Database.TableExists("LU.InstallationAddressRelationshipType"))
			{
				Database.AddTable("LU.InstallationAddressRelationshipType",
					new Column("InstallationAddressRelationshipTypeId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null));

				InsertLookupValue("InvoiceRecipient", "Rechnungsempfänger", "de");
				InsertLookupValue("InvoiceRecipient", "Invoice recipient", "en");
			}
		}
		private void InsertLookupValue(string value, string name, string language)
		{
			Database.ExecuteNonQuery(String.Format("INSERT INTO [LU].[InstallationAddressRelationshipType] " +
			                         "([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive])" +
			                         "VALUES ('{0}', '{1}', '{2}', '0', '0', GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', '1')",
															 value, name, language));
		}
	}
}