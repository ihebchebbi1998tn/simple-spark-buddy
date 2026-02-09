namespace Main.Extensions
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Extensions;

	using MimeKit;
	using MimeKit.Text;

	public static class Pop3ClientExtensions
	{
		public static IEnumerable<MailboxAddress> ToMailAddresses(this InternetAddressList internetAddresses)
		{
			return internetAddresses.Mailboxes.Select(x => new MailboxAddress(x.Name, x.Address));
		}
		public static IEnumerable<string> GetAddresses(this InternetAddressList internetAddresses)
		{
			return internetAddresses.Mailboxes.Select(x => x.Address);
		}
		public static string ConvertHtmlToPlainText(this TextPart textPart)
		{
			TextConverter converter = new HtmlToHtml
			{
				HtmlTagCallback = (HtmlTagContext ctx, HtmlWriter htmlWriter) => { 
					ctx.DeleteEndTag = true;
					ctx.DeleteTag = true;
					if (ctx.TagId == HtmlTagId.Br)
						htmlWriter.WriteText("\r");
					return; 
				},
			};
			return converter.Convert(textPart.Text);
		}
	}
}
