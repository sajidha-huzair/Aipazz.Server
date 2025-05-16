namespace Aipazz.Domian.client
{
    public class Client
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? nic { get; set; } // Partition key
        public string? phone { get; set; }
        public string? email { get; set; }
    }
}