namespace Main.Offline.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Text.Encodings.Web;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;
	using Crm.Library.Validation.Extensions;

	using Main.Extensions;

	using Microsoft.AspNetCore.Html;
	using Microsoft.AspNetCore.Mvc.Rendering;

	using InputType = Main.Extensions.InputType;

	public static class HtmlHelperExtensions
	{
		private const string ConditionBeginHtml = "<!-- ko if: {0} -->";
		private const string NegatedConditionBeginHtml = "<!-- ko ifnot: {0} -->";
		private const string ConditionEndHtml = "<!-- /ko -->";

		public static IHtmlContent TemplateDate<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, object>> expression, string htmlElement = "span", string pattern = null)
				where TRestModel : class
		{
			return htmlHelper.TemplateDate(expression.GetExpressionString().AppendIfMissing("()"), htmlElement, pattern);
		}
		public static IHtmlContent TemplateDate(this IHtmlHelper htmlHelper, string bindingContext, string htmlElement = "span", string pattern = null)
		{
			pattern = pattern ?? "{ date: 'medium' } ";
			return htmlHelper.Raw(String.Format("<{0} data-bind=\"dateText: {{ value: {1}, pattern: {2} }}\"></{0}>", htmlElement, bindingContext, pattern));
		}
		public static IHtmlContent TemplateTime<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, object>> expression, string htmlElement = "span")
			where TRestModel : class
		{
			return htmlHelper.TemplateTime(expression.GetExpressionString(), htmlElement);
		}
		public static IHtmlContent TemplateTime(this IHtmlHelper htmlHelper, string bindingContext, string htmlElement = "span")
		{
			return htmlHelper.Raw(String.Format("<{0} data-bind=\"dateText: {{ value: {1}, pattern: {{ time: 'short' }} }}\"></{0}>", htmlElement, bindingContext));
		}

		public static IHtmlContent TemplateLink<TRestModel>(this IHtmlHelper htmlHelper, string linkText, string href, Expression<Func<TRestModel, object>> idExpression, object htmlAttributes = null)
				where TRestModel : class
		{
			return htmlHelper.TemplateLink(linkText, href, idExpression, htmlAttributes.CreateDictionary());
		}

		public static IHtmlContent TemplateLink<TRestModel>(this IHtmlHelper htmlHelper, string linkText, string href, Expression<Func<TRestModel, object>> idExpression, IDictionary<string, object> htmlAttributes)
			where TRestModel : class
		{
			htmlAttributes.Add("data-bind", "attr: { href: '" + href + "' + window.ko.utils.unwrapObservable(" + idExpression.GetExpressionString() + ") }");
			htmlAttributes.Add("href", "#");
			var tagBuilder = new TagBuilder("a");
			tagBuilder.MergeAttributes(htmlAttributes);
			tagBuilder.InnerHtml.AppendHtml(linkText);
			return tagBuilder;
		}

		public static IHtmlContent TemplateCondition<TRestModel>(this string html, Expression<Func<TRestModel, object>> expression, bool checkIfTrue = true)
			where TRestModel : class
		{
			var stringBuilder = new HtmlContentBuilder();
			stringBuilder.AppendFormat(checkIfTrue ? ConditionBeginHtml : NegatedConditionBeginHtml, expression.GetExpressionString());
			stringBuilder.AppendHtml(html);
			stringBuilder.AppendHtml(ConditionEndHtml);
			return stringBuilder.ToHtmlString();
		}

		public static IHtmlContent TemplateCondition(this string html, string bindingContext, string conditionElse)
		{
			var stringBuilder = new HtmlContentBuilder();
			stringBuilder.AppendFormat(ConditionBeginHtml, bindingContext);
			stringBuilder.AppendHtml(html);
			stringBuilder.AppendHtml(ConditionEndHtml);
			stringBuilder.AppendFormat(NegatedConditionBeginHtml, bindingContext);
			stringBuilder.AppendHtml(conditionElse);
			stringBuilder.AppendHtml(ConditionEndHtml);
			return stringBuilder.ToHtmlString();
		}

		public static IDisposable TemplateCondition<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, object>> expression, bool checkIfTrue = true)
			where TRestModel : class
		{
			return new DisposableHelper(
				() => htmlHelper.ViewContext.Writer.Write(String.Format(checkIfTrue ? ConditionBeginHtml : NegatedConditionBeginHtml, expression.GetExpressionString())),
				() => htmlHelper.ViewContext.Writer.Write(ConditionEndHtml));
		}

		public static IDisposable TemplateCondition(this IHtmlHelper htmlHelper, string condition, bool checkIfTrue = true)
		{
			return new DisposableHelper(
				() => htmlHelper.ViewContext.Writer.Write(String.Format(checkIfTrue ? ConditionBeginHtml : NegatedConditionBeginHtml, condition)),
				() => htmlHelper.ViewContext.Writer.Write(ConditionEndHtml));
		}

		public static IDisposable TemplateVisible(this IHtmlHelper htmlHelper, string condition)
		{
			return new DisposableHelper(
				() => htmlHelper.ViewContext.Writer.Write(String.Format("<div data-bind=\"visible: {0}\">", condition)),
				() => htmlHelper.ViewContext.Writer.Write("</div>"));
		}

		public static IDisposable TemplateForEach(this IHtmlHelper htmlHelper, string objectName)
		{
			return new DisposableHelper(
				() => htmlHelper.ViewContext.Writer.Write(String.Format("<!-- ko if: {0} --><!-- ko foreach: {0} -->", objectName)),
				() => htmlHelper.ViewContext.Writer.Write("<!-- /ko --><!-- /ko -->"));
		}

		public static IDisposable TemplateContext<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, object>> expression)
			where TRestModel : class
		{
			return new DisposableHelper(
				() => htmlHelper.ViewContext.Writer.Write(String.Format("<!-- ko if: {0} --><!-- ko with: {0} -->", expression.GetExpressionString())),
				() => htmlHelper.ViewContext.Writer.Write(ConditionEndHtml + ConditionEndHtml));
		}

		public static IDisposable TemplateContext(this IHtmlHelper htmlHelper, string model)
		{
			return new DisposableHelper(
				() => htmlHelper.ViewContext.Writer.Write(String.Format("<!-- ko if: window.ko.utils.unwrapObservable({0}) --><!-- ko with: {0} -->", model)),
				() => htmlHelper.ViewContext.Writer.Write("<!-- /ko --><!-- /ko -->"));
		}

		#region Fluent Template HtmlHelper Test
		public class TemplateElement<TRestModel, TProperty> : TemplateElementBase
			where TRestModel : class
		{
			protected string htmlTagElement;

			internal TemplateElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, TProperty>> expression, string htmlTagElement)
			:base(htmlHelper)
			{
				this.htmlTagElement = htmlTagElement;
				dataBindings.Add("text", new AttributeValue(expression.GetExpressionString()));
			}

			public TemplateElement<TRestModel, TProperty> Tag(string tagName)
			{
				htmlTagElement = tagName;
				return this;
			}

			public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
			{
					var tagBuilder = new TagBuilder(htmlTagElement);
					if (dataBindings.Any())
					{
						var dataBindingsString = GetDataBindingsString(dataBindings);
						htmlAttributes.Add("data-bind", dataBindingsString);
					}
					if (classes.Any())
					{
						var classesString = classes.Join(", ");
						htmlAttributes.Add("class", classesString);
					}
					tagBuilder.MergeAttributes(htmlAttributes);
					tagBuilder.WriteTo(writer, encoder);
			}
		}

		public class TemplateEnumElement<TRestModel> : TemplateElement<TRestModel, Enum>
					where TRestModel : class
		{
			internal TemplateEnumElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, Enum>> expression, string htmlTagElement)
				: base(htmlHelper, expression, htmlTagElement)
			{
				var crmViewPage = htmlHelper.GetCrmViewPage();
				if (crmViewPage == null)
				{
					throw new ArgumentException($"ViewPage has to implement {nameof(ICrmViewPage)} to support Html.TemplateEnumElement");
				}
				var enumType = ((UnaryExpression)expression.Body).Operand.Type;
				var enumValuesJson = new StringBuilder("{");
					var enumValues = new List<string>();
					foreach (var enumValue in Enum.GetValues(enumType))
					{
						var translation = crmViewPage.ResourceManager.GetTranslation(Enum.GetName(enumType, enumValue), CultureInfo.CurrentUICulture);
						enumValues.Add(string.Format("{0}: '{1}'", (int)enumValue, translation));
					}
				enumValuesJson.Append(enumValues.Join(", "));
				enumValuesJson.Append("}");
				dataBindings.Add("enumValue", new AttributeValue(expression.GetExpressionString()));
				dataBindings.Add("enumValues", new AttributeValue(enumValuesJson.ToString()));
				dataBindings.Remove("text");
			}
		}
		public class TemplateDateTimeElement<TRestModel> : TemplateElement<TRestModel, DateTime>
			where TRestModel : class
		{
			private string bindingName = "dateText";

			internal TemplateDateTimeElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, DateTime>> expression, string htmlTagElement)
				: base(htmlHelper, expression, htmlTagElement)
			{
				dataBindings.Add(this.bindingName, new AttributeValue(expression.GetExpressionString()));
			}
			public TemplateDateTimeElement<TRestModel> Pattern(string pattern)
			{
				var bindingClass = dataBindings[this.bindingName].BindingClass;

				if (!bindingClass.ContainsKey("pattern"))
				{
					bindingClass.Add("pattern", pattern);
				}
				else
				{
					bindingClass["pattern"] = pattern;
				}
				return this;
			}
		}
		public class TemplateNullableDateTimeElement<TRestModel> : TemplateElement<TRestModel, DateTime?>
			where TRestModel : class
		{
			private string bindingName = "dateText";

			internal TemplateNullableDateTimeElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, DateTime?>> expression, string htmlTagElement)
				: base(htmlHelper, expression, htmlTagElement)
			{
				dataBindings.Add(this.bindingName, new AttributeValue(expression.GetExpressionString()));
			}
			public TemplateNullableDateTimeElement<TRestModel> Pattern(string pattern)
			{
				var bindingClass = dataBindings[this.bindingName].BindingClass;

				if (!bindingClass.ContainsKey("pattern"))
				{
					bindingClass.Add("pattern", pattern);
				}
				else
				{
					bindingClass["pattern"] = pattern;
				}
				return this;
			}
		}
		public class TemplateTimeSpanElement<TRestModel> : TemplateElement<TRestModel, TimeSpan>
					where TRestModel : class
		{
			private string bindingName = "durationText";

			internal TemplateTimeSpanElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, TimeSpan>> expression, string htmlTagElement)
				: base(htmlHelper, expression, htmlTagElement)
			{
				dataBindings.Add(this.bindingName, new AttributeValue(expression.GetExpressionString()));
			}
		}
		public class TemplateNullableTimeSpanElement<TRestModel> : TemplateElement<TRestModel, TimeSpan?>
			where TRestModel : class
		{
			private string bindingName = "durationText";

			internal TemplateNullableTimeSpanElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, TimeSpan?>> expression, string htmlTagElement)
				: base(htmlHelper, expression, htmlTagElement)
			{
				dataBindings.Add(bindingName, new AttributeValue(expression.GetExpressionString()));
			}
		}

		public class TemplateDecimalElement<TRestModel> : TemplateElement<TRestModel, decimal>
					where TRestModel : class
		{
			internal TemplateDecimalElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, decimal>> expression, string htmlTagElement)
				: base(htmlHelper, expression, htmlTagElement)
			{
				dataBindings.Add("money", new AttributeValue(expression.GetExpressionString()));
			}
		}
		public class TemplateNullableDecimalElement<TRestModel> : TemplateElement<TRestModel, decimal?>
			where TRestModel : class
		{
			internal TemplateNullableDecimalElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, decimal?>> expression, string htmlTagElement)
				: base(htmlHelper, expression, htmlTagElement)
			{
				dataBindings.Add("money", new AttributeValue(expression.GetExpressionString()));
			}
		}

		public static TemplateElement<TRestModel, object> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, object>> expression)
			where TRestModel : class
		{
			return new TemplateElement<TRestModel, object>(htmlHelper, expression, "span");
		}

		public static TemplateElement<TRestModel, int> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, int>> expression)
			where TRestModel : class
		{
			return new TemplateElement<TRestModel, int>(htmlHelper, expression, "span");
		}

		public static TemplateElement<TRestModel, int?> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, int?>> expression)
			where TRestModel : class
		{
			return new TemplateElement<TRestModel, int?>(htmlHelper, expression, "span");
		}

		public static TemplateElement<TRestModel, long> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, long>> expression)
			where TRestModel : class
		{
			return new TemplateElement<TRestModel, long>(htmlHelper, expression, "span");
		}

		public static TemplateElement<TRestModel, long?> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, long?>> expression)
			where TRestModel : class
		{
			return new TemplateElement<TRestModel, long?>(htmlHelper, expression, "span");
		}

		public static TemplateEnumElement<TRestModel> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, Enum>> expression)
			where TRestModel : class
		{
			return new TemplateEnumElement<TRestModel>(htmlHelper, expression, "span");
		}

		public static TemplateDateTimeElement<TRestModel> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, DateTime>> expression)
					where TRestModel : class
		{
			return new TemplateDateTimeElement<TRestModel>(htmlHelper, expression, "span");
		}
		public static TemplateNullableDateTimeElement<TRestModel> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, DateTime?>> expression)
			where TRestModel : class
		{
			return new TemplateNullableDateTimeElement<TRestModel>(htmlHelper, expression, "span");
		}
		public static TemplateTimeSpanElement<TRestModel> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, TimeSpan>> expression)
			where TRestModel : class
		{
			return new TemplateTimeSpanElement<TRestModel>(htmlHelper, expression, "span");
		}
		public static TemplateNullableTimeSpanElement<TRestModel> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, TimeSpan?>> expression)
			where TRestModel : class
		{
			return new TemplateNullableTimeSpanElement<TRestModel>(htmlHelper, expression, "span");
		}
		public static TemplateDecimalElement<TRestModel> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, decimal>> expression)
			where TRestModel : class
		{
			return new TemplateDecimalElement<TRestModel>(htmlHelper, expression, "span");
		}
		public static TemplateNullableDecimalElement<TRestModel> TemplateText<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, decimal?>> expression)
			where TRestModel : class
		{
			return new TemplateNullableDecimalElement<TRestModel>(htmlHelper, expression, "span");
		}
		#endregion

		#region Form elements

		#region Fluent Template HtmlHelper Test

		public abstract class TemplateElementBase : IHtmlContent
		{
			internal readonly Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();
			internal readonly Dictionary<string, AttributeValue> dataBindings = new Dictionary<string, AttributeValue>();
			internal readonly List<string> classes = new List<string>();
			internal readonly IHtmlHelper htmlHelper;
			public TemplateElementBase(IHtmlHelper htmlHelper)
			{
				this.htmlHelper = htmlHelper;
			}
			public override string ToString()
			{
				var writer = new StringWriter();
				WriteTo(writer, HtmlEncoder.Default);
				return writer.ToString();
			}
			public abstract void WriteTo(TextWriter writer, HtmlEncoder encoder);
		}
		public abstract class TemplateFormElementBase : TemplateElementBase
		{
			internal string caption;
			internal InputType inputType;
			protected TemplateFormElementBase(IHtmlHelper htmlHelper)
				: base(htmlHelper)
			{
			}
		}
		public abstract class TemplateFormElementBase<TProperty> : TemplateFormElementBase
		{
			protected TemplateFormElementBase(IHtmlHelper htmlHelper)
				: base(htmlHelper)
			{
			}
		}

		public class TemplateFormElement<TRestModel, TProperty> : TemplateFormElementBase<TProperty>
			where TRestModel : class
		{
			private readonly Expression<Func<TRestModel, TProperty>> expression;
			internal TemplateFormElement(IHtmlHelper htmlHelper, InputType inputType, Expression<Func<TRestModel, TProperty>> expression)
				: base(htmlHelper)
			{
				caption = htmlHelper.Localize(expression.GetPropertyName());
				this.inputType = inputType;
				this.expression = expression;
				if (inputType == InputType.AutoCompleter || inputType == InputType.DatePicker || inputType == InputType.DropDownList || inputType == InputType.GenericListSelection || inputType == InputType.GroupDropDownList || inputType == InputType.TextArea || inputType == InputType.TextBox)
				{
					classes.Add("form-control");
				}
			}

			public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
			{
				if (classes.Any())
				{
					var classesString = classes.Join(", ");
					htmlAttributes.Add("class", classesString);

				}

				htmlHelper.GetTemplateInput(inputType, expression, caption, htmlAttributes: htmlAttributes, dataBindings: dataBindings).WriteTo(writer, encoder);
			}
		}

		public class TemplateDatePickerFormElement<TRestModel> : TemplateFormElement<TRestModel, DateTime>
			where TRestModel : class
		{
			internal TemplateDatePickerFormElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, DateTime>> expression)
				: base(htmlHelper, InputType.DatePicker, expression)
			{
			}
		}
		public class TemplateNullableDatePickerFormElement<TRestModel> : TemplateFormElement<TRestModel, DateTime?>
			where TRestModel : class
		{
			internal TemplateNullableDatePickerFormElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, DateTime?>> expression)
				: base(htmlHelper, InputType.DatePicker, expression)
			{
			}
		}

		public class TemplateTextFormElement<TRestModel> : TemplateFormElement<TRestModel, string>
			where TRestModel : class
		{
			internal TemplateTextFormElement(IHtmlHelper htmlHelper, Expression<Func<TRestModel, string>> expression)
				: base(htmlHelper, InputType.TextBox, expression)
			{
			}

			public TemplateTextFormElement<TRestModel> Rows(int rows)
			{
				if (rows == 1)
				{
					this.SetHtmlAttribute("rows", rows);
					inputType = InputType.TextBox;
				}
				else if (rows > 1)
				{
					this.SetHtmlAttribute("rows", rows);
					inputType = InputType.TextArea;
				}
				else
				{
					throw new ArgumentException("rows has to be >= 1");
				}
				return this;
			}
		}

		public static TemplateNullableDatePickerFormElement<TRestModel> TemplateInput<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, DateTime?>> expression)
			where TRestModel : class
		{
			return new TemplateNullableDatePickerFormElement<TRestModel>(htmlHelper, expression);
		}

		public static TemplateTextFormElement<TRestModel> TemplateInput<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, string>> expression)
			where TRestModel : class
		{
			return new TemplateTextFormElement<TRestModel>(htmlHelper, expression);
		}

		public static TemplateFormElement<TRestModel, int> TemplateInput<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, int>> expression)
			where TRestModel : class
		{
			return new TemplateFormElement<TRestModel, int>(htmlHelper, InputType.TextBox, expression);
		}

		public static TemplateFormElement<TRestModel, bool> TemplateInput<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, bool>> expression)
			where TRestModel : class
		{
			return new TemplateFormElement<TRestModel, bool>(htmlHelper, InputType.CheckBox, expression);
		}

		public static TemplateFormElement<TRestModel, double?> TemplateInput<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, double?>> expression)
			where TRestModel : class
		{
			return new TemplateFormElement<TRestModel, double?>(htmlHelper, InputType.TextBox, expression);
		}

		public static TemplateFormElement<TRestModel, decimal> TemplateInput<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, decimal>> expression)
			where TRestModel : class
		{
			return new TemplateFormElement<TRestModel, decimal>(htmlHelper, InputType.TextBox, expression);
		}
		public static TemplateFormElement<TRestModel, TimeSpan?> TemplateInput<TRestModel>(this IHtmlHelper htmlHelper, Expression<Func<TRestModel, TimeSpan?>> expression, bool onlyTime)
			where TRestModel : class
		{
			var element = new TemplateFormElement<TRestModel, TimeSpan?>(htmlHelper, InputType.Hidden, expression);
			element.dataBindings.Add("timePicker", new AttributeValue(expression.GetExpressionString()));
			element.dataBindings.Add("pickDuration", new AttributeValue("true"));
			if(onlyTime)
				element.dataBindings.Add("onlyTime", new AttributeValue("true"));
			return element;
		}

		# endregion
		private static IHtmlContent GetTemplateInput<TRestModel, TProperty>(this IHtmlHelper htmlHelper, InputType inputType, Expression<Func<TRestModel, TProperty>> expression, string localizedCaption, TRestModel entity = null, Dictionary<string, object> htmlAttributes = null, Dictionary<string, AttributeValue> dataBindings = null, params object[] inputTypeSpecificObject)
			where TRestModel : class
		{
			var bindingValue = expression.GetExpressionString();

			var crmViewPage = htmlHelper.GetCrmViewPage();
			if (crmViewPage == null)
			{
				throw new ArgumentException($"ViewPage has to implement {nameof(ICrmViewPage)} to support Html.GetTemplateInput");
			}
			var ruleProvider = crmViewPage.RuleProvider;
			var restTypeProvider = crmViewPage.RestTypeProvider;
			var domainType = restTypeProvider.GetDomainType(typeof(TRestModel));
			var rules = ruleProvider.GetRulesFor<TRestModel>().Where(r => r.PropertyName == expression.GetPropertyName()).GetClientSideRules().ToList();
			if (domainType != null && domainType != typeof(TRestModel))
			{
				var domainTypeRules = ruleProvider.GetRulesFor(domainType).Where(r => r.PropertyName == expression.GetPropertyName()).GetClientSideRules().ToList();
				rules.AddRange(domainTypeRules);
			}
			foreach (var rule in rules)
			{
				bindingValue += GetTemplateRuleExtender(rule, crmViewPage.ResourceManager);
			}

			if (inputType != InputType.DatePicker && inputType != InputType.CheckBox)
			{
				dataBindings = dataBindings ?? new Dictionary<string, AttributeValue>();
				dataBindings.Add("value", new AttributeValue(bindingValue));
			}
			else if (inputType == InputType.CheckBox)
			{
				dataBindings = dataBindings ?? new Dictionary<string, AttributeValue>();
				dataBindings.Add("checked", new AttributeValue(bindingValue));
			}
			else if (inputType == InputType.DatePicker)
			{
				dataBindings = dataBindings ?? new Dictionary<string, AttributeValue>();
				dataBindings.Add("datePicker", new AttributeValue(bindingValue));
			}

			//var dataBindingsString = dataBindings == null ? String.Empty : ", " + dataBindings.Select(b => String.Format("{0}: {1}", b.Key, b.Value)).Join(", ");;
			var htmlAttributesDict = new Dictionary<string, object>();
			if (dataBindings != null && dataBindings.Any())
			{
				var dataBindingsString = GetDataBindingsString(dataBindings);
				htmlAttributesDict.Add("data-bind", dataBindingsString);
			}

			if (htmlAttributes != null)
			{
				var keysToRemove = new List<string>();
				foreach (var item in htmlAttributes)
				{
					if (htmlAttributesDict.ContainsKey(item.Key))
					{
						htmlAttributesDict[item.Key] = htmlAttributesDict[item.Key] + ", " + htmlAttributes[item.Key];
						keysToRemove.Add(item.Key);
					}
				}

				foreach (var key in keysToRemove)
				{
					htmlAttributes.Remove(key);
				}

				htmlAttributesDict.AddAll(htmlAttributes);
			}

			var sb = new StringBuilder();
			sb.AppendLine("<div class=\"form-group\">");
			sb.AppendFormat("<label class=\"field\" data-bind=\"validationElement: {0}, validationOptions: {{ insertMessages: false }}\">", expression.GetPropertyName());
			var description = new StringBuilder();
			if (localizedCaption.IsNotNullOrWhiteSpace())
			{
				description.Append("<span class=\"description\">");
				description.Append(localizedCaption);
				if (rules.Any(r => r is RequiredRule<TRestModel> && r.PropertyName == expression.GetPropertyName()))
				{
					var requiredMarker = "{0}";
					if (typeof(IHasRequiredProperty).IsAssignableFrom(typeof(TRestModel)))
					{
						if (entity != null && ((IHasRequiredProperty)entity).Required)
						{
							requiredMarker = "<span data-bind=\"visible: Required\">{0}</span>";
						}
						else
						{
							requiredMarker = "<span data-bind=\"visible: Required\" style=\"display: none\">{0}</span>";
						}
					}
					requiredMarker = string.Format(requiredMarker, "&nbsp;" + htmlHelper.RequiredMarker());

					description.Append(requiredMarker);
				}
				description.Append("</span>");
			}

			if (inputType != InputType.CheckBox && inputType != InputType.Hidden)
			{
				sb.Append(description);
			}

			switch (inputType)
			{
				case InputType.DropDownList:
					htmlAttributesDict.Add("data-theme", "d");
					htmlAttributesDict.AppendToValue("class", " form-control");
					sb.AppendLine("</label>");
					sb.AppendLine(htmlHelper.DropDownList(expression.GetPropertyName(), new List<SelectListItem>(), htmlAttributesDict).ToHtmlString().Value);
					break;
				case InputType.Hidden:
					sb.AppendLine("</label>");
					sb.AppendLine(htmlHelper.Hidden(expression.GetPropertyName(), null, htmlAttributesDict).ToHtmlString().Value);
					break;
				case InputType.TextBox:
					var propertyType = expression.GetPropertyType();
					if (propertyType.IsNumericType())
					{
						htmlAttributesDict.AppendToValue("class", " form-control number-input");
						htmlAttributesDict.Add("type", "number");
						if (!propertyType.IsIntegerType())
						{
							htmlAttributesDict.Add("step", "0.01");
						}
					}
					sb.AppendLine("</label>");
					sb.AppendLine(htmlHelper.TextBox(expression.GetPropertyName(), null, htmlAttributesDict).ToHtmlString().Value);

					// TODO: remaining characters count for max length rule

					//var remainingCharacterCount = new TagBuilder("span");
					//remainingCharacterCount.AddCssClass("remaining-char-count");
					//remainingCharacterCount.InnerHtml = "100";

					//var remainingCharacters = new TagBuilder("span");
					//remainingCharacters.AddCssClass("remaining-char-info");
					//remainingCharacters.InnerHtml = remainingCharacterCount.ToString(TagRenderMode.Normal); // +" " + htmlHelper.Localize("charactersRemaining");

					//sb.AppendLine(remainingCharacters.ToString(TagRenderMode.Normal));

					break;
				case InputType.TextArea:
					if (inputTypeSpecificObject.Length > 0 && inputTypeSpecificObject[0] is int)
					{
						var rows = (int)inputTypeSpecificObject[0];
						htmlAttributesDict.Add("rows", rows);
					}
					htmlAttributesDict.AppendToValue("class", " form-control");
					sb.AppendLine("</label>");
					sb.AppendLine(htmlHelper.TextArea(expression.GetPropertyName(), null, htmlAttributesDict).ToHtmlString().Value);
					break;
				case InputType.DatePicker:
					//htmlAttributesDict.AppendToValue("class", " wrapped-date-picker");
					//sb.AppendLine("<div class=\"date-picker-floater\">");
					htmlAttributesDict.AppendToValue("class", " form-control");
					sb.AppendLine("</label>");
					sb.AppendLine(htmlHelper.TextBox(expression.GetPropertyName(), DateTime.Now.ToString(Global.CurrentCulture.DateTimeFormat.ShortDatePattern), htmlAttributesDict).ToHtmlString().Value);
					//sb.AppendLine("</div>");
					break;
				case InputType.CheckBox:
					sb.AppendLine(htmlHelper.CheckBox(expression.GetPropertyName(), htmlAttributesDict).ToHtmlString().Value);
					sb.Append(description);
					sb.AppendLine("</label>");
					break;
			}

			sb.AppendLine("</div>");
			sb.AppendFormatLine("<p data-bind=\"validationMessage: {0}, validationElement: {0}\"></p>", expression.GetPropertyName());
			return htmlHelper.Raw(sb.ToString());
		}

		private static string GetTemplateRuleExtender(Rule rule, IResourceManager resourceManager)
		{
			const string ruleExtender = ".extend({{ {0}: {{ message: '{1}', params: {2} }} }})";
			var message = rule.GetTranslatedErrorMessage(resourceManager).Replace("'", "\\'");
			if (rule.IsRequiredRule())
			{
				return ruleExtender.WithArgs("required", message, "true");
			}
			if (rule.IsMaxLengthRule())
			{
				return ruleExtender.WithArgs("maxLength", message, ((IMaxLengthRule)rule).MaxLength);
			}
			if (rule.IsMinLengthRule())
			{
				return ruleExtender.WithArgs("minLength", message, ((IMinLengthRule)rule).MinLength);
			}
			if (rule.IsNotNegativeRule())
			{
				return ruleExtender.WithArgs("min", message, 0);
			}
			if (rule.IsDigitRule())
			{
				return ruleExtender.WithArgs("digit", message, "true");
			}
			if (rule.IsNumberRule())
			{
				return ruleExtender.WithArgs("number", message, "true");
			}

			throw new NotImplementedException("Client-side rule extender not defined for type " + rule.GetType().FullName);
		}

		#endregion

		private static string GetDataBindingsString(Dictionary<string, AttributeValue> dataBindings)
		{
			//data-bind=\"bindingName1: bindingValue1, bindingName2: bindingValue2\"
			var notNestedBindingValues = dataBindings.Where(item => !(item.Value).BindingClass.Any());
			var notNestedBindingValuesString = notNestedBindingValues.Select(b => String.Format("{0}: {1}", b.Key, b.Value.BindingValue)).Join(", ");

			//data-bind=\"bindingName: {{ value: bindingValue, bindingParameter1: valueParameter1, bindingParameter2: valueParameter2}}\"
			var nestedBindingValues = dataBindings.Where(item => item.Value.BindingClass.Any());
			var nestedBindingValuesString = nestedBindingValues.Select(
				outer => String.Format("{0}: {{ value: {1}, {2} }}", outer.Key, outer.Value.BindingValue,
					outer.Value.BindingClass.Select(inner => String.Format("{0}: {1}", inner.Key, inner.Value)).Join(", ")))
				.Join(", ");

			var dataBindingsString = nestedBindingValuesString == string.Empty
				? notNestedBindingValuesString
				: string.Join(", ", notNestedBindingValuesString, nestedBindingValuesString);

			return dataBindingsString;
		}

	}

	public class AttributeValue
	{
		internal AttributeValue(string bindingValue)
		{
			this.BindingValue = bindingValue;
			this.BindingClass = new Dictionary<string, string>();
		}
		public string BindingValue { get; set; }
		public Dictionary<string, string> BindingClass { get; set; }
	}

	public class DateTimePattern
	{
		/// <summary>
		/// Result culture depending: dd.MM.
		/// </summary>
		public const string DayAndMonth = "{ skeleton: 'MMdd' }";

		/// <summary>
		/// Result culture depending: DDDD, dd.MMM yyyy
		/// </summary>
		public const string DateFull = "{ date: 'full' }";

		/// <summary>
		/// Result culture depending: dd.MM.yyyy
		/// </summary>
		public const string DateMedium = "{ date: 'medium' }";

		/// <summary>
		/// Result culture depending: HH:mm
		/// </summary>
		public const string TimeShort = "{ time: 'short' }";

		/// <summary>
		/// Result culture depending: DDDD, dd.MMM yyyy HH:mm
		/// </summary>
		public const string DataTimeFull = "{ skeleton: 'yMMMEdHm' }";

		/// <summary>
		/// Result culture depending: dd.MM.yy, HH:mm
		/// </summary>
		public const string DateTimeShort = "{ datetime: 'short' }";

		/// <summary>
		/// Result culture depending: dd.MM.yyyy, HH:mm
		/// </summary>
		public const string DateTimeMedium = "{ skeleton: 'yMdHm'}";
	}

}
