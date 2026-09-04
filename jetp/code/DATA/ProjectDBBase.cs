using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class ProjectDBBase: ShopOnlineDBBase
    {
        public static ProjectDBBase Create ()
        {
            return ( ProjectDBBase ) Activator.CreateInstance ( typeof ( ProjectDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateProject ( Project manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract Project  GetProject(int Id);
        
        public abstract IEnumerable<Project> GetProjectsDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<Project> GetTopLastest(int top,int status);
        public abstract IEnumerable<Project> GetAllProjectsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Project> GetProjectsByFilter(string keyword, int status, int pageIndex, int pageSize, ref int totalRecords);
        #endregion

        #region DELETE STATEMENTs



        public abstract int DeleteProject ( int manuFactoryId );

      

        #endregion



    }
}
