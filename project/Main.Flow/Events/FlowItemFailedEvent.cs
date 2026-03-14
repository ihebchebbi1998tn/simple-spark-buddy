namespace Main.Flow.Events
{
	using System;

	using Crm.Library.Modularization.Events;

	using Main.Flow.Model;

	public class FlowItemFailedEvent : IEvent
	{
		public FlowItem FlowItem { get; set; }
		public Exception Exception { get; set; }
		public FlowItemFailedEvent(FlowItem flowItem, Exception exception)
		{
			FlowItem = flowItem;
			Exception = exception;
		}
	}
}