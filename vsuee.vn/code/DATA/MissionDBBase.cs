using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class MissionDBBase: ShopOnlineDBBase
    {
        public static MissionDBBase Create ()
        {
            return ( MissionDBBase ) Activator.CreateInstance ( typeof ( MissionDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateMission ( Mission Mission );

        #endregion

        #region READ STATEMENTs

        public abstract Mission GetMission ( int MissionId );
      
        public abstract IEnumerable<Mission> GetAllMissionsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords );

        public abstract IEnumerable<Mission> GetMissionsByFilter(string keyword, int categoryId, int status, int pageIndex, int pageSize, int year, int createdBy, ref int totalRecords);
        public abstract IEnumerable<Mission> GetMissionsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Mission> GetTopLastestMissions(int top, int categoryId);

      
        #endregion

        #region DELETE STATEMENTs
        public abstract int ViewAdd(long Id);
        public abstract int DeleteMissionDyn ( string where );
        public abstract int DeleteMission ( int MissionId );
        public abstract int DeleteMissions ( string lstMissionIds );

        #endregion
    }
}
