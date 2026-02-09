namespace Main.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180517091400)]
	public class RemoveTriggersForVisibilityChanges : Migration
	{
		public override void Up()
		{
			if (Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM sys.triggers WHERE name = 'UpdateContactOnContactUserInsert'")) == 1)
			{
				Database.ExecuteNonQuery("DROP TRIGGER [CRM].[UpdateContactOnContactUserInsert]");
			}

			if (Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM sys.triggers WHERE name = 'UpdateContactOnContactUserUpdate'")) == 1)
			{
				Database.ExecuteNonQuery("DROP TRIGGER [CRM].[UpdateContactOnContactUserUpdate]");
			}

			if (Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM sys.triggers WHERE name = 'UpdateContactOnContactUserDelete'")) == 1)
			{
				Database.ExecuteNonQuery("DROP TRIGGER [CRM].[UpdateContactOnContactUserDelete]");
			}

			if (Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM sys.triggers WHERE name = 'UpdateContactOnContactUserGroupInsert'")) == 1)
			{
				Database.ExecuteNonQuery("DROP TRIGGER [CRM].[UpdateContactOnContactUserGroupInsert]");
			}

			if (Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM sys.triggers WHERE name = 'UpdateContactOnContactUserGroupUpdate'")) == 1)
			{
				Database.ExecuteNonQuery("DROP TRIGGER [CRM].[UpdateContactOnContactUserGroupUpdate]");
			}

			if (Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM sys.triggers WHERE name = 'UpdateContactOnContactUserGroupDelete'")) == 1)
			{
				Database.ExecuteNonQuery("DROP TRIGGER [CRM].[UpdateContactOnContactUserGroupDelete]");
			}
		}
	}
}
