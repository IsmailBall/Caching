using StackExchange.Redis;

namespace Caching.CachingWithStackExchange.Services
{
    public class RedisService
    {

        private readonly string _redisConnectionHost;
        private readonly string _redisConnectionPort;
        private ConnectionMultiplexer _redis;

        public IDatabase Database { get; private set; }

        public RedisService(IConfiguration configuration)
        {
            _redisConnectionHost = configuration["Redis:Host"];
            _redisConnectionPort = configuration["Redis:Port"];
        }

        public void Connect()
        {
            var connectionString = $"{_redisConnectionHost}:{_redisConnectionPort}";
            _redis = ConnectionMultiplexer.Connect(connectionString);
        }

        public IDatabase GetDatabase(int db)
        {
            Database = _redis.GetDatabase(db);
            return Database;
        }
    }
}
