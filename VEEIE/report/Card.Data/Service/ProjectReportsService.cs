using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Car.Data.Service
{
    public interface  IProjectReportsService
    {
        int UpdateProjectReport(ProjectReport data);
        int UpdateOrder(int Id, bool upOrder, string username);
        int UpdateStatus(int Id);

        int Delete(int Id);
        ProjectReport GetProjectReport(int Id);
        List<ProjectReport> GetList(string username, string bank, int year, int type,int status, string keyword);
    }
    public class ProjectReportsService : MainDataContextBase, IProjectReportsService
    {
     
       public int UpdateProjectReport(ProjectReport data)
        {
            try
            {
                int? responecode = 0;
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_ProjectReport_InsertUpdate(data.Id, data.Name, data.Bank, data.ProjectId, data.Year, data.Type, data.ProjectInfo,
                        data.UserName,0, data.Status, data.Job1+"", data.Job2 + "", data.Job3 + "", data.Stuck + "", data.Result1 , data.Result2,data.NumberPeople,data.WomanRate,data.Time,data.FileData , ref responecode);
                }
                 
                return responecode.GetValueOrDefault();
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public  ProjectReport GetProjectReport(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetProjectReportsDyn(select, where, order).FirstOrDefault();
        }
        public  List<ProjectReport> GetList(string username,string bank,int year,int type,int status, string keyword)
        {
            var select = " *";

            var where = "";
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "(Name LIKE N'%" + keyword + "%' " + "Or ProjectInfo LIKE N'%" + keyword + "%' )";
            }
            if (!string.IsNullOrEmpty(username))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += " UserName =" + "'" + username + "'";
            }
            if (!string.IsNullOrEmpty(bank))
            {
                if(!bank.Equals("admin"))
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";
                    where += " Bank =" + "'" + bank + "'";
                }
                
            }
            if(year>0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Year = " + year;
            }
            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Status = " + status;
            }
            if (type > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                if(type==1)
                    where += " Type<=4 ";
                if (type == 2)
                    where += " Type>4 ";
            }
            var orderBy = "[Year] DESC , [Type] DESC, Id DESC";

            return GetProjectReportsDyn(select, where, orderBy);
        }
        private List<ProjectReport> GetProjectReportsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    return datacontext.sp_ProjectReport_SelectDynamic(select, where, orderBy).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return new List<ProjectReport>();
            }
        }
        public  int UpdateOrder(int Id, bool upOrder, string username)
        {
            try
            {

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_ProjectReport_UpdateSortOrder(Id, upOrder, username);
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
                    datacontext.SP_ProjectReport_UpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }
        public int Delete(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_ProjectReport_Delete(Id, ref responeCode);
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
