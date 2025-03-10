using System.Buffers;
using System.Text.Json;
using Booking.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Booking.Infrastructure.Caching
{
    public class CacheService(IDistributedCache cache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            byte[]? result = await cache.GetAsync(key, cancellationToken);
            return result is null ? default : Deserialize<T>(result);
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return cache.RemoveAsync(key, cancellationToken);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            byte[] bytes = Serialize(value);
            return cache.SetAsync(key, bytes, CacheOptions.Create(expiration), cancellationToken);
        }

        private static T Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes)!;
        }

        private static byte[] Serialize<T>(T value)
        {
            var buffer = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(buffer);
            JsonSerializer.Serialize(writer, value);
            return buffer.WrittenSpan.ToArray();
        }
    }
}
