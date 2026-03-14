namespace Crm.Service.Database
{
	using System;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130624105015)]
	public class AddIndexLocationContactIdToSmsInstallationHead: Migration
	{
		public override void Up()
		{
			var sb = new StringBuilder();
			sb.AppendLine("IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_InstallationHead_LocationContactId')");
			sb.AppendLine("CREATE NONCLUSTERED INDEX [IX_InstallationHead_LocationContactId] ON [SMS].[InstallationHead] ([LocationContactId] ASC);");
			Database.ExecuteNonQuery(sb.ToString());
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}