using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA.DocumentDB
{
    public class DocumentHome
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Status { get; set; }
        public string SignedBy { get; set; }
        public string SignedByDesc { get; set; }
        public string FilePath { get; set; }
    }
}
