namespace BrowserExtensionApp.Messages {
	public class IncomingMessage {
		public string Type { get; set; }
	}
	public class IncomingMessage<T> : IncomingMessage {
		public T Data { get; set; }
	}
}