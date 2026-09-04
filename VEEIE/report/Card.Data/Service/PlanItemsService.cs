using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Car.Data.Service
{
    public interface IPlanItemsService
    {
        int CreateUpdatePlanItem(PlanItem manuFactory);
        int UpdateOrder(int Id, bool upOrder, int id);
        int UpdateStatus(int Id);
        PlanItem GetPlanItem(int Id);
        int UpdateDynamic(string where, string updatest);
        List<PlanItem> GetList(int id);
    }
    public class PlanItemsService : MainDataContextBase, IPlanItemsService
    {
        public int CreateUpdatePlanItem(PlanItem manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;

                string _name = manufactory.Name;
                string _Config1 = manufactory.Config1;
                string Config2 = manufactory.Config2;
                string Config3 = manufactory.Config3;

                int? PlanId = manufactory.PlanId;

                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                double? total1 = manufactory.Total1;
                double? total2 = manufactory.Total2;
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanItem_InsertUpdate(_id, _name, _Config1, Config2, Config3, PlanId, _order, _status,total1,total2,manufactory.NumberPeople,manufactory.WomanRate,manufactory.Time, ref responecode);
                    return responecode.GetValueOrDefault();

                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }

        public PlanItem GetPlanItem(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPlanItemsDyn(select, where, order).FirstOrDefault();
        }
        public int UpdateDynamic(string where, string updatest)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanItem_UpdateDynamic(updatest,where);
                }

                    return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public List<PlanItem> GetList(int Id)
        {
            var select = " *";

            var where = "";
            if (Id >-1)
            {

                where = " PlanId =" + Id;
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetPlanItemsDyn(select, where, orderBy);
        }


        private List<PlanItem> GetPlanItemsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    return datacontext.sp_PlanItem_SelectDynamic(select, where, orderBy).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;
            }
        }
        public int UpdateOrder(int Id, bool upOrder, int id)
        {
            try
            {

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanItem_UpdateSortOrder(Id, id, upOrder);
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
                    datacontext.SP_PlanItem_UpdateStatus(Id, ref responeCode);
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
