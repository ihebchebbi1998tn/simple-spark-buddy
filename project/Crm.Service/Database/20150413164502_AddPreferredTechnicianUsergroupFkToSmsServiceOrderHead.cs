namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20150413164502)]
	public class AddPreferredTechnicianUsergroupFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_PreferredTechnicianUsergroup'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[PreferredTechnicianUsergroup] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[UserGroup] ug ON soh.[PreferredTechnicianUsergroup] = ug.[UserGroupId] WHERE ug.[UserGroupId] IS NULL");
				var usergroupIdIsGuid = (int)Database.ExecuteScalar(@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Usergroup' AND COLUMN_NAME='UsergroupId' AND DATA_TYPE = 'uniqueidentifier'") == 1;
				Database.ChangeColumn("[SMS].[ServiceOrderHead]", new Column("PreferredTechnicianUsergroup", usergroupIdIsGuid ? DbType.Guid : DbType.Int32, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceOrderHead_PreferredTechnicianUsergroup", "[SMS].[ServiceOrderHead]", "PreferredTechnicianUsergroup", "[CRM].[UserGroup]", "UserGroupId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}