using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Project = DATA.Project;
namespace BIZ
{
    public class ProjectBO
    {
       

        #region CREATE
        public int CreateUpdateProject(Project Project)
        {
            
            int returnVal = ProjectDBBase.Create().CreateUpdateProject(Project);
          
            return returnVal;
        }
        public int UpdateStatus(int ProjectId)
        {
            try
            {
                return ProjectDBBase.Create().UpdateStatus(ProjectId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProjectBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int ProjectId, bool upOrder)
        {
            try
            {
                return ProjectDBBase.Create().UpdateOrder(ProjectId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProjectBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public Project GetProject(int ProjectId)
        {
            try
            {
                return ProjectDBBase.Create().GetProject(ProjectId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProjectBO", "GetProject");
                return null;
            }
        }

        public List<Project> GetProjectsByFilter(string title,  int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            return ProjectDBBase.Create().GetProjectsByFilter(title, status, pageIndex, pageSize, ref totalRecords).ToList();
        }
        public List<Project> GetTopProject(int top,int status)
        {
            var data = ProjectDBBase.Create().GetTopLastest(top, status);
            if (data == null)
                return null;

            return data.ToList();
        }

        
       
       

        #endregion



        #region DELETE

       

        public int DeleteProject(int id)
        {
            var returnVal = ProjectDBBase.Create().DeleteProject(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
