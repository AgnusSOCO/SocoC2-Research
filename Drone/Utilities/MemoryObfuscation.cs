using System;
using System.Runtime.InteropServices;

namespace Drone.Utilities;

public static class MemoryObfuscation
{
    [DllImport("kernel32.dll")]
    private static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

    private const uint PAGE_READWRITE = 0x04;
    private const uint PAGE_EXECUTE_READWRITE = 0x40;

    public static void ObfuscateMemoryRegion(IntPtr region, int size)
    {
        // Change memory protection to allow writing
        uint oldProtect;
        if (!VirtualProtect(region, (uint)size, PAGE_READWRITE, out oldProtect))
            throw new Exception("Failed to change memory protection");

        try
        {
            // XOR the memory region with a random key
            var key = GenerateRandomKey();
            var data = new byte[size];
            Marshal.Copy(region, data, 0, size);

            for (var i = 0; i < size; i++)
            {
                data[i] ^= key[i % key.Length];
            }

            Marshal.Copy(data, 0, region, size);
        }
        finally
        {
            // Restore original memory protection
            VirtualProtect(region, (uint)size, oldProtect, out _);
        }
    }

    public static void DeobfuscateMemoryRegion(IntPtr region, int size, byte[] key)
    {
        // Change memory protection to allow writing
        uint oldProtect;
        if (!VirtualProtect(region, (uint)size, PAGE_READWRITE, out oldProtect))
            throw new Exception("Failed to change memory protection");

        try
        {
            // XOR the memory region with the same key to restore original content
            var data = new byte[size];
            Marshal.Copy(region, data, 0, size);

            for (var i = 0; i < size; i++)
            {
                data[i] ^= key[i % key.Length];
            }

            Marshal.Copy(data, 0, region, size);
        }
        finally
        {
            // Restore original memory protection
            VirtualProtect(region, (uint)size, oldProtect, out _);
        }
    }

    private static byte[] GenerateRandomKey()
    {
        var key = new byte[32];
        var random = new Random();
        random.NextBytes(key);
        return key;
    }
}
