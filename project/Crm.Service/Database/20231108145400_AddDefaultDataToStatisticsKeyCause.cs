namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model.Lookup;

	[Migration(20231108145400)]
	public class AddDefaultDataToStatisticsKeyCause : Migration
	{
		public override void Up()
		{
			Insert("Bedien", "falsche Bedienung", "incorrect  operation", "opération incorrecte", "funcionamiento incorrecto", "helytelen működés", "01");
			Insert("Best", "falsche Bestellung", "incorrect order", "ordre incorrect", "orden incorrecto", "helytelen sorrend", "02");
			Insert("Inst", "falsche Installation", "incorrect installation", "installation incorrecte", "instalación incorrecta", "helytelen telepítés", "03");
			Insert("Komm", "Kommunikationsfehler", "communication error", "erreur de communication", "error de comunicación", "kommunikációs hiba", "04");
			Insert("Konst", "Konstruktionsfehler", "construction error", "défaut de construction", "defectos de diseño", "ervezési hibák", "05");
			Insert("Lief", "falsche Lieferung", "incorrect delivery", "livraison incorrecte", "entrega incorrecta", "hibás szállítás", "06");
			Insert("Mat", "Materialfehler", "material defect", "défaut de matériel", "defecto material", "anyagi hibák", "07");
			Insert("Miss", "Missverständnis", "misunderstanding", "malentendu", "malentendido", "félreértés", "08");
			Insert("Mont", "fehlerhafte Montage (Prod.)", "incorrect assembly (Prod.)", "montage incorrect (Prod.)", "montaje incorrecto (Prod.)", "hibás összeszerelés (Prod.)", "09");
			Insert("MontAr", "fehlende Montageanleitung", "missing assembly instructions", "instructions d''assemblage manquantes", "faltan instrucciones de montaje", "hiányzó összeszerelési útmutató", "10");

			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyCause>("Lu", "StatisticsKeyCause");
		}

		private void Insert(string value, string nameDe, string nameEn, string nameFr, string nameEs, string nameHu, string code, bool favorite = false, int sortOrder = 1000)
		{
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyCause (Value, Name, Language,Code, Favorite, SortOrder, ErrorTypes) VALUES ('{value}', '{nameDe}', 'de', {code}, {(favorite ? "1" : "0")}, {sortOrder}, '')");
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyCause (Value, Name, Language,Code, Favorite, SortOrder, ErrorTypes) VALUES ('{value}', '{nameEn}', 'en', {code}, {(favorite ? "1" : "0")}, {sortOrder}, '')");
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyCause (Value, Name, Language,Code, Favorite, SortOrder, ErrorTypes) VALUES ('{value}', '{nameFr}', 'fr', {code}, {(favorite ? "1" : "0")}, {sortOrder}, '')");
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyCause (Value, Name, Language,Code, Favorite, SortOrder, ErrorTypes) VALUES ('{value}', '{nameEs}', 'es', {code}, {(favorite ? "1" : "0")}, {sortOrder}, '')");
			Database.ExecuteNonQuery($"INSERT INTO LU.StatisticsKeyCause (Value, Name, Language,Code, Favorite, SortOrder, ErrorTypes) VALUES ('{value}', '{nameHu}', 'hu', {code}, {(favorite ? "1" : "0")}, {sortOrder}, '')");
		}
	}
}
