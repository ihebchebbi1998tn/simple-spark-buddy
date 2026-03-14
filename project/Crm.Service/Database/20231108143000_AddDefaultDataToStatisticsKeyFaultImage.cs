namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model.Lookup;

	[Migration(20231108143000)]
	public class AddDefaultDataToStatisticsKeyFaultImage : Migration
	{
		public override void Up()
		{
			Insert("Bedien", "Bedienfehler", "operating error", "erreur de manipulation", "error de funcionamiento", "működési hiba", "01");
			Insert("Best", "Bestellfehler", "order error", "erreur d''ordre", "error de pedido", "rendelési hiba", "02");
			Insert("Folge", "Folgeschaden", "consequential damage", "dommage consécutif", "daños consecuenciales", "következményes károk", "03");
			Insert("Inst", "Installationsfehler", "installation error", "erreur d''installation", "error de instalación", "telepítési hiba", "04");
			Insert("Konst", "Konstruktionsfehler", "construction error", "défaut de construction", "defectos de diseño", "ervezési hibák", "05");
			Insert("Mat", "Materialfehler", "material defect", "défaut de matériel", "defecto material", "anyagi hibák", "06");
			Insert("Menge", "Fehlmengen", "shortfall quantities", "quantités manquantes", "cantidades deficitarias", "hiányos mennyiségek", "07");
			Insert("Mont", "Montagefehler", "assembly error", "erreur d''assemblage", "error de montaje", "összeszerelési hiba", "08");
			Insert("Preis", "Preisabweichung", "price variance", "écart de prix", "variación de precios", "árváltozások", "09");
			Insert("Produk", "Produktmängel", "product defects", "défauts du produit", "defectos del producto", "termékhibák", "10");

			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyFaultImage>("Lu", "StatisticsKeyFaultImage");
		}

		private void Insert(string value, string nameDe, string nameEn, string nameFr, string nameEs, string nameHu, string code, bool favorite = false, int sortOrder = 1000)
		{
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyFaultImage (Value, Name, Language,Code, Favorite, SortOrder) VALUES ('{value}', '{nameDe}', 'de', {code}, {(favorite ? "1" : "0")}, {sortOrder})");
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyFaultImage (Value, Name, Language,Code, Favorite, SortOrder) VALUES ('{value}', '{nameEn}', 'en', {code}, {(favorite ? "1" : "0")}, {sortOrder})");
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyFaultImage (Value, Name, Language,Code, Favorite, SortOrder) VALUES ('{value}', '{nameFr}', 'fr', {code}, {(favorite ? "1" : "0")}, {sortOrder})");
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyFaultImage (Value, Name, Language,Code, Favorite, SortOrder) VALUES ('{value}', '{nameEs}', 'es', {code}, {(favorite ? "1" : "0")}, {sortOrder})");
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyFaultImage (Value, Name, Language,Code, Favorite, SortOrder) VALUES ('{value}', '{nameHu}', 'hu', {code}, {(favorite ? "1" : "0")}, {sortOrder})");
		}
	}
}
