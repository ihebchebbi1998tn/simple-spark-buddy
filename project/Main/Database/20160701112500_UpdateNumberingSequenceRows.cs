namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160701112500)]
	public class UpdateNumberingSequenceRows : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NumberingSequence]')") == 1)
			{
				Database.ExecuteNonQuery(@"UPDATE [dbo].[NumberingSequence] SET SequenceName = 'SMS.ServiceOrderHead:MaintenanceOrder'	WHERE SequenceName = 'SMS.MaintenanceOrder'
																	 UPDATE[dbo].[NumberingSequence] SET SequenceName = 'SMS.ServiceOrderHead:ServiceOrder' WHERE SequenceName = 'SMS.ServiceOrder'");

				Database.AddColumnIfNotExisting("[dbo].[NumberingSequence]", new Column("NextLow", System.Data.DbType.Int64, ColumnProperty.Null, 0));
				Database.AddColumnIfNotExisting("[dbo].[NumberingSequence]", new Column("MaxLow", System.Data.DbType.Int64, ColumnProperty.Null, 0));

				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[NumberingSequence] WHERE SequenceName = 'SMS.ServiceOrderHead:AdHoc'") == 0
					&& (int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[hibernate_unique_key_old] WHERE [tablename] = '[SMS].[ServiceOrderDispatch]'") == 1)
				{
					Database.ExecuteNonQuery(@"INSERT INTO [dbo].[NumberingSequence] ([SequenceName], [LastNumber], [Prefix], [Format], [Suffix], [NextLow], [MaxLow], [TenantKey])	
						VALUES('SMS.ServiceOrderHead:AdHoc', 
										(SELECT [next_hi] FROM [dbo].[hibernate_unique_key_old] WHERE [tablename] = '[SMS].[ServiceOrderDispatch]'),
										'AdHoc-', 
										'000000',
										null, 
										0, 
										32, 
										null)");
				}
			}
		}
	}
}