using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserExtensionApp.Messages {
	public class IncomingMessageJsonConverter : JsonConverter {
		private IncomingMessage<T> CreateIncomingMessage<T>(string type, JObject jsonObject, JsonSerializer serializer) {
			return new IncomingMessage<T>
			{
				Type = type,
				Data = serializer.Deserialize<T>(
					jsonObject.GetValue("data").CreateReader()
				)
			};
		}
		public override bool CanConvert(Type objectType) {
			return typeof(IncomingMessage).IsAssignableFrom(objectType);
		}
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			var jsonObject = JObject.Load(reader);
			var messageType = jsonObject.Value<string>("type");
			switch (messageType) {
				case "readArticle":
					return CreateIncomingMessage<ReadArticleMessageData>(messageType, jsonObject, serializer);
				default:
					return new IncomingMessage
					{
						Type = messageType
					};
			}
		}
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			throw new NotImplementedException();
		}
	}
}