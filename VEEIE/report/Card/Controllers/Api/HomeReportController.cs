using Car.CMS.Models;
using Car.Data.Service;
using Newtonsoft.Json;
using SMS.Data.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Car.CMS.Controllers.Api
{
   
    public class HomeReportController : ApiController
    {
        
        [System.Web.Http.HttpGet]
        public CONTENT_API GetResult()
        {
            var result = new CONTENT_API { Money = 0, Total1 = 0, Total2 = 0 };
            var data = AbstractDAOFactory.Instance().PlansService().GetList("", -1, -1, 1).Where(x => x.Type > 0).ToList();
            result.Year = data.FirstOrDefault().Year.GetValueOrDefault();
            result.Type = data.FirstOrDefault().Type.GetValueOrDefault();

            data = data.Where(x => x.Year == result.Year && x.Type == result.Type).ToList();

           
            //result.Total1 = data.Sum(x => x.Total1);
            //result.Total2 = data.Sum(x => x.Total2);
            
            foreach(var itemReport in data)
            {
                var lstPlanItem = AbstractDAOFactory.Instance().PlanItemsService().GetList(itemReport.Id).Where(x => x.Status == 1).ToList();
                foreach (var item in lstPlanItem)
                {
                    var Item1 = JsonConvert.DeserializeObject<PlanItemData>(item.Config1);
                    result.Money += Item1.Money + Item1.Balance;
                    result.Total1 += item.Total1.GetValueOrDefault();
                    result.Total2 += item.Total2.GetValueOrDefault();
                }
            }
            return result;
        }
        public class CONTENT_API
        {
            public int Year
            {
                get;
                set;
            }
            public int Type
            {
                get;
                set;
            }
            public double Total1
            {
                get;
                set;
            }
            public double Total2
            {
                get;
                set;
            }
            public long Money
            {
                get;
                set;
            }
            
        }
    }
}
