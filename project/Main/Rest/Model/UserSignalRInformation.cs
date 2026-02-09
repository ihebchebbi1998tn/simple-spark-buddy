namespace Main.Rest.Model;

using Crm.Library.Signalr;

public class UserSignalRInformation
{
	public bool Connected { get; set; }
	public string Id { get; set; }
	public JavaScriptLogLevel JavaScriptLogLevel { get; set; }
	public string[] LocalDatabaseLogs { get; set; }
	public string[] LocalStorageLogs { get; set; }
}
