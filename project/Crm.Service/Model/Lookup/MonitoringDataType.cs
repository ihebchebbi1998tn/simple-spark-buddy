namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[MonitoringDataType]")]
	public class MonitoringDataType : EntityLookup<string>
	{
        public virtual int MonitoringDataTypeId { get; set; }
		public virtual string DataTypeKey { get; set; }
		public virtual double? Min { get; set; }
		public virtual double? Max { get; set; }
		public virtual int? BitIndex { get; set; }
		public virtual string QuantityUnit { get; set; }
	}
}
