namespace Main.Rest.Model
{
	using Crm.Library.Rest;

	using Main.Model;

	[RestTypeFor(DomainType = typeof(Log))]
	public class LogRest : RestEntity
	{
		public int Id { get; set; }
		public virtual string Level { get; set; }
		public virtual string Logger { get; set; }
		public virtual string Thread { get; set; }
		public virtual string Context { get; set; }
		public virtual string Message { get; set; }
		public virtual string Exception { get; set; }
	}
}
