using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Car.Data.Service
{
    public interface IPlanRequiresService
    {
        int CreateUpdatePlanRequire(PlanRequire manuFactory);
        int UpdateOrder(int Id, bool upOrder, int id);
        int UpdateStatus(int Id);

        PlanRequire GetPlanRequire(int Id);

        List<PlanRequire> GetList(int id);
    }
    public class PlanRequiresService : MainDataContextBase, IPlanRequiresService
    {
        public int CreateUpdatePlanRequire(PlanRequire manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;


                string Description = manufactory.Description;

                int? PlanId = manufactory.PlanId;
                string FinishDate = manufactory.FinishDate;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                double? total1 = manufactory.Total1;
                double? total2 = manufactory.Total2;
                double? total = manufactory.Total;
                string result = manufactory.Result;
                string ResultOther = manufactory.ResultOther;
                string Place = manufactory.Place;
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanRequire_InsertUpdate(_id, Description, FinishDate, PlanId, _order, _status, total, total1, total2,result,ResultOther,Place, ref responecode);
                    return responecode.GetValueOrDefault();

                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }

        public PlanRequire GetPlanRequire(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPlanRequiresDyn(select, where, order).FirstOrDefault();
        }
        public List<PlanRequire> GetList(int Id)
        {
            var select = " *";

            var where = "";
            if (Id > 0)
            {

                where = " PlanId =" + Id;
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetPlanRequiresDyn(select, where, orderBy);
        }
        private List<PlanRequire> GetPlanRequiresDyn(string select, string where, string orderBy)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    return datacontext.sp_PlanRequire_SelectDynamic(select, where, orderBy).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return new List<PlanRequire>();
            }
        }
        public int UpdateOrder(int Id, bool upOrder, int id)
        {
            try
            {

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanRequire_UpdateSortOrder(Id, id, upOrder);
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
                    datacontext.SP_PlanRequire_UpdateStatus(Id, ref responeCode);
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
