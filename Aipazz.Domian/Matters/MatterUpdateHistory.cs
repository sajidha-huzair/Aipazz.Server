using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Matters
{
    public class MatterUpdateHistory
    {
        public string id { get; set; }
        public string MatterId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ChangeDescription { get; set; }
        public string UserId { get; set; }
        public string ClientNic { get; set; }

    }
}
