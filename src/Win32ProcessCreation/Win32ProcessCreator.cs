using System;
using System.Runtime.InteropServices;

namespace BrowserExtensionApp.Win32ProcessCreation {
	public class Win32ProcessCreator {
		[StructLayout(LayoutKind.Sequential)]
		struct PROCESS_INFORMATION {
			public IntPtr hProcess;
			public IntPtr hThread;
			public int dwProcessId;
			public int dwThreadId;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct SECURITY_ATTRIBUTES {
			public int nLength;
			public IntPtr lpSecurityDescriptor;
			public int bInheritHandle;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		struct STARTUPINFO {
			public Int32 cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public Int32 dwX;
			public Int32 dwY;
			public Int32 dwXSize;
			public Int32 dwYSize;
			public Int32 dwXCountChars;
			public Int32 dwYCountChars;
			public Int32 dwFillAttribute;
			public Int32 dwFlags;
			public Int16 wShowWindow;
			public Int16 cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool CreateProcess(
			string lpApplicationName,
			string lpCommandLine,
			ref SECURITY_ATTRIBUTES lpProcessAttributes,
			ref SECURITY_ATTRIBUTES lpThreadAttributes,
			bool bInheritHandles,
			uint dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			[In] ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation
		);

		public static bool CreateProcess(
			string commandLine,
			Win32ProcessCreationFlags processCreationFlags = Win32ProcessCreationFlags.None
		) {
			var processAttributes = new SECURITY_ATTRIBUTES();
			processAttributes.nLength = Marshal.SizeOf(processAttributes);
			var threadAttributes = new SECURITY_ATTRIBUTES();
			threadAttributes.nLength = Marshal.SizeOf(threadAttributes);

			var startupInfo = new STARTUPINFO();
			startupInfo.cb = Marshal.SizeOf(startupInfo);

			PROCESS_INFORMATION processInfo;

			return CreateProcess(
				lpApplicationName: null,
				lpCommandLine: commandLine,
				lpProcessAttributes: ref processAttributes,
				lpThreadAttributes: ref threadAttributes,
				bInheritHandles: false,
				dwCreationFlags: (uint)processCreationFlags,
				lpEnvironment: IntPtr.Zero,
				lpCurrentDirectory: null,
				lpStartupInfo: ref startupInfo,
				lpProcessInformation: out processInfo
			);
		}
	}
}