using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_explorer
{
     public class DBSmartExplorer
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string NumberOps { get; set; }
        public string StatusConnection { get; set; }
        public string IpConnection { get; set; }
        public DateTime DateTimeAction { get; set; }
    }
}
