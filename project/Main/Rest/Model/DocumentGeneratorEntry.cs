namespace Main.Rest.Model
{
	using System;

	public class DocumentGeneratorEntry
	{
		public DateTime CreateDate { get; set; }
		public string CreateUser { get; set; }
		public string ErrorMessage { get; set; }
		public string GeneratorService { get; set; }
		public Guid Id { get; set; }
		public DateTime ModifyDate { get; set; }
		public string ModifyUser { get; set; }
		public string Type { get; set; }
	}
}
