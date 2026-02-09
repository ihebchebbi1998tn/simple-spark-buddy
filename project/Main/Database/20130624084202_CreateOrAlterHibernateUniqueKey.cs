namespace Main.Database
{
	using System.Data;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130624084202)]
	public class CreateOrAlterHibernateUniqueKey : Migration
	{
		public override void Up()
		{
			var sb = new StringBuilder();
			if (!Database.TableExists("hibernate_unique_key"))
			{
				sb.AppendLine("CREATE TABLE hibernate_unique_key (");
				sb.AppendLine("next_hi int NOT NULL,");
				sb.AppendLine("tablename nvarchar(50) NOT NULL,");
				sb.AppendLine("CONSTRAINT [PK_Hibernate] PRIMARY KEY CLUSTERED ");
				sb.AppendLine("(");
				sb.AppendLine("[tablename] ASC");
				sb.AppendLine(")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
				sb.AppendLine(") ON [PRIMARY]");
			}
			else
			{
				Database.RemovePrimaryKey("hibernate_unique_key");
				
				Database.ChangeColumn("[dbo].[hibernate_unique_key]", new Column("tablename", DbType.String, 50, ColumnProperty.NotNull));

				sb.AppendLine("ALTER TABLE [dbo].[hibernate_unique_key] ADD  CONSTRAINT [PK_Hibernate] PRIMARY KEY CLUSTERED ");
				sb.AppendLine("(");
				sb.AppendLine("[tablename] ASC");
				sb.AppendLine(")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
			}

			Database.ExecuteNonQuery(sb.ToString());
		}
		public override void Down()
		{
		}
	}
}