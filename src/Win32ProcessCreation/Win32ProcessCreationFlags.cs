using System;

namespace BrowserExtensionApp.Win32ProcessCreation {
	// https://docs.microsoft.com/en-us/windows/win32/procthread/process-creation-flags
	[Flags]
	public enum Win32ProcessCreationFlags : uint {
		None = 0,
		CreateBreakawayFromJob = 0x01000000,
		CreateNoWindow = 0x08000000
	}
}