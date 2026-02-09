using Crm.Library.Model.Authorization;
using Crm.Library.Modularization.Menu;
using Main.Flow.Model;

namespace Main.Flow
{
	public class FlowMenuRegistrar : IMenuRegistrar<MaterialMainMenu>
	{
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			menuProvider.Register("Administration", "FlowProcessing", url: "~/Main.Flow/FlowItemList/IndexTemplate", priority: 1600);
			menuProvider.AddPermission("Administration", "FlowProcessing", PermissionGroup.WebApiRead, nameof(FlowItem));
			menuProvider.Register("Administration", "FlowRule", url: "~/Main.Flow/FlowRuleList/IndexTemplate", priority: 1500);
			menuProvider.AddPermission("Administration", "FlowRule", PermissionGroup.WebApiRead, nameof(FlowRule));
		}
	}
}
