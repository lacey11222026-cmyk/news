using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class ChannelDBSproc: ChannelDBBase
    {
        public override int CreateUpdateChannel ( Channel Channel )
        {
            try
            {
                int? _id = Channel.Id;
                long? _contentId = Channel.ContentId;
                var _name = Channel.Name;
                string _description = Channel.Description;
                var _data = Channel.Data;
                int? _published = Channel.Published;
                var _image = Channel.Image;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Channel_InsertUpdate(_id, _name, _description, _image, _published, _data);

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ChannelDBSproc", "CreateUpdateChannel");
                return -1;
            }
        }

        public override Channel GetChannel ( int ChannelId )
        {
            var select = "*";
            var where = "Id = " + ChannelId;
            var orderBy = string.Empty;

            var results = GetChannelsDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<Channel> GetChannelsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Channel_SelectDynamic (select, where, orderBy).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ChannelDBSproc", "GetChannelsDyn: select" + select);
                return null;
            }
        }

        public override IEnumerable<Channel> GetAllChannelsPaged (string title, int pageIndex, int pageSize, ref int totalRecords,int status=-1 )
        {
            string select = "*";
            var where = "";

            if (!string.IsNullOrEmpty(title))
                where += "Title LIKE N'%" + title + "%' ";

            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += " Published=" + status;
            }
               
            string orderBy = "Id DESC";

            return GetAllChannelsPagedDyn ( select, where, orderBy, pageIndex, pageSize, ref totalRecords );
        }
        public override IEnumerable<Channel> GetChannelByIds(string ids, int top)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            if (ids.EndsWith(","))
                ids = ids.Remove(ids.Length - 1);

            var select = " *";
            if (top > 0)
                select = "TOP(" + top + ") *";
            var where = "Id IN (" + ids + ") AND Published = 1";
            var orderBy = "Id DESC";

            return GetChannelsDyn(select, where, orderBy);
        }
        public override IEnumerable<Channel> GetTopFiller(string title, int top)
        {
          
            var select = " *";
            if (top > 0)
                select = "TOP(" + top + ") *";
            var where = "";
            if (!string.IsNullOrEmpty(title))
                where += "Name LIKE N'%" + title + "%' ";
            var orderBy = "Id DESC";

            return GetChannelsDyn(select, where, orderBy);
        }
        public override IEnumerable<Channel> GetAllChannelsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords )
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var results = datacontext.sp_Channel_SelectPagedDynamic ( select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord ).ToArray ();
                    totalRecords = Convert.ToInt32 ( _totalRecord );

                    return results;
                }

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ChannelDBSproc", "GetAllChannelsPagedDyn: select" + select);
                return null;
            }
        }

       
        public override IEnumerable<Channel> GetAllChannelsByFilter ( int? published )
        {
            var select = "Id,Title,FilterType,DataType,Unit,Filter";
            var where = string.Empty;
            var orderBy = "Ordering ASC,Title ASC";
            if ( published != null )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " Published =" + published;
            }

            if ( !string.IsNullOrEmpty ( where ) )
                where += " AND ";

            where += "FilterType  <> 0 " + published;

            return GetChannelsDyn ( select, where, orderBy );
        }

        public override int DeleteChannelDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Channel_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "DeleteChannelDyn");
                return -1;
            }
        }

        public override int DeleteChannel ( int ChannelId ) { var where = "Id =" + ChannelId; return DeleteChannelDyn ( where ); }
        public override int DeleteChannels ( string lstChannelIds ) { var where = "Id IN (" + lstChannelIds + ")"; return DeleteChannelDyn ( where ); }


    }
}
