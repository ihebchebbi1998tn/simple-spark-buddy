namespace Main.ViewModels
{
	public class ErrorViewModel : CrmModel
	{
		public static readonly ErrorViewModel Forbidden = new ErrorViewModel { StatusCode = 403, ErrorType = "Forbidden" };
		public static readonly ErrorViewModel NotFound = new ErrorViewModel { StatusCode = 404, ErrorType = "NotFound" };
		public static readonly ErrorViewModel InternalServerError = new ErrorViewModel { StatusCode = 500, ErrorType = "InternalServerError" };

		public int StatusCode { get; set; }
		public string ErrorType {get; set; }
	}
}
