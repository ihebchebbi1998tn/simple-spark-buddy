namespace Main.Flow.BusinessRules.FlowRule
{
    using Crm.Library.Validation.BaseRules;

    using Main.Flow.Model;

    public class EndpointRequired : RequiredRule<FlowRule>
    {
        public EndpointRequired()
        {
            Init(x => x.Endpoint);
        }
    }
}