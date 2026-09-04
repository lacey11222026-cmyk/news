using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class ShopOnlineDBBase
    {
        private static string _conn;
       
        protected virtual ShopOnlineDataContext DataContext
        {
            get
            {
                ShopOnlineDataContext datacontext = new ShopOnlineDataContext ();

                datacontext.Connection.ConnectionString = ConnectionString;

                return datacontext;
            }
        }

        protected virtual string ConnectionString
        {
            get
            {
                if ( _conn == null )
                    _conn = Global.Configuration.ShopOnlineConnectionString;
                return _conn;
            }
        }

       


    }
}
