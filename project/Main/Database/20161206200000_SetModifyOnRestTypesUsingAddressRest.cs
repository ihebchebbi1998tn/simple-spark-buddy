namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161206200000)]
	public class SetModifyOnRestTypesUsingAddressRest : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"UPDATE [CRM].[Contact] SET ModifyDate = GETUTCDATE() WHERE IsActive = 1");
		}
	}
}
