namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160825210000)]
	public class CreateMissingIndicesInContactAndNote : Migration
	{
		public override void Up()
		{
			if (!Database.IndexExists("[CRM].[Contact]", "IX_Contact_IsActive"))
			{
				Database.ExecuteNonQuery(@"
					CREATE NONCLUSTERED INDEX[IX_Contact_IsActive]
					ON [CRM].[Contact] ([IsActive])
					INCLUDE([CreateUser],[Visibility],[ContactId],[ParentKey])
				");
			}

			if (!Database.IndexExists("[CRM].[Note]", "IX_Note_IsActive_Plugin"))
			{
				Database.ExecuteNonQuery(@"
					CREATE NONCLUSTERED INDEX[IX_Note_IsActive_Plugin]
					ON [CRM].[Note] ([IsActive],[Plugin])
					INCLUDE([ElementKey])
				");
			}
		}
	}
}