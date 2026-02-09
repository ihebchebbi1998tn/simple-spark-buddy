namespace Crm.Service.Model.Lookup {

	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[CommissioningStatus]", "CommissioningStatusId")]
	public class CommissioningStatus : EntityLookup<string>
	{
		// Members
		public static readonly string NoCommissioningKey = "0";
		public static readonly string ToBeCommissionedKey = "1";
		public static readonly string PartlyCommissionedKey = "2";
		public static readonly string CommissionedKey = "3";
	}
}