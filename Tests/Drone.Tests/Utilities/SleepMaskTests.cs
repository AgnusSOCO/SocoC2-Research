using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Drone.Utilities;

namespace Drone.Tests.Utilities;

public class SleepMaskTests
{
    [Fact]
    public async Task ObfuscatedSleep_ShouldSleepForRequestedDuration()
    {
        // Arrange
        var duration = 1000;
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        await SleepMask.ObfuscatedSleep(duration);
        stopwatch.Stop();

        // Assert
        // Allow for some timing variance
        Assert.InRange(stopwatch.ElapsedMilliseconds, duration * 0.9, duration * 1.1);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task ObfuscatedSleep_ShouldWorkWithDifferentDurations(int duration)
    {
        // Arrange
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        await SleepMask.ObfuscatedSleep(duration);
        stopwatch.Stop();

        // Assert
        Assert.InRange(stopwatch.ElapsedMilliseconds, duration * 0.9, duration * 1.1);
    }
}
