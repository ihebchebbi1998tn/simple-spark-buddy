namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220719164500)]
	public class AddNonInternalServiceCaseCreateUserAndCreateDate : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceNotifications", new Column("ServiceCaseCreateUser", DbType.String, 60, ColumnProperty.NotNull, "'System'"));
			Database.AddColumnIfNotExisting("SMS.ServiceNotifications", new Column("ServiceCaseCreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.ExecuteNonQuery(@"UPDATE sc
																		SET sc.ServiceCaseCreateUser = c.CreateUser, sc.ServiceCaseCreateDate = c.CreateDate
																		FROM SMS.ServiceNotifications sc 
																		JOIN CRM.Contact c ON sc.ContactKey = c.ContactId");
		}
	}
}