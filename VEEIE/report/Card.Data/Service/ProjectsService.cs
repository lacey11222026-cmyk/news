using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Car.Data.Service
{
    public interface  IProjectsService
    {
        int UpdateProject(Project data);
        int UpdateOrder(int Id, bool upOrder, string username);
        int UpdateStatus(int Id);
        Project GetProject(int Id);
        List<Project> GetList(string username);
    }
    public class ProjectsService : MainDataContextBase, IProjectsService
    {
     
       public int UpdateProject(Project data)
        {
            try
            {
                int? responecode = 0;
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Project_InsertUpdate(data.Id, data.Name, data.Bank, data.Investor, data.Address, data.Contact, data.Organ,
                        data.UserName, data.Tel, data.Email, data.TimeExpire, data.Total, data.Total1, data.Total2, data.GroupName, 0, data.Status, ref responecode);
                }
                 
                return responecode.GetValueOrDefault();
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public  Project GetProject(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetProjectsDyn(select, where, order).FirstOrDefault();
        }
        public  List<Project> GetList(string username)
        {
            var select = " *";

            var where = "";
            if (!string.IsNullOrEmpty(username))
            {

                where = " UserName =" + "'" + username + "'";
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetProjectsDyn(select, where, orderBy);
        }
        private List<Project> GetProjectsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Project_SelectDynamic(select, where, orderBy).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return new List<Project>();
            }
        }
        public  int UpdateOrder(int Id, bool upOrder, string username)
        {
            try
            {

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Project_UpdateSortOrder(Id, upOrder, username);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }


        public  int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Project_UpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }

    }
}
