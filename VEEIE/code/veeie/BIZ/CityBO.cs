using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using City = DATA.City;
namespace BIZ
{
    public class CityBO
    {
       

        #region CREATE
        public int CreateUpdateCity(City City)
        {
            
            int returnVal = CityDBBase.Create().CreateUpdateCity(City);
          
            return returnVal;
        }
       
       
        #endregion

        #region READ


        public City GetCity(int CityId)
        {
            try
            {
                return CityDBBase.Create().GetCity(CityId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "CityBO", "GetCity");
                return null;
            }
        }


        public List<City> GetTopCity(int top,int status,int type)
        {
            var data = CityDBBase.Create().GetTopLastest(top, status,type);
            if (data == null)
                return null;

            return data.ToList();
        }

      
       

        #endregion



        

      
    }
}
