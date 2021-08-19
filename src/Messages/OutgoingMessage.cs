namespace BrowserExtensionApp.Messages {
	public class OutgoingMessage {
		public OutgoingMessage(string version, bool succeeded, ProblemDetails error = null) {
			Version = version;
			Succeeded = succeeded;
			Error = error;
		}
		public string Version { get; }
		public bool Succeeded { get; }
		public ProblemDetails Error { get; }
	}
}