namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160317104000)]
	public class AddAvatarToCrmUser : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[User]", "Avatar"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[User] ADD [Avatar] varbinary(max) NULL");
			}
		}
	}
}