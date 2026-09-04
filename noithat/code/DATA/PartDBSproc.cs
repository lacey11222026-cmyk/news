using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class PartDBSproc : PartDBBase
    {
        public override int CreateUpdatePart(Part Part)
        {
            try
            {
                int? _id = Part.Id;
                
                string _Name = Part.Name;
                string _NameEn = Part.NameEn;
                string _Code = Part.Code;
                string _Supplier = Part.Supplier;
                double? _Price = Part.Price;
                int? _Status = Part.Status;


                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Part_InsertUpdate(_id, _Code, _Supplier, _Name, _NameEn, _Status, _Price);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CreateUpdatePart");
                return -1;
            }
        }

        public override Part GetPart(int PartId)
        {
            var select = "*";
            var where = "Id = " + PartId;
            var orderBy = string.Empty;

            var results = GetPartsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<Part> GetPartsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Part_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllPartsPagedDyn select=" + select + "| where" + where);
                return null;
            }
        }

        public override IEnumerable<Part> GetAllPartsPaged(int status, string code,int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "Id DESC";
            if (status > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            if (!string.IsNullOrEmpty(code))
            {

                where += "( Name LIKE N'%" + code + "%' ";
                where += "OR NameEn LIKE N'%" + code + "%' ";
                where += "OR Code LIKE N'%" + code + "%' )";

            }
            return GetAllPartsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Part> GetAllPartsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext dc = DataContext)
                {
                    var results = dc.sp_Part_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllPartsPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Part> GetAllParts(int status,string code)
        {
            var select = "*";
            var where = string.Empty;
            code = Utils.FormatKeywordSearch(code);
            string orderBy = "Id DESC";


            if (status >0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            if (!string.IsNullOrEmpty(code))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Code =" + "'" + code + "'";

            }

            return GetPartsDyn(select, where, orderBy);
        }

       

        public override int DeletePart(int PartId)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.SP_Part_Delete(PartId);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      
       

    }
}
