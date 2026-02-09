namespace Main.Model.Enums
{
	using System;

	// used for Contact List UI
	[Flags]
	public enum ContactType : ushort
	{
		Person = 1,
		Company = 2,
		Article = 4,
		Folder = 8,
		Installation = 16,
		ServiceCase = 32,
		ServiceContract = 64,
		ServiceOrderHead = 128,
		ServiceObject = 256
	}

	[Flags]
	public enum MessageState
	{
		Pending = 0,
		Dispatched = 1,
		Failed = 2
	}

	public enum NoteCollection
	{
		All,
		CurrentUser
	}

	[Flags]
	public enum LogLevel
	{
		None = 0,
		Fatal = 1,
		Error = 2,
		Warn = 4,
		Debug = 8,
		Info = 16
	}

	public enum CommunicationType
	{
		Phone = 1,
		Fax,
		Email,
		Website
	}

	public enum DeleteStatus
	{
		DoDelete = 0,
		DontDelete_InforDBMissing = 1,
		DontDelete_DocsFound = 2
	}

	public enum InsertStatus
	{
		DoInsert = 0,
		DontInsert_InforDBMissing = 1,
		DontInsert_RecordExists = 2
	}
}
