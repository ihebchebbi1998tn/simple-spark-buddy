using System;
namespace Crm.Service.Model
{
	using Crm.Library.BaseModel;

	public class InstallationPosSerial : EntityBase<Guid>
	{
		public virtual Guid InstallationPosId { get; set; }
		public virtual string SerialNo { get; set; }
		public virtual bool IsInstalled { get; set; }
	}
}