using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car.Data.Service
{
    public interface IPlanStucksService
    {
        int CreateUpdatePlanStuck(PlanStuck manuFactory);
        int UpdateOrder(int Id, bool upOrder, int id);
        int UpdateStatus(int Id);
        PlanStuck GetPlanStuck(int Id);
        List<PlanStuck> GetList(int id);
        int UpdateDynamic(string where, string updatest);
    }
    public class PlanStucksService : MainDataContextBase, IPlanStucksService
    {
        public int CreateUpdatePlanStuck(PlanStuck manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;

                string _name = manufactory.Name;
                string Description = manufactory.Description;
                string Organ = manufactory.Organ;
                string Result1 = manufactory.Result1;
                string Result2 = manufactory.Result2;
                int? PlanId = manufactory.PlanId;
                string FinishDate = manufactory.FinishDate;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanStuck_InsertUpdate(_id, _name, Description, Organ, FinishDate, Result1, Result2, PlanId, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();

                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }
        public int UpdateDynamic(string where, string updatest)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanStuck_UpdateDynamic(updatest, where);
                }

                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public PlanStuck GetPlanStuck(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPlanStucksDyn(select, where, order).FirstOrDefault();
        }
        public List<PlanStuck> GetList(int Id)
        {
            var select = " *";

            var where = "";
            if (Id > -1)
            {

                where = " PlanId =" + Id;
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetPlanStucksDyn(select, where, orderBy);
        }


        private List<PlanStuck> GetPlanStucksDyn(string select, string where, string orderBy)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    return datacontext.sp_PlanStuck_SelectDynamic(select, where, orderBy).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return new List<PlanStuck>();
            }
        }
        public int UpdateOrder(int Id, bool upOrder, int id)
        {
            try
            {

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanStuck_UpdateSortOrder(Id, id, upOrder);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }


        public int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanStuck_UpdateStatus(Id, ref responeCode);
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
