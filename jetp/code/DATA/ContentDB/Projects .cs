using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.ContentDB
{

    public class Projects
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Config { get; set; }
        public string Contact { get; set; }
        public string ProcessTime { get; set; }
        public string StartTime { get; set; }
        public string Result { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int SystemType { get; set; }
        public int Order { get; set; }

        public string Target { get; set; }
        public string Region { get; set; }
        public string ProposedCapital { get; set; }
        public string Capital { get; set; }
        public string Language { get; set; }
        public string MinimumTaget { get; set; }
        public string Sponsor { get; set; }

        public string SupportGroup { get; set; }



    }
    public class ProjectFull
    {
        public ProjectConfig ProjectConfig { get; set; }
        public Projects Project { get; set; }
    }
    public class ProjectConfig
    {

        public string Name1 { get; set; }
        public string Path1 { get; set; }
        public string Name2 { get; set; }
        public string Path2 { get; set; }
        public string Name3 { get; set; }
        public string Path3 { get; set; }

    }
}
