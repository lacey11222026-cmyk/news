using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA.SMS
{
    public class SMSLog
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Admin { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string PartnerCode { get; set; }
        public int Status { get; set; }
    }
}
