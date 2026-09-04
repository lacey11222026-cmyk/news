using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class NoteDBBase: ShopOnlineDBBase
    {
        public static NoteDBBase Create ()
        {
            return ( NoteDBBase ) Activator.CreateInstance ( typeof ( NoteDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateNote ( Note manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract Note  GetNote(int Id);
        public abstract IEnumerable<Note> GetAllPaged(string keyword, int pageIndex, int pageSize, ref int totalRecords, int? published,int type);
        public abstract IEnumerable<Note> GetNotesDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<Note> GetTopLastest(int top,int type);

        #endregion

        #region DELETE STATEMENTs

        

        public abstract int DeleteNote ( int manuFactoryId );

      

        #endregion



    }
}
