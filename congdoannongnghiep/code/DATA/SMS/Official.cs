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
        public long IdTinTuc { get; set; }
        public long IdList { get; set; }
        public string ImgLink0 { get; set; }
        public string TieuDe { get; set; }

        public string TomTat { get; set; }
        //loại
        public string NoiDung { get; set; }

        public string Ngay { get; set; }

        //lĩnh vực
        public string ProCode { get; set; }


        public int Viewed { get; set; }



    }

}
