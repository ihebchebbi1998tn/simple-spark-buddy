namespace Main.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;
	using Crm.Library.Validation.Interfaces;

	using Main.Data.NHibernateProvider;
	using Main.Services.Interfaces;

	using NHibernate;
	using NHibernate.Cfg;
	using NHibernate.Mapping;
	using NHibernate.Metadata;
	using NHibernate.Proxy;

	public class DatabaseColumnLengthValidator : IBusinessRuleValidator
	{
		private readonly IInformationSchemaCache informationSchemaCache;
		private readonly IDictionary<string, IClassMetadata> metaData;
		private readonly Configuration configuration;

		public virtual List<Rule> GetRules(Type entityType)
		{
			var rules = GetMaxLengthRules(entityType);
			return rules.Values.Select(x => (Rule)x).ToList();
		}
		public virtual List<RuleViolation> GetRuleViolations(object entity)
		{
			var entityType = NHibernateProxyHelper.GetClassWithoutInitializingProxy(entity);
			var rules = GetMaxLengthRules(entityType);
			return GetBusinessRuleViolations(entity, rules);
		}

		protected virtual List<RuleViolation> GetBusinessRuleViolations(object entity, Dictionary<string, IMaxLengthRule> rules)
		{
			var ruleViolations = new List<RuleViolation>();
			foreach (KeyValuePair<string, IMaxLengthRule> rule in rules)
			{
				var propertyInfo = entity.GetType().GetProperty(rule.Key);
				var propertyValue = propertyInfo.GetValue(entity);
				if (propertyValue != null && propertyValue.ToString().Length > rule.Value.MaxLength)
				{
					ruleViolations.Add(new RuleViolation(entity, rule.Key, null, RuleClass.MaxLength));
				}
			}
			return ruleViolations;
		}

		protected virtual Dictionary<string, IMaxLengthRule> GetMaxLengthRules(Type entityType)
		{
			var rules = new Dictionary<string, IMaxLengthRule>();
			var entityTypeName = entityType.FullName;
			if (metaData.ContainsKey(entityTypeName))
			{
				var classMetadata = metaData[entityTypeName];
				var classMapping = configuration.GetClassMapping(entityTypeName);
				foreach (string propertyName in classMetadata.PropertyNames)
				{
					var property = classMapping.GetProperty(propertyName);
					var column = property.ColumnIterator.FirstOrDefault() as Column;
					if (!property.Type.IsComponentType && column != null)
					{
						var informationSchema = informationSchemaCache.GetAll().FirstOrDefault(x =>
							x.TableSchema == column.Value.Table.Schema
							&& x.TableName == column.Value.Table.Name
							&& x.ColumnName == column.Name);
						if (informationSchema != null && informationSchema.CharacterMaximumLength.HasValue && informationSchema.CharacterMaximumLength.Value > 0)
						{
							var rule = new MaxLengthRule();
							rule.Init(entityType, propertyName, informationSchema.CharacterMaximumLength.Value);
							rules.Add(propertyName, rule);
						}
					}
				}
			}
			return rules;
		}

		public virtual bool IsValid(object entity)
		{
			return GetRuleViolations(entity).Any();
		}

		public DatabaseColumnLengthValidator(IInformationSchemaCache informationSchemaCache, ISessionFactory sessionFactory, INHibernateInitializer nhibernateInitializer)
		{
			this.informationSchemaCache = informationSchemaCache;
			metaData = sessionFactory.GetAllClassMetadata();
			configuration = nhibernateInitializer.Configuration;
		}
	}
}
