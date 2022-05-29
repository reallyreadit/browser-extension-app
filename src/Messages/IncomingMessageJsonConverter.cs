// Copyright (C) 2022 reallyread.it, inc.
// 
// This file is part of Readup.
// 
// Readup is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 as published by the Free Software Foundation.
// 
// Readup is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License version 3 along with Foobar. If not, see <https://www.gnu.org/licenses/>.

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