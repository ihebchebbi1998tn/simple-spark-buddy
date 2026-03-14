namespace Main
{
	using System.Linq;

	using Autofac;
	using Autofac.Core;

	using Crm.Library.Environment.Logging;

	using log4net;

	public class LoggingModule : Module
	{
		private void OnComponentPreparing(object sender, PreparingEventArgs e)
		{
			e.Parameters = e.Parameters.Union(
				new[]
				{
					new ResolvedParameter(
						(p, i) => p.ParameterType == typeof(ILog),
						(p, i) => i.Resolve<ILogManager>().GetLog(p.Member.DeclaringType)
					)
				});
		}

		protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
		{
			registration.Preparing += OnComponentPreparing;
		}
	}
}
