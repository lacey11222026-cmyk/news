using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CarModelDBSproc: CarModelDBBase
    {
        public override int CreateUpdateCarModel ( CarModel CarModel )
        {
            try
            {
                int? _id = CarModel.Id;
                
               
                string description = CarModel.Description;
                string _title = CarModel.Name;
                string _description = CarModel.Description;
                int? _status = CarModel.Status;
                int? _groupId = CarModel.GroupId;
                int? _order = CarModel.Order;
                string _param = CarModel.Config;
                string _url = CarModel.Url;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_CarModel_InsertUpdate(_id, _title, description, _status, _order, _groupId, _url,  _param);

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CarModelDBSproc", "CreateUpdateCarModel");
                return -1;
            }
        }
     
        public override CarModel GetCarModel ( int CarModelId )
        {
            var select = "*";
            var where = "Id = " + CarModelId;
            var orderBy = string.Empty;

            var results = GetCarModelsDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }
        public override CarModel GetByUrl(string url)
        {
            var select = "*";
            var where = $"Url = '{url}'";
            var orderBy = string.Empty;

            var results = GetCarModelsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public override IEnumerable<CarModel> GetTopCarModels(int groupId,int status)
        {
            var select = "*";
            var where = "";
            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Status=" + status;
            }
            if (groupId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "GroupId=" + groupId;
            }
            var orderBy = "[Order] ASC";

            var results = GetCarModelsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results;
        }
        public override IEnumerable<CarModel> GetCarModelsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_CarModel_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CarModelDBSproc", "GetCarModelsDyn: select" + select);
                return null;
            }
        }

      

        
        public override int DeleteCarModelDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_CarModel_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "DeleteCarModelDyn " + where);
                return -1;
            }
        }
        public override int UpdateCarModelDyn(string update, string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_CarModel_UpdateDynamic(update,where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "UpdateCarModelDyn " + update + where);
                return -1;
            }
        }

        public override int DeleteCarModel ( int CarModelId ) { var where = "Id =" + CarModelId; return DeleteCarModelDyn ( where ); }
        

    }
}
