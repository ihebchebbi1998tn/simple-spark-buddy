namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160404161200)]
	public class AddIndexToCrmNotes : Migration
	{
		public override void Up()
		{
			if (!Database.IndexExists("[CRM].[Note]", "IX_Note_ElementKey"))
			{
				Database.ExecuteNonQuery(
					@"CREATE NONCLUSTERED INDEX [IX_Note_ElementKey] ON [CRM].[Note]
						(
							[ElementKey] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)");
			}
		}
	}
}
