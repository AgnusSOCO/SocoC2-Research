using System.Collections.Generic;

namespace TeamServer.C2Profiles;

public class MalleableProfile : C2Profile
{
    public JitterSettings Jitter { get; set; } = new();
    public SleepTechnique SleepTechnique { get; set; } = new();
    public Dictionary<string, string> HttpHeaders { get; set; } = new();
    public UserAgentSettings UserAgent { get; set; } = new();
    public PostExSettings PostEx { get; set; } = new();

    public class JitterSettings
    {
        public int MinValue { get; set; } = 10;
        public int MaxValue { get; set; } = 50;
        public bool EnableDynamicJitter { get; set; } = false;
        public int JitterChangeInterval { get; set; } = 300;
    }

    public class SleepTechnique
    {
        public SleepType Type { get; set; } = SleepType.Standard;
        public bool EnableSleepMask { get; set; } = false;
        public int SleepMaskInterval { get; set; } = 1000;
        public bool RandomizeSleepMask { get; set; } = false;
    }

    public class UserAgentSettings
    {
        public string[] UserAgents { get; set; } = new[] { "Mozilla/5.0" };
        public bool RandomizeUserAgent { get; set; } = false;
        public int UserAgentRotationInterval { get; set; } = 3600;
    }

    public class PostExSettings
    {
        public bool EnableProcessInjection { get; set; } = false;
        public string[] AllowedProcesses { get; set; } = new[] { "explorer.exe", "svchost.exe" };
        public bool EnableThreadHijacking { get; set; } = false;
        public bool EnableMemoryObfuscation { get; set; } = false;
    }

    public enum SleepType
    {
        Standard,
        Dynamic,
        Obfuscated,
        Hybrid
    }
}
