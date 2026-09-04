using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA.DocumentDB
{
    //System.Guid
    public class Official
    {
      
        public string So_hieu { get; set; }
        public string Trich_yeu { get; set; }

      
       

        public DateTime Ngay_ban_hanh { get; set; }
        public DateTime Ngay_co_hieu_luc { get; set; }


        //loại
        public string Hinh_thuc_van_ban { get; set; }
        //lĩnh vực
        public string Linh_vuc { get; set; }

        //cơ quan ban hành
        public string Co_quan_ban_hanh { get; set; }


        public string Nguoi_ky_duyet { get; set; }
        public string DocumentTypeName { get; set; }



    }
  
}
