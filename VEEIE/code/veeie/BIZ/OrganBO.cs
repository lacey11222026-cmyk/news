using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Organ = DATA.Organ;
namespace BIZ
{
    public class OrganBO
    {
       

        #region CREATE
        public int CreateUpdateOrgan(Organ Organ)
        {
            
            int returnVal = OrganDBBase.Create().CreateUpdateOrgan(Organ);
          
            return returnVal;
        }
        public int UpdateStatus(int OrganId)
        {
            try
            {
                return OrganDBBase.Create().UpdateStatus(OrganId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "OrganBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int OrganId, bool upOrder)
        {
            try
            {
                return OrganDBBase.Create().UpdateOrder(OrganId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "OrganBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public Organ GetOrgan(int OrganId)
        {
            try
            {
                return OrganDBBase.Create().GetOrgan(OrganId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "OrganBO", "GetOrgan");
                return null;
            }
        }


        public List<Organ> GetTopOrgan(int top,int type)
        {
            var data = OrganDBBase.Create().GetTopLastest(top,type);
            if (data == null)
                return null;

            return data.ToList();
        }

        public List<Organ> GetOrgansPaged(string keyword, int pageIndex, int pageSize, ref int totalRecords, int? published,int type, string lang)
        {
            var data = OrganDBBase.Create().GetAllPaged(keyword,pageIndex, pageSize, ref  totalRecords, published, type,lang);
            if (data == null)
                return null;

            return data.ToList();
        }
       
       

        #endregion



        #region DELETE

       

        public int DeleteOrgan(int id)
        {
            var returnVal = OrganDBBase.Create().DeleteOrgan(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
