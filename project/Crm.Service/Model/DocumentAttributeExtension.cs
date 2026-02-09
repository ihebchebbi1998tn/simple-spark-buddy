namespace Crm.Service.Model
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Attributes;
	using Crm.Model;

	using Newtonsoft.Json;

	public class DocumentAttributeExtension : EntityExtension<DocumentAttribute>
	{
		public virtual Guid? DispatchId { get; set; }
		public virtual Guid? ServiceOrderMaterialId { get; set; }
		public virtual Guid? ServiceOrderTimeId { get; set; }
		[UI(UIignore = true), JsonIgnore, NotMapped]
		public virtual string ServiceOrderTimePosNo { get; set; }
		[UI(UIignore = true), JsonIgnore, NotMapped]
		public virtual ServiceOrderTime ServiceOrderTime { get; set; }
		[UI(UIignore = true), JsonIgnore, NotMapped]
		public virtual ServiceOrderDispatch ServiceOrderDispatch { get; set; }
	}
}
