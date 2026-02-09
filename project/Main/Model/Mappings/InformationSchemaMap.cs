namespace Main.Model.Mappings
{
	using Crm.Library.Model;

	using NHibernate.Mapping.ByCode.Conformist;

	public class InformationSchemaMap : ClassMapping<InformationSchema>
	{
		public InformationSchemaMap()
		{
			Schema("INFORMATION_SCHEMA");
			Table("COLUMNS");

			ComposedId(map =>
			{
				map.Property(x => x.TableSchema, m => m.Column("TABLE_SCHEMA"));
				map.Property(x => x.TableName, m => m.Column("TABLE_NAME"));
				map.Property(x => x.ColumnName, m => m.Column("COLUMN_NAME"));
			});

			Property(x => x.TableSchema, m => m.Column("TABLE_SCHEMA"));
			Property(x => x.TableName, m => m.Column("TABLE_NAME"));
			Property(x => x.ColumnName, m => m.Column("COLUMN_NAME"));
			Property(x => x.CharacterMaximumLength, m => m.Column("CHARACTER_MAXIMUM_LENGTH"));
			Property(x => x.IsNullableString, m => m.Column("IS_NULLABLE"));
		}
	}
}
