namespace BrowserExtensionApp.Messages {
	public class ProblemDetails {
		public ProblemDetails(string detail) {
			Detail = detail;
			Title = "A general exception occurred.";
			Type = GeneralErrorType.Exception;
		}
		public ProblemDetails(string type, string title, string detail = null, string instance = null) {
			Detail = detail;
			Instance = instance;
			Title = title;
			Type = type;
		}
		public string Detail { get; }
		public string Instance { get; }
		public string Title { get; }
		public string Type { get; }
	}
}