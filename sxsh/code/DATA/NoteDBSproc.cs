using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class NoteDBSproc : NoteDBBase
    {
        #region Overrides of NoteDBBase

        public override int CreateUpdateNote(Note manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;

                string _name = manufactory.Title;
                string _content = manufactory.Contents;
                string _des = manufactory.Description;
                string _Location = manufactory.Location;
                string _image = manufactory.Image;
                string _CategoryPathway = manufactory.CategoryPathway;
                string _Params = manufactory.Params;
                //string _fax = manufactory.Fax;
                string _CreatedBy = manufactory.CreatedBy;
                int? _CategoryId = manufactory.CategoryId;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                DateTime? _PublishDate = manufactory.PublishDate;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Note_InsertUpdate(_id, _des, _content, _image, _CreatedBy, _name, _CategoryId, _CategoryPathway, _PublishDate, _Location, _status, _Params, ref responecode);
                    return responecode.GetValueOrDefault();
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "NoteDBSproc", "CreateUpdateNote");
                return -1;
            }
        }

        public override Note GetNote(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetNotesDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<Note> GetTopLastest(int top, int type)
        {
            var select = " [Id] ,[Description] ,[Image] ,[Title] ,[CategoryId] ,[PublishDate] ,[Location],[Status]";

                

            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            if (type > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + type + ",%' ";
            }
            var orderBy = "[Order] DESC, Id DESC";

            return GetNotesDyn(select, where, orderBy);
        }
        public override IEnumerable<Note> GetAllPaged(string keyword, int pageIndex, int pageSize, ref int totalRecords, int? published, int type)
        {
            var select = " [Id] ,[Description] ,[Image] ,[Title] ,[CategoryId] ,[PublishDate] ,[Location],[Status]";


            var where = string.Empty;


            if (published >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + published;
            }
            if (type >0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + type + ",%' ";

            }
           
           
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( [Title] LIKE N'%" + keyword + "%' ";
               
                where += "OR [Location] LIKE N'%" + keyword + "%' )";

            }
            var orderBy = "[Order] DESC, ID DESC";

            return GetAllNotesPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public IEnumerable<Note> GetAllNotesPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                string _select = select;
                string _where = where;
                string _orderBy = orderBy;
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecords = 0;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var list = datacontext.sp_Note_SelectPagedDynamic(_select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "NoteDBSproc", "GetAllNotesPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Note> GetNotesDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Note_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "NoteDBSproc", "GetNotesDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id, bool upOrder)
        {
            try
            {

                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Note_UpdateSortOrder(Id, upOrder);
                    return 1;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }


        public override int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Note_UpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }


        public override int DeleteNote(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Note_Delete(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }



        #endregion
    }
}
