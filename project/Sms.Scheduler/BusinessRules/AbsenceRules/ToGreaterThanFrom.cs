using Crm.Library.Validation.BaseRules;
using Crm.Library.Validation;
using Sms.Scheduler.Model;

namespace Sms.Scheduler.BusinessRules.AbsenceRules
{
	public class ToGreaterThanFrom : OrderRule<Absence>
	{
		public ToGreaterThanFrom()
		{
			Init(t => t.To, t => t.From, ValueOrder.FirstValueGreater);
		}
	}
}
