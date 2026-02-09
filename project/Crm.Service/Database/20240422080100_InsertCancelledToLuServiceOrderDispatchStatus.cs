namespace Crm.Service.Database
{
	using System.Globalization;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Service.Model.Lookup;

	[Migration(20240422080100)]
	public class InsertCancelledToLuServiceOrderDispatchStatus : Migration
	{
		public override void Up()
		{
			InsertLookupValue(
				ServiceOrderDispatchStatus.CancelledNotCompleteKey,
				"Abgebrochen (Folgeeinsatz erforderlich)",
				"de",
				false,
				9);
			InsertLookupValue(
				ServiceOrderDispatchStatus.CancelledKey,
				"Abgebrochen",
				"de",
				false,
				10);
			InsertLookupValue(
				ServiceOrderDispatchStatus.CancelledNotCompleteKey,
				"Cancelled (Follow-up dispatch required)",
				"en",
				false,
				9);
			InsertLookupValue(
				ServiceOrderDispatchStatus.CancelledKey,
				ServiceOrderDispatchStatus.CancelledKey,
				"en",
				false,
				10);
			InsertLookupValue(
				ServiceOrderDispatchStatus.CancelledNotCompleteKey,
				"Törölt (Intervention de suivi nécessaire)",
				"hu",
				false,
				9);
			InsertLookupValue(
				ServiceOrderDispatchStatus.CancelledKey,
				"Törölt",
				"hu",
				false,
				10);
			InsertLookupValue(
				ServiceOrderDispatchStatus.CancelledNotCompleteKey,
				"Cancelado (se requiere una orden de seguimiento)",
				"es",
				false,
				9);
			InsertLookupValue(
				ServiceOrderDispatchStatus.CancelledKey,
				"Cancelado",
				"es",
				false,
				10);
		}
		private void InsertLookupValue(string value, string name, string language, bool favorite = false, int sortOrder = 0)
		{
			var tableName = "[SMS].[ServiceOrderDispatchStatus]";
			var favoriteString = favorite ? "1" : "0";
			var sortOrderString = sortOrder.ToString(CultureInfo.InvariantCulture);
			Database.InsertIfNotExists(tableName,
				new[] { "Value", "Name", "Language", "Favorite", "SortOrder" },
				new[] { value, name, language, favoriteString, sortOrderString });
		}
	}
}
