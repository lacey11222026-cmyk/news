using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.ContentDB
{
    public class Auditor
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string No { get; set; }
        public string FullName { get; set; }
        public string BirthDay { get; set; }
        public string Passport { get; set; }
        public string Nation { get; set; }
        public int Status { get; set; }
        public int Order { get; set; }
        public string Level { get; set; }
        public string Organ { get; set; }
        public string MSDN { get; set; }
        public string Role { get; set; }
        public string Config { get; set; }

        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Group { get; set; }
        public int Cate { get; set; }

        public string Images { get; set; }
        public string Province { get; set; }

    }
    public class AuditorFull: Auditor
    {
        public AuditorConfig ProjectConfig { get; set; }
        public Auditor Auditor { get; set; }
    }
    public class AuditorConfig
    {

        public string MobileOffice { get; set; }
        public string TrainingTime { get; set; }

        public string IssueDate { get; set; }
        public string ExpirationDate { get; set; }

       

    }
}
