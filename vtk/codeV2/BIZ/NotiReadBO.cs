using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class NotiReadBO
    {
     

        #region CREATE UPDATE

        public int Read(NotiRead manuFactory)
        {
            return NotiReadDBBase.Create().Read(manuFactory);
        }

        public int ReadMulti(int expireDate, string userName, string notiIds)
        {
            return NotiReadDBBase.Create().ReadMulti(expireDate, userName, notiIds);
        }

        #endregion

        #region READ



        public List<NotiRead> GetNotiRead(string CreateUser, int ExpireDate)
        {
            var listNotiRead = NotiReadDBBase.Create().GetNotiRead(CreateUser, ExpireDate);
            if (listNotiRead == null)
                return null;
            return listNotiRead.ToList();
        }

      

        #endregion

       
    }
}
