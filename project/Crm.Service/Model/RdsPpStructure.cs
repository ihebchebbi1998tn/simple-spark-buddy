namespace Crm.Service.Model
{
	using System;

	using Crm.Library.BaseModel;

	public class RdsPpStructure : EntityBase<Guid>
	{
		public virtual Guid? ParentRdsPpStructureKey { get; set; }
		public virtual string RdsPpClassification { get; set; }
		public virtual string Description { get; set; }
	}
}