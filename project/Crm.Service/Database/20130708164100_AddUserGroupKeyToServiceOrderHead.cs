namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130708164100)]
	public class AddUserGroupKeyToServiceOrderHead : Migration
	{
		public override void Up()
		{
			var usergroupIdIsGuid = (int)Database.ExecuteScalar(@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Usergroup' AND COLUMN_NAME='UsergroupId' AND DATA_TYPE = 'uniqueidentifier'") == 1;
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("UserGroupKey", usergroupIdIsGuid ? DbType.Guid : DbType.Int32, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}