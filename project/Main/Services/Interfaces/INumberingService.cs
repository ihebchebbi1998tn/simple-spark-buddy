namespace Main.Services.Interfaces
{
	using Crm.Library.AutoFac;

	public interface INumberingService : IDependency
	{
		string GetNextFormattedNumber(string sequenceName);
		long? GetNextHighValue(string sequenceName);
		bool NumberingSequenceExists(string sequenceName);
	}
}
