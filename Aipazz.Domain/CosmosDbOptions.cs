using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domain
{
    public class CosmosDbOptions
        {
            public string AccountEndpoint { get; set; } = "";
            public string AuthKey { get; set; } = "";
            public string DatabaseName { get; set; } = "";
            public Dictionary<string, string> Containers { get; set; } = new();
        }
    
}
