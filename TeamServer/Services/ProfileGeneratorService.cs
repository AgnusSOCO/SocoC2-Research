using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamServer.C2Profiles;

namespace TeamServer.Services;

public class ProfileGeneratorService
{
    private static readonly string[] CommonUserAgents = new[]
    {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Edge/120.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:120.0) Gecko/20100101 Firefox/120.0"
    };

    private static readonly string[] CommonHeaders = new[]
    {
        "Accept",
        "Accept-Language",
        "Accept-Encoding",
        "Cache-Control",
        "Connection",
        "DNT",
        "Upgrade-Insecure-Requests"
    };

    private static readonly string[] CommonPaths = new[]
    {
        "/api/v1/data",
        "/rest/2.0/feed",
        "/cdn/content",
        "/static/resources",
        "/assets/data"
    };

    public async Task<MalleableProfile> GenerateRandomProfile()
    {
        return await Task.Run(() =>
        {
            var profile = new MalleableProfile
            {
                Name = $"profile_{DateTime.UtcNow.Ticks}",
                Jitter = GenerateRandomJitterSettings(),
                SleepTechnique = GenerateRandomSleepTechnique(),
                HttpHeaders = GenerateRandomHeaders(),
                UserAgent = GenerateRandomUserAgentSettings(),
                PostEx = GenerateRandomPostExSettings(),
                Http = new C2Profile.HttpProfile
                {
                    Sleep = Random.Shared.Next(30, 120),
                    Jitter = Random.Shared.Next(10, 50),
                    GetPaths = GenerateRandomPaths(),
                    PostPaths = GenerateRandomPaths()
                }
            };

            return profile;
        });
    }

    private MalleableProfile.JitterSettings GenerateRandomJitterSettings()
    {
        return new MalleableProfile.JitterSettings
        {
            MinValue = Random.Shared.Next(5, 20),
            MaxValue = Random.Shared.Next(30, 60),
            EnableDynamicJitter = Random.Shared.Next(2) == 1,
            JitterChangeInterval = Random.Shared.Next(60, 600)
        };
    }

    private MalleableProfile.SleepTechnique GenerateRandomSleepTechnique()
    {
        return new MalleableProfile.SleepTechnique
        {
            Type = (MalleableProfile.SleepType)Random.Shared.Next(4),
            EnableSleepMask = Random.Shared.Next(2) == 1,
            SleepMaskInterval = Random.Shared.Next(500, 2000),
            RandomizeSleepMask = Random.Shared.Next(2) == 1
        };
    }

    private Dictionary<string, string> GenerateRandomHeaders()
    {
        var headers = new Dictionary<string, string>();
        var headerCount = Random.Shared.Next(3, 7);

        for (var i = 0; i < headerCount; i++)
        {
            var header = CommonHeaders[Random.Shared.Next(CommonHeaders.Length)];
            if (!headers.ContainsKey(header))
            {
                headers.Add(header, GenerateRandomHeaderValue());
            }
        }

        return headers;
    }

    private MalleableProfile.UserAgentSettings GenerateRandomUserAgentSettings()
    {
        return new MalleableProfile.UserAgentSettings
        {
            UserAgents = GetRandomSubset(CommonUserAgents, Random.Shared.Next(1, 4)),
            RandomizeUserAgent = Random.Shared.Next(2) == 1,
            UserAgentRotationInterval = Random.Shared.Next(1800, 7200)
        };
    }

    private MalleableProfile.PostExSettings GenerateRandomPostExSettings()
    {
        return new MalleableProfile.PostExSettings
        {
            EnableProcessInjection = Random.Shared.Next(2) == 1,
            AllowedProcesses = GetRandomSubset(new[] { "explorer.exe", "svchost.exe", "RuntimeBroker.exe" }, Random.Shared.Next(1, 4)),
            EnableThreadHijacking = Random.Shared.Next(2) == 1,
            EnableMemoryObfuscation = Random.Shared.Next(2) == 1
        };
    }

    private string[] GenerateRandomPaths()
    {
        return GetRandomSubset(CommonPaths, Random.Shared.Next(2, 4));
    }

    private string GenerateRandomHeaderValue()
    {
        // Implementation details omitted for brevity
        return "value";
    }

    private T[] GetRandomSubset<T>(T[] source, int count)
    {
        var result = new T[count];
        var indices = new HashSet<int>();
        var random = Random.Shared;

        while (indices.Count < count)
        {
            indices.Add(random.Next(source.Length));
        }

        var i = 0;
        foreach (var index in indices)
        {
            result[i++] = source[index];
        }

        return result;
    }
}
