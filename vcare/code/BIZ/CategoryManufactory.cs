using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class CategoryManufactoryBO
    {
      

        #region CREATE UPDATE

        public int CreateUpdateCategoryManufactory(int cateid, int manuid)
        {
            var manuFactory = new CategoryManufactory
            {
                CategoryId= cateid,
                ManufactoryId= manuid
            };
            return CategoryManufactoryDBBase.Create().CreateUpdateCategoryManufactory(manuFactory);
        }



        #endregion

        #region READ


        public List<CategoryManufactory> GetByManuId(int Cateid)
        {
            var listCategoryManufactory = CategoryManufactoryDBBase.Create().GetByManuId(Cateid);
            if (listCategoryManufactory == null)
                return null;
            return listCategoryManufactory.ToList();
        }


        public List<CategoryManufactory> GetByCateId(int Cateid)
        {
            var listCategoryManufactory = CategoryManufactoryDBBase.Create().GetByCateId(Cateid);
            if (listCategoryManufactory == null)
                return null;
            return listCategoryManufactory.ToList();
        }

        

        #endregion

        #region DELETE

        public int DeleteCategoryManufactory(int manuFactoryId)
        {
            int returnVal = CategoryManufactoryDBBase.Create().DeleteById(manuFactoryId);
           
            return returnVal;
        }

        public int DeleteManufactories(string listId)
        {
            var returnVal = CategoryManufactoryDBBase.Create().DeleteManufactories(listId);
           
            return returnVal;
        }

        #endregion

      

        
    }
}
