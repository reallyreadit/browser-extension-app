using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using BrowserExtensionApp.Messages;
using BrowserExtensionApp.Win32ProcessCreation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BrowserExtensionApp {
	class Program {
		private static string appVersion = "1.1.0";
		private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings {
			ContractResolver = new DefaultContractResolver {
				NamingStrategy = new CamelCaseNamingStrategy()
			},
			Converters = {
				new IncomingMessageJsonConverter()
			}
		};
		static void Main(string[] args) {
			IncomingMessage incomingMessage;
			using (
				var input = Console.OpenStandardInput()
			) {
				try {
					var incomingMessageSizeBuffer = new byte[4];
					input.Read(incomingMessageSizeBuffer);
					var incomingMessageSize = BitConverter.ToInt32(incomingMessageSizeBuffer);

					var incomingMessageBuffer = new byte[incomingMessageSize];
					input.Read(incomingMessageBuffer);

					incomingMessage = JsonConvert.DeserializeObject<IncomingMessage>(
						Encoding.UTF8.GetString(incomingMessageBuffer),
						jsonSettings
					);
				} catch {
					SendErrorResponse(
						new ProblemDetails(
							type: BrowserExtensionAppErrorType.MessageParsingFailed,
							title: "Failed to parse incoming message."
						)
					);
					return;
				}
			}

			switch (incomingMessage) {
				case IncomingMessage<ReadArticleMessageData> readArticleMessage:
					// Build the url using the Readup protocol to launch the main app.
					var readupAppUrl = new UriBuilder("readup://read");
					readupAppUrl.Query = "?url=" + Uri.EscapeDataString(readArticleMessage.Data.Url);
					if (readArticleMessage.Data.Star) {
						readupAppUrl.Query += "&star";
					}
					// Attempt to open the app.
					bool readupAppProcessCreated;
					try {
						if (
							RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
						) {
							// Firefox will immediately close any process we create here as soon as this application finishes
							// processing the message unless we use the CREATE_BREAKAWAY_FROM_JOB flag which is not available
							// using the managed Process.Start function.
							// https://developer.mozilla.org/en-US/docs/Mozilla/Add-ons/WebExtensions/Native_messaging#closing_the_native_app
							var escapedUrl = readupAppUrl
								.ToString()
								.Replace("&", "^&");
							readupAppProcessCreated = Win32ProcessCreator.CreateProcess(
								commandLine: $"cmd /c start {escapedUrl}",
								processCreationFlags: Win32ProcessCreationFlags.CreateBreakawayFromJob | Win32ProcessCreationFlags.CreateNoWindow
							);
						} else if (
							RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
						) {
							// xdg-open works fine with Firefox.
							var process = Process.Start(
								"xdg-open",
								readupAppUrl.ToString()
							);
							readupAppProcessCreated = process != null;
						} else {
							// macOS has its own BrowserExtensionApp executable written in Swift that is bundled with the Readup App Store app.
							readupAppProcessCreated = false;
						}
					} catch {
						readupAppProcessCreated = false;
					}
					if (readupAppProcessCreated) {
						SendSuccessResponse();
					} else {
						SendErrorResponse(
							new ProblemDetails(
								type: BrowserExtensionAppErrorType.ReadupProtocolFailed,
								title: "Failed to launch Readup app using custom protocol."
							)
						);
					}
					return;
				default:
					SendErrorResponse(
						new ProblemDetails(
							type: BrowserExtensionAppErrorType.UnexpectedMessageType,
							title: $"Unexpected message type: {incomingMessage.Type}."
						)
					);
					return;
			}
		}
		private static void SendResponse(OutgoingMessage message) {
			var outgoingMessageData = Encoding.UTF8.GetBytes(
				JsonConvert.SerializeObject(message, jsonSettings)
			);
			using (
				var output = Console.OpenStandardOutput()
			) {
				output.Write(
					BitConverter.GetBytes(outgoingMessageData.Length)
				);
				output.Write(outgoingMessageData);
			}
		}
		private static void SendSuccessResponse() {
			SendResponse(
				new OutgoingMessage(
					version: appVersion,
					succeeded: true
				)
			);
		}
		private static void SendErrorResponse(ProblemDetails error) {
			SendResponse(
				new OutgoingMessage(
					version: appVersion,
					succeeded: false,
					error
				)
			);
		}
	}
}