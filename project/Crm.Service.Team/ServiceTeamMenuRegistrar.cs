namespace Crm.Service.Team
{
	using System.Runtime.CompilerServices;

	using Crm.Library.Model.Authorization;
	using Crm.Library.Modularization.Menu;

	public class ServiceTeamMenuRegistrar : IMenuRegistrar<MaterialMainMenu>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			menuProvider.Register("MasterData", "ServiceTeams", url: "~/Crm.Service.Team/ServiceTeamList/IndexTemplate", priority: 900);
			menuProvider.AddPermission("MasterData", "ServiceTeams", PermissionGroup.UserAdmin, ServiceTeamPlugin.PermissionName.ListServiceTeams);
		}
	}
}
