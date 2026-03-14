namespace Main.Offline.Extensions
{
	public static class TemplateElementExtensions
	{
		public static T SetHtmlAttribute<T>(this T templateElement, string attributeName, object attributeValue)
			where T : HtmlHelperExtensions.TemplateElementBase
		{
			if (templateElement.htmlAttributes.ContainsKey(attributeName))
			{
				templateElement.htmlAttributes[attributeName] = attributeValue;
			}
			else
			{
				templateElement.htmlAttributes.Add(attributeName, attributeValue);
			}
			return templateElement;
		}
		public static T AddClass<T>(this T templateElement, string className)
			where T : HtmlHelperExtensions.TemplateElementBase
		{
			if (!templateElement.classes.Contains(className))
			{
				templateElement.classes.Add(className);
			}
			return templateElement;
		}

		public static T Id<T>(this T templateElement, string id)
			where T : HtmlHelperExtensions.TemplateElementBase
		{
			templateElement.SetHtmlAttribute("id", id);
			return templateElement;
		}
	}
}
