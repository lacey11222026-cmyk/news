using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class AttributeGroupDBSproc: AttributeGroupDBBase
    {

        public override int CreateUpdateAttributeGroup ( AttributeGroup attributeGroup )
        {
            try
            {
                int? _id = attributeGroup.Id;
                int? _categoryid = attributeGroup.CategoryId;
                string _title = attributeGroup.Title;
                byte? _published = attributeGroup.Published;
                int? _ordering = attributeGroup.Ordering;
                string _language = attributeGroup.Language;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_AttributeGroup_InsertUpdate(_id, _categoryid, _title, _published, _ordering, _language);
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "AttributeGroupDBSproc", "CreateUpdateAttributeGroup");
                return -1;
            }
        }

        public override AttributeGroup GetAttributeGroup ( int attributeGroupId )
        {
            try
            {
                int? _attributeGroupId = attributeGroupId;
                const string select = "Id,CategoryId,Title,Published,Ordering,Language";
                var where = "Id = " + _attributeGroupId;
                var orderBy = string.Empty;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_AttributeGroup_SelectDynamic ( select, where, orderBy ).FirstOrDefault ();
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "AttributeGroupDBSproc", "GetAttributeGroup:attributeGroupId=" + attributeGroupId);
                return null;
            }
        }

        public override IEnumerable<AttributeGroup> GetAttributeGroupsDyn ( string select, string where, string orderBy )
        {
            try
            {
                string _select = select;
                var _where = where;
                var _orderBy = orderBy;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_AttributeGroup_SelectDynamic ( _select, _where, _orderBy ).ToArray ();
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "AttributeGroupDBSproc", "GetAttributeGroupsDyn:select=" + select);
                return null;
            }
        }

        public override IEnumerable<AttributeGroup> GetAllAttributeGroups ()
        {
            var select = "Id,Title,Published,Language";
            var where = string.Empty;
            var orderBy = "Ordering ASC";
            var attributeGroups = GetAttributeGroupsDyn ( select, where, orderBy );
            if ( attributeGroups == null )
                return null;
            return attributeGroups.ToArray ();
        }

        public override int DeleteAttributeGroupDyn ( string where )
        {
            try
            {
                var _where = where;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_AttributeGroup_DeleteDynamic ( _where );
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "AttributeGroupDBSproc", "GetAttributeGroupsDyn:where=" + where);
                return -1;
            }
        }

        public override int DeleteAttributeGroup ( int attributeGroupId )
        {
            var where = "Id = " + attributeGroupId;
            return DeleteAttributeGroupDyn ( where );
        }

        public override int DeleteAttributeGroups ( string lstAttributeGroupIds )
        {
            var where = "Id IN (" + lstAttributeGroupIds + ")";
            return DeleteAttributeGroupDyn ( where );
        }

    }
}
