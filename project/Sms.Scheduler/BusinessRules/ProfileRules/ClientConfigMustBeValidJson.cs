namespace Sms.Scheduler.BusinessRules.ProfileRules
{
	using System.Linq;
	
	using Crm.Library.Validation;

	using log4net;

	using Newtonsoft.Json;
	using NJsonSchema;
	using NJsonSchema.Validation;

	using Sms.Scheduler.Model;

	public class ClientConfigMustBeValidJson : Rule<Profile>
	{
		private static readonly JsonSchema schema = JsonSchema.FromType<ClientConfig>();
		private static readonly ILog Logger = LogManager.GetLogger(typeof(ClientConfigMustBeValidJson));
		
		public ClientConfigMustBeValidJson() : base(RuleClass.Undefined)
		{
		}

		public override bool IsSatisfiedBy(Profile entity)
		{
			//ignore for null
			if (entity.ClientConfig == null) return true;

			//deny whitespaces
			if (string.IsNullOrWhiteSpace(entity.ClientConfig))
			{
				Logger.Warn("ClientConfig must not be empty!");
				return false;
			}

			if (entity.ClientConfig.Length > 10_000)
			{
				Logger.Warn($"ClientConfig must be less than 10 000 characters!");
				return false;
			}

			try
			{
				var errors = schema.Validate(entity.ClientConfig);
				if (errors.Count > 0)
				{
					Logger.ErrorFormat($"Errors while validating ClientConfig:\n{string.Join("\n", errors.Select(validationError => validationError.ToString()))}");
					return false;
				}
				var clientConfig = JsonConvert.DeserializeObject<ClientConfig>(entity.ClientConfig);

				var hours = clientConfig.NonWorkingHours;
				if (hours != null)
				{
					for (var i = 0; i < hours.Length; i++)
					{
						var from = hours[i].From;
						if (hours[i].To <= from) return false;
						if (i > 0 && hours[i - 1].To > from) return false;
					}
				}

				if (clientConfig.ServiceOrderDispatchMaximumDuration.HasValue && clientConfig.ServiceOrderDispatchMaximumDuration != 0)
				{
					if (clientConfig.ServiceOrderDispatchMaximumDuration < 60 || clientConfig.ServiceOrderDispatchMaximumDuration > 2400)
					{
						return false;
					}
					if (clientConfig.ServiceOrderDispatchMaximumDuration.Value < clientConfig.ServiceOrderDispatchDefaultDuration)
					{
						return false;
					}
					if (clientConfig.ServiceOrderDispatchIgnoreCalculatedDuration == false && clientConfig.ServiceOrderDispatchForceMaximumDuration == false)
					{
						return false;
					}
				}

				return true;
			}
			catch
			{
				return false;
			}
		}
		protected override RuleViolation CreateRuleViolation(Profile entity) => new RuleViolation(nameof(ClientConfigMustBeValidJson));
	}
	internal static class JsonSchemaExtensions
	{
		public static bool IsValid(this JsonSchema jsonSchema, string jsonData, JsonSchemaValidatorSettings settings = null)
		{
			try
			{
				var errors = jsonSchema.Validate(jsonData, settings);
				return !errors.Any();
			}
			catch
			{
				return false;
			}
		}
	}
}
