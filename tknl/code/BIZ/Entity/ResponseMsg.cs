using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIZ.Entity
{
    [Serializable]
    public class ResponseMsg
    {
        public bool Success { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        
        // method
        public string ToJsonString ()
        {
            return UTILS.Utils.ConvertToJson ( this, string.Empty );
        }
    }
}
