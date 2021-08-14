using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;


namespace BrowserExtensionApp {
	class IncomingMessage {
		public string Url { get; set; }
	}
	class OutgoingMessage {
		public int Status { get; set; }
	}

	class Program {
		static void Main(string[] args) {
			var jsonOptions = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			IncomingMessage incomingMessage;
			using (
				var input = Console.OpenStandardInput()
			) {
				var incomingMessageSizeBuffer = new byte[4];
				input.Read(incomingMessageSizeBuffer);
				var incomingMessageSize = BitConverter.ToInt32(incomingMessageSizeBuffer);

				var incomingMessageBuffer = new byte[incomingMessageSize];
				input.Read(incomingMessageBuffer);

				incomingMessage = JsonSerializer.Deserialize<IncomingMessage>(incomingMessageBuffer, jsonOptions);
			}

			var readupAppUrl = new UriBuilder("readup://read");
			readupAppUrl.Query = "?url=" + Uri.EscapeDataString(incomingMessage.Url);

			Process readupAppProcess;
			try {
				if (
					RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
				) {
					readupAppProcess = Process.Start(
						new ProcessStartInfo(
							readupAppUrl.ToString()
						) {
							UseShellExecute = true
						}
					);
				} else if (
					RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
				) {
					readupAppProcess = Process.Start(
						"xdg-open",
						readupAppUrl.ToString()
					);
				} else {
					readupAppProcess = null;
				}
			} catch {
				readupAppProcess = null;
			}

			var outgoingMessage = JsonSerializer.SerializeToUtf8Bytes(
				new OutgoingMessage {
					Status = readupAppProcess != null ? 0 : 1
				},
				jsonOptions
			);
			using (
				var output = Console.OpenStandardOutput()
			) {
				output.Write(
					BitConverter.GetBytes(outgoingMessage.Length)
				);
				output.Write(outgoingMessage);
			}
		}
	}
}