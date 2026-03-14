namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130917091100)]
	public class RenameColumnInstallationPosSerials : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[SMS].[InstallationPosSerials]", "CreateUser"))
			{
				Database.ExecuteNonQuery("sp_RENAME 'SMS.InstallationPosSerials.CreatorId', 'CreateUser' ALTER TABLE SMS.[InstallationPosSerials] ALTER COLUMN CreateUser nvarchar(60)");
			}
			if (!Database.ColumnExists("[SMS].[InstallationPosSerials]", "ModifyUser"))
			{
				Database.ExecuteNonQuery("sp_RENAME 'SMS.InstallationPosSerials.ModifyId', 'ModifyUser' ALTER TABLE SMS.[InstallationPosSerials] ALTER COLUMN ModifyUser nvarchar(60)");
			}
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}