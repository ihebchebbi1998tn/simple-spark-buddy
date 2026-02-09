namespace Main.Model
{
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class Log : EntityBase<int>, ISoftDelete
	{
		public virtual string Level { get; set; }
		public virtual string Logger { get; set; }
		public virtual string Thread { get; set; }
		public virtual string Context { get; set; }
		public virtual string Message { get; set; }
		public virtual string Exception { get; set; }
	}
}
