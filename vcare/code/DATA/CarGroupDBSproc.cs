using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CarGroupDBSproc: CarGroupDBBase
    {
        public override int CreateUpdateCarGroup ( CarGroup CarGroup )
        {
            try
            {
                int? _id = CarGroup.Id;
                
               
                string description = CarGroup.Description;
                string _title = CarGroup.Name;
                string _description = CarGroup.Description;
                int? _status = CarGroup.Status;
                string _param = CarGroup.Config;
                string _url = CarGroup.Url;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_CarGroup_InsertUpdate(_id, _title, description, _status, _url,  _param);

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CarGroupDBSproc", "CreateUpdateCarGroup");
                return -1;
            }
        }
     
        public override CarGroup GetCarGroup ( int CarGroupId )
        {
            var select = "*";
            var where = "Id = " + CarGroupId;
            var orderBy = string.Empty;

            var results = GetCarGroupsDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }
        public override IEnumerable<CarGroup> GetTopCarGroups(int status)
        {
            var select = "*";
            var where = "";
            if (status > -1)
                where = "Status=" + status;
            var orderBy = string.Empty;

            var results = GetCarGroupsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results;
        }
        public override IEnumerable<CarGroup> GetCarGroupsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_CarGroup_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CarGroupDBSproc", "GetCarGroupsDyn: select" + select);
                return null;
            }
        }

      

        
        public override int DeleteCarGroupDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_CarGroup_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "DeleteCarGroupDyn " + where);
                return -1;
            }
        }
       

        public override int DeleteCarGroup ( int CarGroupId ) { var where = "Id =" + CarGroupId; return DeleteCarGroupDyn ( where ); }
        

    }
}
