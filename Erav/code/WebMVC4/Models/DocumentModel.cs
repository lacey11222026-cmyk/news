using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ.Entity;
using DATA;

namespace WebMVC4.Models
{
    public class DocumentModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<DOCUMENT_FULL> listdata { get; set; }
        public int pageIndex { get; set; }

        public List<CATEGORY_FULL>subcate { get; set; }
    }
    public class MissionModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int Year { get; set; }
        public int CreatedBy { get; set; }
        public int pageSize { get; set; }
        public List<MISSION_FULL> listdata { get; set; }
        public int pageIndex { get; set; }
        public string keyword { get; set; }


    }
    public class ProjectModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<Project> listdata { get; set; }
        public int pageIndex { get; set; }

      
    }
    public class TechProcessModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<TechProcess> listdata { get; set; }
        public int pageIndex { get; set; }


    }
    public class NhaMayModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<NhaMay> listdata { get; set; }
        public int pageIndex { get; set; }

        public int status { get; set; }
        public int loai { get; set; }
        public int hinhthuc { get; set; }
        public string fromdate { get; set; }

        public string todate { get; set; }
    }
    
}