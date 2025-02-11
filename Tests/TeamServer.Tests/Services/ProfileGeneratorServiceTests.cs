using System;
using System.Threading.Tasks;
using Xunit;
using TeamServer.Services;
using TeamServer.C2Profiles;

namespace TeamServer.Tests.Services;

public class ProfileGeneratorServiceTests
{
    private readonly ProfileGeneratorService _service;

    public ProfileGeneratorServiceTests()
    {
        _service = new ProfileGeneratorService();
    }

    [Fact]
    public async Task GenerateRandomProfile_ShouldCreateValidProfile()
    {
        // Act
        var profile = await _service.GenerateRandomProfile();

        // Assert
        Assert.NotNull(profile);
        Assert.NotNull(profile.Name);
        Assert.NotNull(profile.Http);
        Assert.NotNull(profile.Jitter);
        Assert.NotNull(profile.SleepTechnique);
        Assert.NotNull(profile.HttpHeaders);
        Assert.NotNull(profile.UserAgent);
        Assert.NotNull(profile.PostEx);
    }

    [Fact]
    public async Task GenerateRandomProfile_ShouldCreateUniqueProfiles()
    {
        // Act
        var profile1 = await _service.GenerateRandomProfile();
        var profile2 = await _service.GenerateRandomProfile();

        // Assert
        Assert.NotEqual(profile1.Name, profile2.Name);
        Assert.NotEqual(profile1.Http.GetPaths, profile2.Http.GetPaths);
        Assert.NotEqual(profile1.Http.PostPaths, profile2.Http.PostPaths);
    }
}
