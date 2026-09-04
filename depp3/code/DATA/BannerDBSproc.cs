using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class BannerDBSproc: BannerDBBase
    {
        public override int CreateUpdateBanner ( Banner Banner )
        {
            try
            {
                int? _id = Banner.Id;
                
                int? _region = Banner.Region;
                int? _oder = Banner.Order;
                string _url = Banner.Url;
                string _data = Banner.Data;
                string _name = Banner.Name;
                string _description = Banner.Description;
                byte? _status = Banner.Status;
                byte? _type = Banner.Type;
                
                

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Banner_InsertUpdate(_id, _name,_data, _description, _region,_status, _type, _url,_oder);

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "BannerDBSproc", "CreateUpdateBanner");
                return -1;
            }
        }
        public override IEnumerable<Banner> GetTopLastestBanners(int top, int region,int status,int type)
        {
            var select = " Id,Name,Url,Data,[Order],Region,[Type],Status,Description";
            if (top > 1)
                select = "TOP(" + top + ") Id,Name,Url,Data,[Order],Region,[Type],Status,Description";
            var where = String.Empty;
            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status=" + status.ToString();
            }
            if (region > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Region=" +region.ToString() ;
            }
            if (type > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " [Type]=" + type.ToString();
            }
            var orderBy = "[Order] ASC";

            return GetBannersDyn(select, where, orderBy);
        }
        public override Banner GetBanner ( int BannerId )
        {
            var select = "*";
            var where = "Id = " + BannerId;
            var orderBy = string.Empty;

            var results = GetBannersDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<Banner> GetBannersDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Banner_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "BannerDBSproc", "GetBannersDyn: select" + select);
                return null;
            }
        }
                

        public override int DeleteBannerDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Banner_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "DeleteBannerDyn");
                return -1;
            }
        }

        public override int DeleteBanner ( int BannerId ) { var where = "Id =" + BannerId; return DeleteBannerDyn ( where ); }
        public override int DeleteBanners ( string lstBannerIds ) { var where = "Id IN (" + lstBannerIds + ")"; return DeleteBannerDyn ( where ); }


    }
}
