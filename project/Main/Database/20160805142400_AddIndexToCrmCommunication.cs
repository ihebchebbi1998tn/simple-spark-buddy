namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160805142400)]
	public class AddIndexToCrmCommunication : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Communication]') AND name = N'IX_Communication_GroupKey_AddressKey_ContactKey_IsActive'") > 0)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Communication_GroupKey_AddressKey_ContactKey_IsActive] ON [CRM].[Communication]");
			}
			Database.ExecuteNonQuery(@"
				CREATE NONCLUSTERED INDEX [IX_Communication_GroupKey_AddressKey_ContactKey_IsActive] ON [CRM].[Communication]
				(
					[GroupKey] ASC,
					[AddressKey] ASC,
					[ContactKey] ASC,
					[IsActive] ASC
				)
				INCLUDE ( 	[CommunicationId],
					[LegacyId],
					[Data],
					[TypeKey],
					[Comment],
					[CreateDate],
					[ModifyDate],
					[CreateUser],
					[ModifyUser]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
			");
		}
	}
}
