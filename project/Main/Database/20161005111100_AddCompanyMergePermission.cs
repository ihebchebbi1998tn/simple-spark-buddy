namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161005111100)]
	public class AddCompanyMergePermission : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM CRM.Permission WHERE PGroup = 'Company' AND Name = 'Merge')
				INSERT INTO CRM.Permission(Name, PluginName, CreateDate, ModifyDate, Status, Type, PGroup, CreateUser, ModifyUser, IsActive) 
				VALUES ('Merge', 'Main', GETUTCDATE(), GETUTCDATE(), 1, 0, 'Company', 'Setup', 'Setup', 1)");
		}
	}
}
