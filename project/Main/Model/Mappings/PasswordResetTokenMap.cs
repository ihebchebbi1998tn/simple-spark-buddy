namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using LMobile.Unicore.NHibernate;

	public class PasswordResetTokenMap : EntityClassMapping<PasswordResetToken>
	{
		public PasswordResetTokenMap()
		{
			Schema("dbo");
			Table("PasswordResetToken");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("Id");
					map.Generator(GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.ExpiryDate);
			Property(x => x.Username);
		}
	}
}
