using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car.Data
{
    public class MainDataContextBase
    {


        protected virtual MainDataContext DataContext
        {
            get
            {
                MainDataContext datacontext = new MainDataContext();

                datacontext.Connection.ConnectionString = ConnectionString;

                return datacontext;
            }
        }

        protected virtual string ConnectionString
        {
            get
            {
                
                return Config.MainConnectionString;
            }
        }
    }
}
