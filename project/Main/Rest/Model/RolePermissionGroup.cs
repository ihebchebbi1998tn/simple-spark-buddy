namespace Main.Rest.Model
{
	public class RolePermissionGroup
	{
		public string Name { get; set; }
		public PermissionGroupPermission[] Permissions { get; set; }
	}
}
