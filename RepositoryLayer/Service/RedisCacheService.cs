
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RepositoryLayer.Service
{
    public class RedisCacheService
    {
        private readonly IDatabase _database;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _database = redis.GetDatabase();
            _logger = logger;
        }

        public void SetData(string key, string value, TimeSpan expiry)
        {
            _database.StringSet(key, value, expiry);
            _logger.LogInformation($"Data stored in Redis: Key = {key}, Expiry = {expiry.TotalMinutes} min");
        }

        public string? GetData(string key)
        {
            var data = _database.StringGet(key);
            _logger.LogInformation($"Fetching from Redis: Key = {key}, Exists = {!data.IsNullOrEmpty}, Value = {data}");
            return data;
        }

        public void RemoveData(string key)
        {
            _database.KeyDelete(key);
            _logger.LogInformation($"Deleted from Redis: Key = {key}");
        }
    }
}

