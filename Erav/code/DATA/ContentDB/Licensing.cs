using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA.DocumentDB
{
    public class Licensing
    {
        public int Id { get; set; }
        public string LicenseNo { get; set; }
        public int AreaId { get; set; }
        public int LicensingStatus { get; set; }

        public DateTime ExpiredLicensing { get; set; }
        public DateTime DateofIssue { get; set; }

        public string AttachmentLicensing { get; set; }

        public string OrgName { get; set; }

        public string Address { get; set; }



    }
}
