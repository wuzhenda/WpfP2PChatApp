using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class Country
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long Area { get; set; }
        public string Capital { get; set; }
        public string Province { get; set; }
        public string CountinentCode { get; set; }
    }
}
