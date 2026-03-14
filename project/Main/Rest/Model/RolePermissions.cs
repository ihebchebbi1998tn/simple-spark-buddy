namespace Main.Rest.Model
{
	public class RolePermissions
	{
		public RolePermissionGroup[] PermissionGroups { get; set; }

		public string[] Permissions { get; set; }
		public string[] SourcePermissions { get; set; }
	}
}
