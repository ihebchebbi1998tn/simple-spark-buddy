namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130919103619)]
	public class AddFolderKeyToSmsInstallation : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.InstallationHead", new Column("FolderKey", DbType.Int32, ColumnProperty.Null));
		}
	}
}