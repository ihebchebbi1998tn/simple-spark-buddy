namespace Main.Flow.EventHandler
{
    using Crm.Library.Helper;
    using Crm.Library.Modularization.Events;

    using Main.Flow.BackgroundServices;
    using Main.Flow.Model;

    using Quartz;

    public class FlowItemSavedEventHandler : IEventHandler<EntityCreatedEvent<FlowItem>>, ISeparateTransactionEventHandler
    {
        private readonly IScheduler scheduler;
        private readonly IAppSettingsProvider appSettingsProvider;
        public FlowItemSavedEventHandler(IScheduler scheduler, IAppSettingsProvider appSettingsProvider)
        {
            this.scheduler = scheduler;
            this.appSettingsProvider = appSettingsProvider;
        }
        public virtual void Handle(EntityCreatedEvent<FlowItem> e)
        {
            FlowProcessingService.Trigger(scheduler);
        }
    }
}
