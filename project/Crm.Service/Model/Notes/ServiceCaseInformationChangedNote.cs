namespace Crm.Service.Model.Notes
{
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Extensions;
	using Crm.Model.Notes;

	public class ServiceCaseInformationChangedNote : Note
	{
		public override string ImageTextKey
		{
			get { return "ServiceCaseInformation"; }
		}
		public override string PermanentLabelResourceKey
		{
			get { return "ServiceCaseInformationChangedBy"; }
		}
		public ServiceCaseInformationChangedNote()
		{
			Plugin = "Crm.Service";
		}
	}
}
