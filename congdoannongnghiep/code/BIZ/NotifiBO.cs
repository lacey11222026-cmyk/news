using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class NotifiBO
    {
       

        #region CREATE UPDATE

        public int Create(Notifi manuFactory)
        {
            return NotifiDBBase.Create().CreateUpdateNotifi(manuFactory);
        }

       
        #endregion

        #region READ

        
      

        public List<Notifi> GetNotifi(string CreateUser, int ExpireDate)
        {
            var manuFactorys = NotifiDBBase.Create().GetNotifi(CreateUser, ExpireDate);
            if (manuFactorys == null)
                return new List<Notifi>();
            return manuFactorys.ToList();
        }
        

        #endregion

        

       
        
        
    }
}
