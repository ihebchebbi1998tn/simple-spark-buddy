namespace Crm.Service.Team.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240419161600)]
	public class AddValidFromAndValidToToCrmUsergroup : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.UserGroup",
				new Column("ValidFrom",
					DbType.DateTime,
					ColumnProperty.Null));
			Database.AddColumnIfNotExisting("CRM.UserGroup",
				new Column("ValidTo",
					DbType.DateTime,
					ColumnProperty.Null));
		}
	}
}
