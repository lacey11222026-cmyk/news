using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Part = DATA.Part;
namespace BIZ
{
    public class PartBO
    {
       

        #region CREATE
        public int CreateUpdatePart(Part Part)
        {
            
            int returnVal = PartDBBase.Create().CreateUpdatePart(Part);
          
            return returnVal;
        }
       
        #endregion

        #region READ


        public Part GetPart(int PartId)
        {
            try
            {
                return PartDBBase.Create().GetPart(PartId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PartBO", "GetPart");
                return null;
            }
        }

        public List<Part> GetSearchParts(int published, string code)
        {
            var data = PartDBBase.Create().GetAllParts(published, code);

            return data?.ToList();
        }


        public List<Part> GetPartsPaged(int pageIndex, int pageSize, ref int totalRecords, int published,string code)
        {
            var data = PartDBBase.Create().GetAllPartsPaged(published,code,pageIndex, pageSize, ref  totalRecords);
            if (data == null)
                return null;

            return data.ToList();
        }
       
       

        #endregion



        #region DELETE

       

        public int DeletePart(int id)
        {
            var returnVal = PartDBBase.Create().DeletePart(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
