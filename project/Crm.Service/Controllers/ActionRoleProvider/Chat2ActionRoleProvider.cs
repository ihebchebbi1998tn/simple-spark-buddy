namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service;
	using Main.VideoCall;

	public class Chat2ActionRoleProvider : RoleCollectorBase
	{
		public Chat2ActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(VideoCallPlugin.PermissionGroup.Chat, PermissionName.Index, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.ServiceBackOffice);
			Add(VideoCallPlugin.PermissionGroup.Chat, VideoCallPlugin.PermissionName.Call, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.ServiceBackOffice);
			Add(VideoCallPlugin.PermissionGroup.Chat, VideoCallPlugin.PermissionName.Draw, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}