namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Model;

	using NHibernate;
	using NHibernate.Mapping.ByCode;
	using NHibernate.Type;

	public class PostingBaseMap : EntityClassMapping<PostingBase>
	{
		public PostingBaseMap()
		{
			Schema("CRM");
			Table("Posting");
			Discriminator(x => x.Column("Category"));
			Id(x => x.Id, m =>
				{
					m.Column("PostingId");
					m.Generator(Generators.Identity);
				});
			Property(x => x.EntityId);
			Property(x => x.PostingState, m =>
				{
					m.Column("State");
					m.Type<EnumStringType<PostingState>>();
				});
			Property(x => x.StateDetails, m => m.Type(NHibernateUtil.StringClob));
			Property(x => x.PostingType, m =>
				{
					m.Column("Type");
					m.Type<EnumStringType<PostingType>>();
				});
			Property(x => x.EntityTypeName);
			Property(x => x.SerializedEntity, m => m.Length(Int32.MaxValue));
			Property(x => x.TransactionId);
			Property(x => x.RetryAfter);
			Property(x => x.Retries);
			Property(x => x.Category, m =>
			{
				m.Type<EnumStringType<PostingCategory>>();
				m.Update(false);
				m.Insert(false);
			});
		}
	}
}
