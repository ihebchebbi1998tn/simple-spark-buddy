namespace Main.Rest.Model
{
	public class PermissionGroupPermission
	{
		public string PermissionName { get; set; }
		public string[] ImportedBy { get; set; }
		public string[] ImportedPermissions { get; set; }
	}
}
