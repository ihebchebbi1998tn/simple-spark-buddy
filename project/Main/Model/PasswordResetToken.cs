namespace Main.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class PasswordResetToken : EntityBase<Guid>, INoAuthorisedObject
	{
		public virtual DateTime? ExpiryDate { get; set; }
		public virtual string Username { get; set; }
	}
}
