namespace Main.ViewModels
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.EntityConfiguration;

	public class GenericListViewModel : CrmModel
	{
		public string IdentifierPropertyName { get; set; }
		public string Title { get; set; }
		public string TypeName { get; set; }
		public string ControllerName { get { return String.Format("{0}List", String.IsNullOrEmpty(TypeNameOverride) ? Type.Name : TypeNameOverride); } }
		public string PluginName { get; set; }
		public IEnumerable<SortDefinition> OrderByProperties { get; set; } = new List<SortDefinition>();
		public IEnumerable<FilterDefinition> FilterProperties { get; set; } = new List<FilterDefinition>();
		public string MapTileLayerUrl { get; set; }
		public string EmptySlate { get; set; }
		public Type Type { get; set; }
		public string TypeNameOverride { get; set; }
		public bool IsCsvExportable { get; set; }
		public bool IsRssSubscribable { get; set; }
		public string FilterRoute { get; set; }
	}
}
