using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class AttributeDBSproc: AttributeDBBase
    {
        public override int CreateUpdateAttribute ( Attribute attribute )
        {
            try
            {
                int? _id = attribute.Id;
                int? _groupid = attribute.GroupId;
                int? _categoryid = attribute.CategoryId;
                string _title = attribute.Title;
                byte? _type = attribute.FilterType;
                byte? _published = attribute.Published;
                int? _ordering = attribute.Ordering;
                string _params = attribute.Params;
                byte? _datatype = attribute.DataType;
                string _unit = attribute.Unit;
                string _filter = attribute.Filter;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Attribute_InsertUpdate ( _id, _groupid, _categoryid, _title, _type, _published, _ordering, _params, _datatype, _unit, _filter );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "AttributeDBSproc", "CreateUpdateAttribute");
                return -1;
            }
        }

        public override Attribute GetAttribute ( int attributeId )
        {
            var select = "*";
            var where = "Id = " + attributeId;
            var orderBy = string.Empty;

            var results = GetAttributesDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<Attribute> GetAttributesDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Attribute_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "AttributeDBSproc", "GetAttributesDyn: select" + select);
                return null;
            }
        }

        public override IEnumerable<Attribute> GetAllAttributesPaged ( int pageIndex, int pageSize, ref int totalRecords )
        {
            string select = "*";
            var where = "";
            string orderBy = "Id DESC";

            return GetAllAttributesPagedDyn ( select, where, orderBy, pageIndex, pageSize, ref totalRecords );
        }

        public override IEnumerable<Attribute> GetAllAttributesPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords )
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var results = datacontext.sp_Attribute_SelectPagedDynamic ( select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord ).ToArray ();
                    totalRecords = Convert.ToInt32 ( _totalRecord );

                    return results;
                }

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "AttributeDBSproc", "GetAllAttributesPagedDyn: select" + select);
                return null;
            }
        }

        public override IEnumerable<Attribute> GetAllAttributes ( string name, int categoryId, int groupId )
        {
            var select = "*";
            var where = string.Empty;
            var orderBy = "Ordering ASC,Title ASC";

            if ( !string.IsNullOrEmpty ( name ) )
                where += "Title LIKE N'%" + name + "%' ";
            if (categoryId != 0)
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " CategoryId =" + categoryId;
            }

            if ( groupId > 0 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " GroupId =" + groupId;
            }

            return GetAttributesDyn ( select, where, orderBy );
        }

        public override IEnumerable<Attribute> GetAllAttributesByFilter ( int categoryId, byte? published )
        {
            var select = "Id,Title,FilterType,DataType,Unit,Filter";
            var where = string.Empty;
            var orderBy = "Ordering ASC,Title ASC";

            if ( categoryId > 0 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " CategoryId =" + categoryId;
            }
            if ( published != null )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " Published =" + published;
            }

            if ( !string.IsNullOrEmpty ( where ) )
                where += " AND ";

            where += "FilterType  <> 0 " + published;

            return GetAttributesDyn ( select, where, orderBy );
        }

        public override int DeleteAttributeDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Attribute_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "DeleteAttributeDyn");
                return -1;
            }
        }

        public override int DeleteAttribute ( int attributeId ) { var where = "Id =" + attributeId; return DeleteAttributeDyn ( where ); }
        public override int DeleteAttributes ( string lstAttributeIds ) { var where = "Id IN (" + lstAttributeIds + ")"; return DeleteAttributeDyn ( where ); }


    }
}
