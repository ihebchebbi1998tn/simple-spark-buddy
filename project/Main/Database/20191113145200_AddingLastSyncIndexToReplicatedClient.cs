namespace Main.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191113145200)]
	public class AddingLastSyncIndexToReplicatedClient : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX IX_ReplicatedClient_LastSync ON [dbo].[ReplicatedClient] ([LastSync] ASC)");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}
