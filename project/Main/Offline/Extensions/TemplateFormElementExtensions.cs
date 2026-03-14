namespace Main.Offline.Extensions
{
	using Main.Extensions;

	public static class TemplateFormElementExtensions
	{
		public static T UpdateWhileTyping<T>(this T templateFormElement, bool updateWhileTyping = true)
			where T : HtmlHelperExtensions.TemplateFormElementBase
		{
			if (updateWhileTyping)
			{
				if (templateFormElement.dataBindings.ContainsKey("valueUpdate"))
				{
					templateFormElement.dataBindings["valueUpdate"] = new AttributeValue("'input'");
				}
				else
				{
					templateFormElement.dataBindings.Add("valueUpdate", new AttributeValue("'input'"));
				}
			}
			else if(templateFormElement.dataBindings.ContainsKey("valueUpdate"))
			{
				templateFormElement.dataBindings.Remove("valueUpdate");
			}
			return templateFormElement;
		}

		public static T Caption<T>(this T templateFormElement, string newCaption)
			where T : HtmlHelperExtensions.TemplateFormElementBase
		{
			templateFormElement.caption = newCaption;
			return templateFormElement;
		}

		public static T Placeholder<T>(this T templateFormElement, string placeholder)
			where T : HtmlHelperExtensions.TemplateFormElementBase
		{
			templateFormElement.SetHtmlAttribute("placeholder", placeholder);
			return templateFormElement;
		}

		public static T AsHidden<T>(this T templateFormElement)
			where T : HtmlHelperExtensions.TemplateFormElementBase
		{
			templateFormElement.inputType = InputType.Hidden;
			templateFormElement.caption = null;
			return templateFormElement;
		}

		public static T Min<T>(this T templateFormElement, decimal? min)
			where T : HtmlHelperExtensions.TemplateFormElementBase<decimal>
		{
			if (min.HasValue)
			{
				templateFormElement.htmlAttributes["min"] = min.Value;
			}
			else if (templateFormElement.htmlAttributes.ContainsKey("min"))
			{
				templateFormElement.htmlAttributes.Remove("min");
			}
			return templateFormElement;
		}
		public static T Min<T>(this T templateFormElement, int? min)
			where T : HtmlHelperExtensions.TemplateFormElementBase<int?>
		{
			if (min.HasValue)
			{
				templateFormElement.htmlAttributes["min"] = min.Value;
			}
			else if (templateFormElement.htmlAttributes.ContainsKey("min"))
			{
				templateFormElement.htmlAttributes.Remove("min");
			}
			return templateFormElement;
		}

	}
}
