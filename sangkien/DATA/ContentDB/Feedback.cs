using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.ContentDB
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ResponedTime { get; set; }
        public string Email { get; set; }
        public string ResponedUser { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
        public string Mobile { get; set; }
        public int Status { get; set; }
    }
}
