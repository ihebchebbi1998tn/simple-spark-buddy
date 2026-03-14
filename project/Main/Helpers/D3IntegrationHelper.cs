namespace Main.Helpers
{
	using System;
	using System.Text;
	using System.Threading;

	using Crm.Library.Extensions.IIdentity;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	public static class D3IntegrationHelper
	{
		public static FileContentResult OpenD3Document(HttpResponse response, string documentType, string keyField, string inforId)
		{
			var contents = new StringBuilder();
			contents.AppendLine("search");
			contents.AppendLine(String.Format("doc_type={0}", documentType));
			contents.AppendLine(String.Format("{0}={1}", keyField, inforId));

			return UploadD3LinkFile(response, contents.ToString(), inforId);
		}
		public static FileContentResult OpenSalesDocument(HttpResponse response, string documentType, string keyField, string inforId)
		{
			var contents = new StringBuilder();
			contents.AppendLine("search");
			contents.AppendLine(String.Format("doc_type={0}", documentType));
			contents.AppendLine(String.Format("{0}={1}", keyField, inforId));

			return UploadD3LinkFile(response, contents.ToString(), inforId);
		}
		public static byte[] CreateMetaFile(string inforId, string contactType, string name, string key, string originalFileName, string comment)
		{
			var contents = new StringBuilder();
			contents.AppendLine(String.Format("dok_dat_feld[39] = \"{0}\"", "Leiner"));
			contents.AppendLine(String.Format("dok_dat_feld[50] = \"{0}\"", DateTime.Now.Date.ToString("dd.MM.yyyy")));
			contents.AppendLine(String.Format("dok_dat_feld[18] = \"{0}\"", originalFileName));
			contents.AppendLine(String.Format("dok_dat_feld[38] = \"{0}\"", Thread.CurrentPrincipal.Identity.GetUserName()));
			contents.AppendLine(String.Format("dok_dat_feld[6] = \"{0}\"", key));

			switch (contactType)
			{
				case "Company":
					contents.AppendLine(String.Format("dok_dat_feld[1] = \"{0}\"", inforId));
					contents.AppendLine(String.Format("dok_dat_feld[2] = \"{0}\"", name));
					contents.AppendLine(String.Format("dokuart = \"{0}\"", "KNCRM"));
					break;
				case "Person":
					contents.AppendLine(String.Format("dok_dat_feld[1] = \"{0}\"", inforId));
					contents.AppendLine(String.Format("dok_dat_feld[7] = \"{0}\"", name));
					contents.AppendLine(String.Format("dokuart = \"{0}\"", "PNCRM"));
					break;
				case "Project":
					contents.AppendLine(String.Format("dok_dat_feld[26] = \"{0}\"", name));
					contents.AppendLine(String.Format("dokuart = \"{0}\"", "PJCRM"));
					break;
			}

			return Encoding.Default.GetBytes(contents.ToString());
		}
		public static byte[] CreateActionFile(string fileExtension)
		{
			var contents = new StringBuilder();
			contents.AppendLine(String.Format("import_file_ext=\"{0}\"", fileExtension));

			return Encoding.Default.GetBytes(contents.ToString());
		}

		public static FileContentResult UploadD3LinkFile(HttpResponse response, string content, string fileName)
		{
			//response.ClearHeaders();
			//response.Clear();
			//response.ContentType = "application/d3link";
			//response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
			//response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}.d3l", fileName));
			//response.Write(content);
			//response.End();
			return new FileContentResult(Encoding.Default.GetBytes(content), "application/d3link")
				{
					FileDownloadName = string.Format("{0}.d3l", fileName)
				};
		}
	}
}
