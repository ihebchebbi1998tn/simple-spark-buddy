namespace Main.Model
{
	using Crm.Library.Globalization.Lookup;

	public interface IStatusChoosable<TStatus> where TStatus : ILookup
	{
		TStatus Status { get; set; }
	}
}
