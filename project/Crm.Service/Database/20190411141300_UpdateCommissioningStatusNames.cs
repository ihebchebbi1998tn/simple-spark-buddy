namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190411141300)]
	public class UpdateCommissioningStatusNames : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[SMS].[CommissioningStatus]", new Column("Name", DbType.String, ColumnProperty.NotNull));
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Keine Kommissionierung', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'NoCommissioning' AND [Language] = 'de'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Kommissionierung steht aus', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'ToBeCommissioned' AND [Language] = 'de'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Teilweise kommissioniert', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'PartlyCommissioned' AND [Language] = 'de'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Kommissioniert', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'Commissioned' AND [Language] = 'de'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'No picking', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'NoCommissioning' AND [Language] = 'en'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'To be picked', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'ToBeCommissioned' AND [Language] = 'en'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Partly picked', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'PartlyCommissioned' AND [Language] = 'en'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Picked', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'Commissioned' AND [Language] = 'en'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Pas de prélèvement', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'NoCommissioning' AND [Language] = 'fr'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'En attente du prélèvement', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'ToBeCommissioned' AND [Language] = 'fr'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Prélevé en partie', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'PartlyCommissioned' AND [Language] = 'fr'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Prélevé', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'Commissioned' AND [Language] = 'fr'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Nincs megbízatás', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'NoCommissioning' AND [Language] = 'hu'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Kommissiózandó', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'ToBeCommissioned' AND [Language] = 'hu'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Részben összekészített', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'PartlyCommissioned' AND [Language] = 'hu'");
			Database.ExecuteNonQuery("UPDATE [SMS].[CommissioningStatus] SET [Name] = 'Meghatalmazott', [ModifyDate] = GETUTCDATE() WHERE [Name] = 'Commissioned' AND [Language] = 'hu'");
		}
	}
}