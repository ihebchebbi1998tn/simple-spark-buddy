namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Data.NHibernateProvider.UserTypes;

	using Main.Model.Enums;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Type;

	public class MessageMap : EntityClassMapping<Message>
	{
		public MessageMap()
		{
			Schema("CRM");
			Table("Message");

			Id(a => a.Id, m =>
			{
				m.Column("MessageId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.From, m => m.Column("`From`"));
			Property(x => x.Recipients, map => map.Type<DelimitedStringUserType>());
			Property(x => x.Bcc, map => map.Type<DelimitedStringUserType>());
			Property(x => x.Subject);
			Property(x => x.Body, m => m.Length(Int32.MaxValue));
			Property(x => x.IsBodyHtml);
			Property(x => x.State, m => m.Type<EnumStringType<MessageState>>());
			Property(x => x.LanguageKey);
			Set(x => x.AttachmentIds, map =>
			{
				map.Schema("CRM");
				map.Table("MessageAttachment");
				map.Key(km => km.Column("MessageKey"));
				map.Fetch(CollectionFetchMode.Select);
				map.BatchSize(100);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Persist);
				map.BatchSize(100);
			}, r => r.Element(m => m.Column("FileResourceKey")));
			Property(x => x.ErrorMessage, m => m.Length(Int32.MaxValue));
		}
	}
}
