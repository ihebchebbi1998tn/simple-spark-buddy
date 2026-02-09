namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20250627135000)]
	public class FixHungarianLookups : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[UserStatus]"))
			{
				AddOrUpdate("[LU].[UserStatus]",
					"Available",
					"Elérhető");
				AddOrUpdate("[LU].[UserStatus]",
					"Away",
					"Távol");
				AddOrUpdate("[LU].[UserStatus]",
					"DnD",
					"Ne zavarj");
			}

			if (Database.TableExists("[LU].[PaymentCondition]"))
			{
				AddOrUpdate("[LU].[PaymentCondition]",
					"1",
					"Kedvezmény nélkül, átvételkor fizetendő");
			}

			if (Database.TableExists("[LU].[PaymentInterval]"))
			{
				AddOrUpdate("[LU].[PaymentInterval]",
					"1",
					"Azonnal");
				AddOrUpdate("[LU].[PaymentInterval]",
					"2",
					"Negyedévente");
			}

			if (Database.TableExists("[LU].[PaymentType]"))
			{
				AddOrUpdate("[LU].[PaymentType]",
					"1",
					"Számla");
				AddOrUpdate("[LU].[PaymentType]",
					"2",
					"Terhelés");
			}

			if (Database.TableExists("[LU].[InvoicingType]"))
			{
				AddOrUpdate("[LU].[InvoicingType]",
					"LumpSum",
					"Átalánydíj");
				AddOrUpdate("[LU].[InvoicingType]",
					"TMbasis",
					"T&M alapú");
				AddOrUpdate("[LU].[InvoicingType]",
					"ByStatus",
					"Állapot szerint");
				AddOrUpdate("[LU].[InvoicingType]",
					"Goodwill",
					"Jóakarat");
			}
		}

		public void AddOrUpdate(string table, string lookupValue, string lookupName)
		{
			if ((int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {table} WHERE [Value] = N'{lookupValue}'") == 0)
			{
				return;
			}

			if ((int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {table} WHERE [Language] = 'hu' AND [Value] = N'{lookupValue}'") == 0)
			{
				var favorite = (bool)Database.ExecuteScalar($"SELECT [Favorite] FROM {table} WHERE [Language] = 'en' AND [Value] = N'{lookupValue}'") ? "1" : "0";
				var sortOrder = Database.ExecuteScalar($"SELECT [SortOrder] FROM {table} WHERE [Language] = 'en' AND [Value] = N'{lookupValue}'").ToString();
				string color = null;
				if (Database.ColumnExists(table, "Color"))
				{
					color = Database.ExecuteScalar($"SELECT Color FROM {table} WHERE [Language] = 'en' AND [Value] = N'{lookupValue}'").ToString();
				}

				if (color == null)
				{
					Database.ExecuteNonQuery($"INSERT INTO {table} ([Language], [Value], [Name], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES ('hu', N'{lookupValue}', N'{lookupName}', '{favorite}', '{sortOrder}', GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
				}
				else
				{
					Database.ExecuteNonQuery($"INSERT INTO {table} ([Language], [Value], [Name], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES ('hu', N'{lookupValue}', N'{lookupName}', '{favorite}', '{sortOrder}', '{color}', GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
				}
			}
			else
			{
				Database.ExecuteNonQuery($"UPDATE {table} SET [Name] = N'{lookupName}', [ModifyDate] = GETUTCDATE() WHERE [Language] = 'hu' AND [Value] = N'{lookupValue}'");
			}
		}
	}
}
