using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car.Data.Api
{
    [Serializable]
    public class ResponeData
    {
        public int ResponseCode { get; set; }
        public string Description { get; set; }
       
        public string Signature { get; set; }
    }

    
}
