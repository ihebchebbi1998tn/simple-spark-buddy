namespace Main.Flow.BusinessRules.FlowRule
{
    using Crm.Library.Validation.BaseRules;

    using Main.Flow.Model;

    public class EntityTypeRequired : RequiredRule<FlowRule>
    {
        public EntityTypeRequired()
        {
            Init(x => x.EntityType);
        }
    }
}