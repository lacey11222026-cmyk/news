using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.ContentDB
{

    public class UserProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }
        public string Config { get; set; }
        public string Organ { get; set; }
        public string Location { get; set; }
        public string Unit { get; set; }
        public string UnitIInfo { get; set; }
        public string Total { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }

        public int ViewCount { get; set; }
        public int SubType { get; set; }
        public string Currency { get; set; }

        public string Detail { get; set; }
        public string Source { get; set; }
        public int Progress { get; set; }
        public string LegalStatus { get; set; }
        public string Impact { get; set; }
        public string Document { get; set; }
        public string Rule1 { get; set; }
        public string Rule2 { get; set; }
        public string Rule3 { get; set; }
        public string Rule4 { get; set; }
        public string Username { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime SendTime { get; set; }

    }
    public class Project2
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string Config { get; set; }
        public string Organ { get; set; }
        public string Location { get; set; }
        public string Unit { get; set; }
        public string UnitIInfo { get; set; }
        public string Total { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }

        public int ViewCount { get; set; }
        public int SubType { get; set; }
        public string Currency { get; set; }

        public string Detail { get; set; }
        public string Source { get; set; }
        public int Progress { get; set; }
        public string LegalStatus { get; set; }
        public string Impact { get; set; }
        public string Document { get; set; }
        public string Rule1 { get; set; }
        public string Rule2 { get; set; }
        public string Rule3 { get; set; }
        public string Rule4 { get; set; }
        public string Username { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime SendTime { get; set; }

    }
    public class Project2Full : Project2
    {
        public UserProjectConfig ProjectConfig { get; set; }

    }
    public class Project2FullV2
    {
        public UserProjectConfig ProjectConfig { get; set; }
        public Project2 Project { get; set; }
    }
    public class UserProjectFull: UserProject
    {
        public UserProjectConfig ProjectConfig { get; set; }
       
    }
    public class UserProjectConfig
    {

        public string Time { get; set; }
        public string Finish { get; set; }
        public string Support { get; set; }
        public string UnitDev { get; set; }
        public string Revenue { get; set; }

        public string Finance { get; set; }

        public int TA { get; set; }
        public string TADetail { get; set; }
        public string Rate { get; set; }


        public string Role { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Fullname { get; set; }
    }
}
