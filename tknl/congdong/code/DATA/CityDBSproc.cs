using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CityDBSproc: CityDBBase
    {


        public override int CreateUpdateCity(City manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;

                string _name = manufactory.Name;
               
                string _nameen = manufactory.NameEn;
                
                string _url = manufactory.Url;
               
                int? _type = manufactory.Type;
                int? _status = manufactory.Status;
                int? _order = manufactory.OrderSort;

                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.sp_City_InsertUpdate(_id, _type, _nameen,_url, _status, _order);
                    return responecode.GetValueOrDefault();
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ShopDBSproc", "CreateUpdateShop");
                return -1;
            }
        }
        public override IEnumerable<City> GetTopLastest(int top, int? published,int type)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            
            if (published >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + published;
            }
            if (type >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Type = " + type;
            }

            var orderBy = "[OrderSort] ASC, Name ASC";

            return GetCitysDyn(select, where, orderBy);
        }
        public override City GetCity(int id)
        {
            var select = " *";
           
            var where = "Id = "+ id;

           

            var orderBy = "";

            return GetCitysDyn(select, where, orderBy).FirstOrDefault(); ;
        }
        public override IEnumerable<City> GetCitysDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_City_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CityDBSproc", "GetCitysDyn");
                return null;
            }
        }

     
      

      

        
    }
}
