using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA.DocumentDB
{
    //System.Guid
    public class Official
    {
        public int ID { get; set; }
        public int CateOfficialID { get; set; }
        public string NoCode { get; set; }
        public string OfficialName { get; set; }

        public string Writer { get; set; }
        //loại
        public int DocTypeId { get; set; }

        public DateTime DatePublic { get; set; }

        //lĩnh vực
        public int AreaId { get; set; }

        //cơ quan ban hành
        public int OrgId { get; set; }


        public DateTime? EffectDate { get; set; }
        //ngày hết hiệu lực
        public DateTime? ExpiredDate { get; set; }



        public string Quote { get; set; }



    }
    public class OfficialFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        

    }
}
