namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230329120200)]
	public class UpdateTimeUnitsPerYear : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("LU.TimeUnit", "TimeUnitsPerYear"))
			{
				var sb = new StringBuilder();
				sb.AppendLine("UPDATE LU.TimeUnit");
				sb.AppendLine("SET TimeUnitsPerYear = 52");
				sb.AppendLine("WHERE Value = 'Week' AND TimeUnitsPerYear IS NULL");

				Database.ExecuteNonQuery(sb.ToString());
			}
		}
	}
}
