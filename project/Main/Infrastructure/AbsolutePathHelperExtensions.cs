namespace Main.Infrastructure
{
	using Microsoft.AspNetCore.Routing;

	public static class AbsolutePathHelperExtensions
	{
		public static string GetTasksPath(this IAbsolutePathHelper pathHelper)
		{
			return pathHelper.GetPath("Index", "TaskList", new RouteValueDictionary(new { plugin = "Crm" }));
		}
	}
}
