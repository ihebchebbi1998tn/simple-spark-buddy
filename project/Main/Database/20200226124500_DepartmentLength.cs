namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200226124500)]
	public class DepartmentLength : Migration
	{
		public override void Up()
		{
			var departmentLength = (int)Database.ExecuteScalar(
				@"	SELECT CHARACTER_MAXIMUM_LENGTH
						FROM INFORMATION_SCHEMA.COLUMNS
						WHERE TABLE_SCHEMA = 'CRM' AND
							TABLE_NAME = 'Person' AND
							COLUMN_NAME = 'Department'");
			if (departmentLength < 256)
			{
				Database.ExecuteNonQuery($"ALTER TABLE [CRM].[Person] ALTER COLUMN [Department] NVARCHAR(256) NULL");
			}
		}
	}
}
