using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA.DocumentDB
{
    public class Official
    {
        public long IdVanBan { get; set; }
        public long IdList { get; set; }
        public string MaSo { get; set; }
        public string TieuDe { get; set; }

        public string TomTat { get; set; }
        //loại
        public int IdType { get; set; }

        public string FileLink { get; set; }

        //lĩnh vực
        public int IdCoquan { get; set; }


        public string Ngay { get; set; }



    }
    public class Official2
    {
        public long id { get; set; }
      
        public string title { get; set; }
        public string image { get; set; }

        public string content { get; set; }


        public string description { get; set; }
        public int category_id { get; set; }


        public int views { get; set; }


        public DateTimeOffset created_at { get; set; }
      
        public DateTimeOffset publish_date { get; set; }

    }

}
