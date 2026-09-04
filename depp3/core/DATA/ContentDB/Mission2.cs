using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.ContentDB
{
    public class Mission2
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string Organ { get; set; }
        public int Status { get; set; }

        public int FromDate { get; set; }
        public int ToDate { get; set; }
        public int Result { get; set; }
        public int Accept { get; set; }
    }
}
