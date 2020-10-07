using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectorFS
{
    public class Record
    {
        public string BarCode { get; set; }
        public string Quantity { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string ExternalIdCode { get; set; }
        public string BatchNumber { get; set; }
    }
}
