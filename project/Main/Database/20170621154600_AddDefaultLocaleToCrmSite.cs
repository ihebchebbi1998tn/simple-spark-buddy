namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170621154600)]
	public class AddDefaultLocaleToCrmSite : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Site]", "DefaultLocale"))
			{
				Database.AddColumn("[CRM].[Site]", new Column("DefaultLocale", DbType.String, 20, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE [CRM].[Site] SET [DefaultLocale] = [DefaultLanguage]");
				Database.ExecuteNonQuery("UPDATE [CRM].[Site] SET [DefaultLocale] = 'de' WHERE [DefaultLocale] = 'de-DE'");
				Database.ExecuteNonQuery("UPDATE [CRM].[Site] SET [DefaultLocale] = 'en-US-POSIX' WHERE [DefaultLocale] = 'en-US'");
				Database.ExecuteNonQuery("UPDATE [CRM].[Site] SET [DefaultLanguage] = CASE WHEN [DefaultLocale] = 'de' THEN 'de' ELSE 'en' END");
				Database.ChangeColumn("[CRM].[Site]", new Column("DefaultLocale", DbType.String, 20, ColumnProperty.NotNull));
			}
		}
	}
}