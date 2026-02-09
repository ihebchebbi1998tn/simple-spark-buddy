namespace Main.Controllers
{
	using System;

	using Crm.Library.Extensions;

	using log4net;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	public class LogController : Controller
	{
		private readonly ILog logger;
		public LogController(ILog logger)
		{
			this.logger = logger;
		}
		[AllowAnonymous]
		public virtual void LogError(string message, string url, string line, string column, string error)
		{
			var text = $"Message: {message.RemoveLineBreaks()} {Environment.NewLine}Url: {url.RemoveLineBreaks()}{Environment.NewLine}Line: {line.RemoveLineBreaks()} {Environment.NewLine}Column: {column.RemoveLineBreaks()} {Environment.NewLine}Error: {error.RemoveLineBreaks()}";
			logger.Error(text);
		}
	}
}
