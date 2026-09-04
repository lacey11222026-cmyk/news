using System;
using System.Configuration;
using System.Web.Configuration;

namespace UTILS
{
    public class Configuration
    {
        public string ShopOnlineConnectionString { get; set; }
        //public string MemberShipSQLProviderConnectionString { get; set; }

        public string ExceptionPolicy { get; set; }

        public Configuration ()
        {
            // Declare properties and objects
            Cryptography.RijndaelEnhanced rijndaelKey = new Cryptography.RijndaelEnhanced ( "shoponline", "@1B2c3D4e5F6g7H8" );
            // Init Connection object
            ConnectionStringSettingsCollection ListConnectionString = WebConfigurationManager.ConnectionStrings;
            //ShopOnlineConnectionString = rijndaelKey.Decrypt ( ListConnectionString [Constants.SHOP_ONLINE_CONNECTION_STRING].ToString () );
            ShopOnlineConnectionString = ListConnectionString[Constants.SHOP_ONLINE_CONNECTION_STRING].ToString();
            //MemberShipSQLProviderConnectionString = rijndaelKey.Decrypt ( ListConnectionString [Constant.MEMBERSHIP_CONNECTION_STRING].ToString () );

            ExceptionPolicy = Constants.EXCEPTION_POLICY;
        }

    }


}
