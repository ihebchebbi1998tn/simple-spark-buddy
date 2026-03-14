namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Service.Model.Lookup;

	[Migration(20240422150000)]
	public class AddDefaultValuesToSmsServiceOrderDispatchCancellationReason : Migration
	{
		public override void Up()
		{
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Customer,
				"Customer not accessible",
				"en",
				"0",
				"0"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Customer,
				"Kunde nicht erreichbar",
				"de",
				"0",
				"0"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Customer,
				"Ügyfél nem elérhető",
				"hu",
				"0",
				"0"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Customer,
				"Cliente inaccesible",
				"es",
				"0",
				"0"
			);

			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.ContactPerson,
				"Contact person not accessible",
				"en",
				"0",
				"1"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.ContactPerson,
				"Kontaktperson nicht erreichbar",
				"de",
				"0",
				"1"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.ContactPerson,
				"Kapcsolattartó személy nem elérhető",
				"hu",
				"0",
				"1"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.ContactPerson,
				"Persona de contacto inaccesible",
				"es",
				"0",
				"1"
			);

			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.BadWeather,
				"Bad weather",
				"en",
				"0",
				"2"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.BadWeather,
				"Schlechtes Wetter",
				"de",
				"0",
				"2"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.BadWeather,
				"Rossz időjárás",
				"hu",
				"0",
				"2"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.BadWeather,
				"Mal tiempo",
				"es",
				"0",
				"2"
			);

			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Other,
				"Other",
				"en",
				"0",
				"3"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Other,
				"Sonstiges",
				"de",
				"0",
				"3"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Other,
				"Egyéb",
				"hu",
				"0",
				"3"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Other,
				"Varios",
				"es",
				"0",
				"3"
			);
		}

		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.ExecuteNonQuery(String.Format("INSERT INTO [SMS].[ServiceOrderDispatchCancellationReason] " +
			                                       "([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [CreateUser], [ModifyDate], [ModifyUser]) " +
			                                       "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', GETUTCDATE(), '{5}', GETUTCDATE(), '{6}')",
				value,
				name,
				language,
				favorite,
				sortOrder,
				"Setup",
				"Setup"));
		}
	}
}
