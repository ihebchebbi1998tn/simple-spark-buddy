namespace Crm.Service.Database
{
	using System.Data;

	using Library.Data.MigratorDotNet.Framework;

	[Migration(20160811112500)]
	public class ChangeContractIsActiveToIsExpired : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[SMS].[ServiceContract]", "IsExpired"))
			{
				Database.AddColumn("[SMS].[ServiceContract]", new Column("IsExpired", DbType.Boolean, ColumnProperty.NotNull, 0));

				Database.ExecuteNonQuery(@"UPDATE sc
																		SET IsExpired = CASE WHEN c.IsActive = 0 THEN 1 ELSE 0 END
																		FROM SMS.ServiceContract sc
																		JOIN CRM.Contact c ON sc.ContactKey = c.ContactId");

				Database.ExecuteNonQuery(@"UPDATE CRM.Contact SET IsActive = 1, ModifyDate = getutcdate() WHERE ContactType = 'ServiceContract'");
			}
		}
	}
}