namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130815161212)]
	public class AddPreferredTechnicianAndPreferredTechnicianUsergroupToServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("PreferredTechnician", DbType.String, 256, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("PreferredTechnicianUsergroup", DbType.String, 100, ColumnProperty.Null));
		}
	}
}