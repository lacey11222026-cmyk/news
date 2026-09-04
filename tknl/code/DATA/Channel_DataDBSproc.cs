using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class Channel_DataDBSproc: Channel_DataDBBase
    {
        public override int CreateUpdateChannel_Data ( Channel_Data Channel_Data )
        {
            try
            {
                long? _id = Channel_Data.Id;
                long? _contentId = Channel_Data.ContentId;
                int? _channelId = Channel_Data.ChannelId;
               
                
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_ChannelData_InsertUpdate(_id, _contentId, _channelId);

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "Channel_DataDBSproc", "CreateUpdateChannel_Data");
                return -1;
            }
        }

        public override Channel_Data GetById(int Id)
        {
            var select = "*";
            var where = "Id  = " + Id;
            var orderBy = string.Empty;

            var results = GetChannel_DatasDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<Channel_Data> GetChannel_DatasDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_ChannelData_SelectDynamic (select, where, orderBy).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ChannelDataDBSproc", "GetChannel_DatasDyn: select" + select);
                return null;
            }
        }

        public override IEnumerable<Channel_Data> GetAllChannel_DatasPaged(int channelId, int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "[ChannelId]="+ channelId;
            string orderBy = "Id DESC";

            return GetAllChannel_DatasPagedDyn ( select, where, orderBy, pageIndex, pageSize, ref totalRecords );
        }

        public override IEnumerable<Channel_Data> GetAllChannel_DatasPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords )
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var results = datacontext.sp_ChannelData_SelectPagedDynamic ( select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord ).ToArray ();
                    totalRecords = Convert.ToInt32 ( _totalRecord );

                    return results;
                }

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "Channel_DataDBSproc", "GetAllChannel_DatasPagedDyn: select" + select);
                return null;
            }
        }

       
        public override IEnumerable<Channel_Data> GetByContentId (long ContentId )
        {
            var select = "*";
            var where = string.Empty;
            var orderBy = "Id Desc";

            where += " ContentId =" + ContentId;
            
          
            return GetChannel_DatasDyn ( select, where, orderBy );
        }

        public override int DeleteChannel_DataDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_ChannelData_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "DeleteChannel_DataDyn");
                return -1;
            }
        }
        public override int DeleteById(int ChannelId,long ContenId) { var where = "ChannelId =" + ChannelId +"And ContentId="+ContenId; return DeleteChannel_DataDyn(where); }
        public override int DeleteByChannelId(int ChannelId) { var where = "ChannelId =" + ChannelId; return DeleteChannel_DataDyn(where); }
        public override int DeleteById(long Id) { var where = "Id =" + Id; return DeleteChannel_DataDyn(where); }
       

    }
}
