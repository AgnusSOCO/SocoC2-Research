using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drone.Utilities;

public static class ChunkingUtility
{
    public static IEnumerable<byte[]> ChunkData(byte[] data, int chunkSize)
    {
        for (var i = 0; i < data.Length; i += chunkSize)
        {
            var chunk = new byte[Math.Min(chunkSize, data.Length - i)];
            Buffer.BlockCopy(data, i, chunk, 0, chunk.Length);
            yield return chunk;
        }
    }

    public static int GetRandomChunkSize(int minSize = 512, int maxSize = 4096)
    {
        var random = new Random();
        return random.Next(minSize, maxSize + 1);
    }

    public static async Task DelayBetweenChunks(int minDelay = 10, int maxDelay = 100)
    {
        var random = new Random();
        await Task.Delay(random.Next(minDelay, maxDelay + 1));
    }
}
