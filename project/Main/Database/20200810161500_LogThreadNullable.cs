namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200810161500)]
	public class LogThreadNullable : Migration
	{
		public override void Up()
		{
			var nullable = (bool)Database.ExecuteScalar(@"
				SELECT c.is_nullable
				FROM sys.columns c
				WHERE object_id = object_id('[CRM].[Log]') AND c.[name] = 'Thread'");
			if (nullable == false)
			{
				Database.ChangeColumn("[CRM].[Log]", new Column("Thread", DbType.String, 512, ColumnProperty.Null));
			}
		}
	}
}
