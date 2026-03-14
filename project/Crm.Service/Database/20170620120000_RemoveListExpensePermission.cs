namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170620120000)]
	public class RemoveListExpensePermission : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				UPDATE [CRM].[Permission]
				SET Type = 0
					, ModifyUser = 'Migration_20170620120000'
					, ModifyDate = GETUTCDATE()
					, IsActive = 0
				WHERE PGroup = 'Miscellaneous' AND Name = 'ListExpense'");
		}
	}
}