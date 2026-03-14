namespace Main.Rest.Model
{
	public class LookupType
	{
		public string Name { get; set; }
		public string FullName { get; set; }
		public bool IsEditable { get; set; }
		public LookupProperty[] LookupProperties { get; set; }
	}
}
