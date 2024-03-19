namespace Zero.EventSourcing.EventStoreDb
{
    public class EventStoreDbConfig
    {
        public string Protocol = "esdb+discover";
        public string Url = "localhost";
        public string Port = "2113";
        public bool Tls = false;
        public int KeepAliveTimeout = 10000;
        public int KeepAliveInterval = 10000;
        public string ConnectionName { get; set; }

        public string GetConnectionString()
        {
            return $"{Protocol}://{Url}:{Port}?tls={Tls.ToString().ToLower()}&keepAliveTimeout={KeepAliveTimeout}&keepAliveInterval={KeepAliveInterval}";
        }
    }
}