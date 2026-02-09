namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(MonitoringDataType))]
	public class MonitoringDataTypeRest : RestEntityLookupWithExtensionValues
	{
		[RestrictedField]
		public int MonitoringDataTypeId { get; set; }
		[RestrictedField]
		public string DataTypeKey { get; set; }
		[RestrictedField]
		public double? Min { get; set; }
		[RestrictedField]
		public double? Max { get; set; }
		[RestrictedField]
		public int? BitIndex { get; set; }
		[RestrictedField]
		public string QuantityUnit { get; set; }
	}
}
