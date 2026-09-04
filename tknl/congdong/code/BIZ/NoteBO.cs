using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Note = DATA.Note;
namespace BIZ
{
    public class NoteBO
    {
       

        #region CREATE
        public int CreateUpdateNote(Note Note)
        {
            
            int returnVal = NoteDBBase.Create().CreateUpdateNote(Note);
          
            return returnVal;
        }
        public int UpdateStatus(int NoteId)
        {
            try
            {
                return NoteDBBase.Create().UpdateStatus(NoteId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "NoteBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int NoteId, bool upOrder)
        {
            try
            {
                return NoteDBBase.Create().UpdateOrder(NoteId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "NoteBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public Note GetNote(int NoteId)
        {
            try
            {
                return NoteDBBase.Create().GetNote(NoteId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "NoteBO", "GetNote");
                return null;
            }
        }


        public List<Note> GetTopNote(int top,int type)
        {
            var data = NoteDBBase.Create().GetTopLastest(top,type);
            if (data == null)
                return null;

            return data.ToList();
        }

        public List<Note> GetNotesPaged(string keyword,int pageIndex, int pageSize, ref int totalRecords, int? published,int type)
        {
            var data = NoteDBBase.Create().GetAllPaged(keyword,pageIndex, pageSize, ref  totalRecords, published, type);
            if (data == null)
                return null;

            return data.ToList();
        }
       
       

        #endregion



        #region DELETE

       

        public int DeleteNote(int id)
        {
            var returnVal = NoteDBBase.Create().DeleteNote(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
