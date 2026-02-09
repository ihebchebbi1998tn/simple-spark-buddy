namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240828101000)]
	public class AddMinPasswordStrengthColumnToDomain : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("MinPasswordStrength", DbType.Int16, ColumnProperty.NotNull, 2));
		}
	}
}
