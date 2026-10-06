using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class AttributeGroupBO
    {

        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_ATTRIBUTEGROUP;
        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);
               

        #region CREATE

        public int CreateUpdateAttributeGroup ( AttributeGroup attributeGroup )
        {
            return AttributeGroupDBBase.Create ().CreateUpdateAttributeGroup ( attributeGroup );
        }

        public int CreateUpdateAttributeGroup ( ATTRIBUTEGROUP_FULL attributeGroupFull )
        {
            AttributeGroup attributeGroup = attributeGroupFull.ConvertToBase ();

            int returnVal = CreateUpdateAttributeGroup ( attributeGroup );
            if (returnVal != -1)
            {
                UpdateCache(attributeGroupFull);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get attributeGroup by attributeGroup id
        /// </summary>
        /// <param name="attributeGroupId">The attributeGroup id.</param>
        /// <returns></returns>
        public AttributeGroup GetAttributeGroup ( int attributeGroupId )
        {
            return AttributeGroupDBBase.Create ().GetAttributeGroup ( attributeGroupId );
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get attributeGroup by id => add to local cache
        /// </summary>
        /// <param name="attributeGroupId">The attributeGroup id.</param>
        /// <returns></returns>
        public ATTRIBUTEGROUP_FULL GetAttributeGroupFull ( int attributeGroupId )
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_ATTRIBUTEGROUP + attributeGroupId;

                var item = ( ATTRIBUTEGROUP_FULL ) LocalCaching.GetData ( strKeyCached );
                if ( item != null )
                    return item;

                var itemBase = GetAttributeGroup ( attributeGroupId );

                item = new ATTRIBUTEGROUP_FULL
                {
                    Id = itemBase.Id,
                    CategoryId = itemBase.CategoryId,
                    Title = itemBase.Title,
                    Published = itemBase.Published,
                    Ordering = itemBase.Ordering
                };

                LocalCaching.Add ( strKeyCached, item );

                return item;
            }
            catch ( Exception e )
            {
                ExHandler.Handle(e, "ATTRIBUTEGROUPBO", "ATTRIBUTEGROUP_FULL :attributeGroupId=" + attributeGroupId);
                return null;
            }
        }

        public string GetAttributeGroupFull_Json ( int attributeGroupId )
        {
            var attributeGroupFull = GetAttributeGroupFull ( attributeGroupId );

            if ( attributeGroupFull == null )
                return null;

            return UTILS.Utils.ConvertToJson ( GetAttributeGroupFull ( attributeGroupId ), string.Empty );
        }

        public List<AttributeGroup> GetAllAttributeGroups ()
        {
            var list = AttributeGroupDBBase.Create ().GetAllAttributeGroups ();

            if ( list == null )
                return null;

            return list.ToList ();
        }

        public List<ATTRIBUTEGROUP_FULL> GetAllAttributeGroupsFull ()
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_ALL_ATTRIBUTEGROUPS;
                var strGroupKeyCached = Constants.CACHE_GROUPKEY_ATTRIBUTEGROUP;

                var listGroupKey = ( List<string> ) LocalCaching.GetData ( strGroupKeyCached );
                if ( listGroupKey == null )
                    LocalCaching.AddToGroupKey ( strKeyCached, strGroupKeyCached );

                var lstItem = ( List<ATTRIBUTEGROUP_FULL> ) LocalCaching.GetData ( strKeyCached );
                //var lstItem = new List<CATEGORY_FULL> ();
                if ( lstItem != null && lstItem.Count > 0 )
                    return lstItem;

                var lstItemBase = GetAllAttributeGroups ();
                if ( lstItemBase == null || lstItemBase.Count == 0 )
                    return null;

                lstItem = new List<ATTRIBUTEGROUP_FULL> ();

                foreach ( var itemBase in lstItemBase )
                {
                    var item = new ATTRIBUTEGROUP_FULL ()
                    {
                        Id = itemBase.Id,                     
                        Title = itemBase.Title,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Language =itemBase.Language
                        
                    };

                    lstItem.Add ( item );
                }

                if ( lstItem.Count > 0 )
                {
                    LocalCaching.Add ( strKeyCached, lstItem );
                    LocalCaching.AddToGroupKey ( strKeyCached, strGroupKeyCached );
                }

                return lstItem;
            }
            catch ( Exception e )
            {
                ExHandler.Handle(e, "ATTRIBUTEGROUPBO", "ATTRIBUTEGROUP_FULL" );
                return null;
            }
        }

        #endregion

        #region UPDATE

        public void UpdateCache ( ATTRIBUTEGROUP_FULL attributeGroupFull )
        {
            var strKeyCached = Constants.CACHE_KEY_ATTRIBUTEGROUP + attributeGroupFull.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke ( strKeyCached, attributeGroupFull, null, null );

        }

        #endregion

        #region DELETE

        public int DeleteAttributeGroup ( int attributeGroupId )
        {
            var returnVal = AttributeGroupDBBase.Create().DeleteAttributeGroup(attributeGroupId);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public int DeleteAttributeGroups ( string listId )
        {
            var returnVal =  AttributeGroupDBBase.Create ().DeleteAttributeGroups ( listId );            
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        #endregion


        public void FlushAllCache(string containKey)
        {
            DelegateFlushAllCache delegateFlushAllProductCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllProductCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }
      

    }
}
