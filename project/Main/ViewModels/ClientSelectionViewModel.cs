namespace Main.ViewModels
{
	using System.Collections.Generic;

	using Main.Services;

	public class ClientSelectionViewModel : CrmModel
	{
		public List<RedirectProviderResult> RedirectProviderResults { get; set; }
	}
}
