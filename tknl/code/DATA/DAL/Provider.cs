using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.DAL
{
    public  class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public string Address { get; set; }
        public string Images { get; set; }
        public string MST { get; set; }
        public string Year { get; set; }
        public int Cate { get; set; }

        public string Represent { get; set; }
        
        public string Business { get; set; }
        public string Office { get; set; }
        public string Contact { get; set; }
        public string Province { get; set; }
    }
}
