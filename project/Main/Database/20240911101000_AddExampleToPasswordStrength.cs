namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(202420240911101000)]
	public class AddExampleToPasswordStrength : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("LU.PasswordStrength", new Column("Example", DbType.String, ColumnProperty.Null));
			Database.Update("LU.PasswordStrength", new[] { "Example" }, new[] { "1234" }, "Value = 0");
			Database.Update("LU.PasswordStrength", new[] { "Example" }, new[] { "Password99" }, "Value = 1");
			Database.Update("LU.PasswordStrength", new[] { "Example" }, new[] { "Tulip?85" }, "Value = 2");
			Database.Update("LU.PasswordStrength", new[] { "Example" }, new[] { "42Fish-Thanks" }, "Value = 3");
			Database.Update("LU.PasswordStrength", new[] { "Example" }, new[] { "F4iry-Du5t-Gl1tter-tastE" }, "Value = 4");
		}
	}
}
