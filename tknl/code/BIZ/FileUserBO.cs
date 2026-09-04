using BIZ.Entity;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public class FileUserBO
    {



        public int CreateUpdateFileUser(FileUserFull fileUserfull)
        {

            FileUser content = fileUserfull.ConvertToBase();
            int returnVal = FileUserDBBase.Create().CreateUpdateFileUser(content);

            return returnVal;
        }


        public FileUserFull GetById(long id)
        {
            try
            {


                var content = FileUserDBBase.Create().GetFileUser(id);
                var item = new FileUserFull
                {
                    Id = content.Id,

                    CreateTime = content.CreateTime,
                    UserName = content.UserName,
                    FileName = content.FileName,
                    Keyword = content.Keyword,

                };

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "GetById");
                return null;
            }
        }
        public List<FileUserFull> GetFileUsersByFilter(int top, string title, string filetype, string username, string fromdate = "", string todate = "")
        {

            var lstItemBase = FileUserDBBase.Create().GetFileUsersByFilter(top, title,  filetype,username, fromdate, todate).ToList();
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;
            var lstItem = new List<FileUserFull>();
            foreach (var content in lstItemBase)
            {
                var item = new FileUserFull
                {
                    Id = content.Id,

                    CreateTime = content.CreateTime,
                    UserName = content.UserName,
                    FileName = content.FileName,
                    Keyword = content.Keyword,

                };

                lstItem.Add(item);
            }
            return lstItem;

        }

        public int DeleteFile(long fileUserId)
        {
            return FileUserDBBase.Create().DeleteFileUser(fileUserId);
        }




    }
}
