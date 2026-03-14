namespace Main.Model
{
	using System;
	using System.Collections.Generic;

	public class PushNotification
	{
		public IEnumerable<string> Usernames { get; set; }
		public string TitleResourceKey { get; set; }
		public List<string> TitleResourceParams { get; set; } = null;
		public string BodyResourceKey { get; set; }
		public List<string> BodyResourceParams { get; set; } = null;
		public string Url { get; set; }
		public Guid Id { get; set; } = Guid.NewGuid();
		public object Data { get; set; }
	}
}
