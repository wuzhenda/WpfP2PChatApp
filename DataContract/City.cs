using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class City
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public long CountryID { get; set; }
        public long Population { get; set; }
        public string Province { get; set; }
    }

    public class ReduceCity
    {
        public string Name { get; set; }
        public long Population { get; set; }
        public string Province { get; set; }
        public long CountryID { get; set; }
        public long Count { get; set; }
        public List<SummeryCity> Summery { get; set; }
    }

    public class SummeryCity
    {
        public string Name { get; set; }
        public long Population { get; set; }
        public string Province { get; set; }
    }
}
