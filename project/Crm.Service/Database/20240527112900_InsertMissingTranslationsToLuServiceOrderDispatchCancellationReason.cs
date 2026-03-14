namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Service.Model.Lookup;

	[Migration(20240527112900)]
	public class InsertMissingTranslationsToLuServiceOrderDispatchCancellationReason : Migration
	{
		public override void Up()
		{
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Customer,
				"Le client n'est pas accessible",
				"fr",
				"0",
				"0"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.ContactPerson,
				"Personne de contact non accessible",
				"fr",
				"0",
				"1"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.BadWeather,
				"Mauvais temps",
				"fr",
				"0",
				"2"
			);
			InsertLookupValue(
				ServiceOrderDispatchCancellationReason.Other,
				"Autres",
				"fr",
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
				name.Replace("'", "''"),
				language,
				favorite,
				sortOrder,
				"Setup",
				"Setup"));
		}
	}
}
