using System;
using System.Runtime.InteropServices;
using Xunit;
using Drone.Utilities;

namespace Drone.Tests.Utilities;

public class MemoryObfuscationTests
{
    [Fact]
    public void ObfuscateMemoryRegion_ShouldModifyMemoryContent()
    {
        // Arrange
        var testData = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        var ptr = Marshal.AllocHGlobal(testData.Length);
        Marshal.Copy(testData, 0, ptr, testData.Length);

        // Act
        MemoryObfuscation.ObfuscateMemoryRegion(ptr, testData.Length);
        
        // Assert
        var result = new byte[testData.Length];
        Marshal.Copy(ptr, result, 0, testData.Length);
        Assert.NotEqual(testData, result);

        // Cleanup
        Marshal.FreeHGlobal(ptr);
    }

    [Fact]
    public void DeobfuscateMemoryRegion_ShouldRestoreOriginalContent()
    {
        // Arrange
        var testData = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        var key = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
        var ptr = Marshal.AllocHGlobal(testData.Length);
        Marshal.Copy(testData, 0, ptr, testData.Length);

        // Act
        MemoryObfuscation.ObfuscateMemoryRegion(ptr, testData.Length);
        MemoryObfuscation.DeobfuscateMemoryRegion(ptr, testData.Length, key);

        // Assert
        var result = new byte[testData.Length];
        Marshal.Copy(ptr, result, 0, testData.Length);
        Assert.Equal(testData, result);

        // Cleanup
        Marshal.FreeHGlobal(ptr);
    }
}
