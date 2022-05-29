// Copyright (C) 2022 reallyread.it, inc.
// 
// This file is part of Readup.
// 
// Readup is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 as published by the Free Software Foundation.
// 
// Readup is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License version 3 along with Foobar. If not, see <https://www.gnu.org/licenses/>.

namespace BrowserExtensionApp.Messages {
	public static class BrowserExtensionAppErrorType {
		public const string MessageParsingFailed = "https://docs.readup.com/errors/browser-extension-app/message-parsing-failed";
		public const string ReadupProtocolFailed = "https://docs.readup.com/errors/browser-extension-app/readup-protocol-failed";
		public const string UnexpectedMessageType = "https://docs.readup.com/errors/browser-extension-app/unexpected-message-type";
	}
	public static class GeneralErrorType {
		public const string Exception = "https://docs.readup.com/errors/general/exception";
	}
}