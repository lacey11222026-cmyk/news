using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Newtonsoft.Json;

namespace UTILS
{
    public class RedisCaching
    {
      

        public static void Add ( string key, object data )
        {
            try
            {
                CacheUtils.SetCacheObject( key, data );
            }
            catch ( Exception e )
            {
                ExHandler.Handle ( e );
            }

        }

      
        public static void Remove ( string key )
        {
            try
            {
                CacheUtils.RemoveCache( key );
            }
            catch ( Exception e )
            {
                ExHandler.Handle ( e );
            }

        }
        public static void RemoveGroup(string key)
        {
            try
            {
                CacheUtils.RemoveCacheGroup(key);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e);
            }

        }

        public static void Flush ()
        {
            try
            {
                CacheUtils.Flush();
            }
            catch ( Exception e )
            {
                ExHandler.Handle ( e );
            }

        }

        public static object GetData ( string key )
        {
            try
            {
                return CacheUtils.GetCacheObject( key );
            }
            catch ( Exception e )
            {
                ExHandler.Handle ( e );
                return null;
            }

        }

     

       
    }
}
