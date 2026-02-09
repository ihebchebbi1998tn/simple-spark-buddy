namespace Crm.Service.Team
{
	using Crm.Library.Modularization;

	[Plugin(ModuleId = "FLD00060", Requires = "Crm.Article,Crm.Service")]
	public class ServiceTeamPlugin : Plugin
	{
		public static readonly string PluginName = typeof(ServiceTeamPlugin).Namespace;
		public static class PermissionName
		{
			public const string ListServiceTeams = "ListServiceTeams";
		}
	}
}
