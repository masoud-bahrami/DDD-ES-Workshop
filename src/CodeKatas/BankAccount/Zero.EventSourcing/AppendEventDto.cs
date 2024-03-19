using System.Text.Json.Serialization;

namespace Zero.EventSourcing
{
    public class AppendEventDto
    {
        public string GlobalUniqueEventId { get; set; }
        public Guid GlobalUniqueEventIdGuid { get; set; }
        public string EventType { get; set; }
        public int Version { get; set; }
        public string Metadata { get; set; }
        public string Payload { get; set; }

        [JsonConstructor]
        public AppendEventDto()
        {

        }

        public static AppendEventDto Version1(string globalUniqueEventId, object payload)
        {

            return new()
            {
                GlobalUniqueEventIdGuid = GenerateUniqueIdGuid(),
                GlobalUniqueEventId = globalUniqueEventId,
                EventType = payload.GetType().AssemblyQualifiedName,
                Metadata = "",
                Payload = ToJson(payload),
                Version = 1
            };
        }


        public static AppendEventDto Version1(object payload) =>
            new()
            {
                GlobalUniqueEventId = GenerateUniqueId(),
                GlobalUniqueEventIdGuid = GenerateUniqueIdGuid(),
                EventType = payload.GetType().AssemblyQualifiedName,
                Metadata = "",
                Payload = ToJson(payload),
                Version = 1
            };
        private static string ToJson(object payload) => Newtonsoft.Json.JsonConvert.SerializeObject(payload);

        private static string GenerateUniqueId()
            => Guid.NewGuid().ToString();
        private static Guid GenerateUniqueIdGuid()
            => Guid.NewGuid();
    }
}