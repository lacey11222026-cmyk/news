using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class ProductAttributeDBSproc: ProductAttributeDBBase
    {
        #region Overrides of ProductAttributeDBBase

        public override int CreateUpdateProductAttribute ( ProductAttribute productAttribute )
        {
            try
            {
                int? _id = productAttribute.Id;
                int? _attributeid = productAttribute.AttributeId;
                int? _productid = productAttribute.ProductId;
                double? _numbericvalue = productAttribute.NumbericValue;
                string _textvalue = productAttribute.TextValue;
                byte _ordering = productAttribute.Ordering;
                string _params = productAttribute.Params;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_ProductAttribute_InsertUpdate ( _id, _attributeid, _productid, _numbericvalue, _textvalue, _ordering, _params );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ProductAttributeDBSproc", "CreateUpdateProductAttribute");
                return -1;
            }
        }

        public override ProductAttribute GetProductAttribute ( int productAttributeId )
        {
            try
            {
                string select = "*";
                string where = "Id =" + productAttributeId;
                string orderBy = string.Empty;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_ProductAttribute_SelectDynamic ( select, where, orderBy ).FirstOrDefault ();
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ProductAttributeDBSproc", "GetProductAttribute productAttributeId= " + productAttributeId);
                return null;
            }
        }

        public override IEnumerable<ProductAttribute> GetProductAttributesDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_ProductAttribute_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ProductAttributeDBSproc", "GetProductAttributesDyn where= " + where);
                return null;
            }
        }

        public override IEnumerable<ProductAttribute> GetAllProductAttributes ( int attributeId )
        {
            var select = "Id,AttributeId,ProductId,NumbericValue,TextValue";
            var where = "AttributeId = " + attributeId;
            var order = string.Empty;

            return GetProductAttributesDyn ( select, where, order );
        }

        public override IEnumerable<ProductAttribute> GetAllProductAttributes ( int attributeId, List<string> lstTextValue, List<double> lstNumberValue )
        {
            var select = "Id,AttributeId,ProductId,NumbericValue,TextValue,Ordering";
            var where = string.Empty;

            if ( attributeId > 0 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";
                where += "AttributeId =" + attributeId;
            }

            if ( lstTextValue.Count > 0 )
            {
                foreach ( var textValue in lstTextValue )
                {
                    if ( string.IsNullOrEmpty ( textValue ) )
                        continue;

                    if ( !string.IsNullOrEmpty ( where ) )
                        where += " AND ";
                    where += "TextValue LIKE N'%" + textValue + "%' ";
                }
            }

            if ( lstNumberValue.Count > 0 )
            {
                foreach ( var numberVale in lstNumberValue )
                {
                    if ( !string.IsNullOrEmpty ( where ) )
                        where += " AND ";

                    where += "NumbericValue = " + numberVale;
                }


            }

            var order = "Ordering ASC";

            return GetProductAttributesDyn ( select, where, order );
        }


        public override IEnumerable<ProductAttribute> GetAllProductAttributes ( int attributeId, double fromVal, double toVal )
        {
            var select = "Id,AttributeId,ProductId,NumbericValue,TextValue,Ordering";

            var where = string.Empty;

            if ( attributeId > 0 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";
                where += "AttributeId =" + attributeId;
            }

            if ( !string.IsNullOrEmpty ( where ) )
                where += " AND ";

            where += "NumbericValue => " + fromVal + " AND NumbericValue <= " + toVal;

            var order = "Ordering ASC";

            return GetProductAttributesDyn ( select, where, order );
        }

        public override IEnumerable<ProductAttribute> GetAllProductAttributesByProduct(int productId)
        {
            var select = "Id,AttributeId,ProductId,NumbericValue,TextValue,Ordering";

            var where = string.Empty;

            if (productId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "ProductId =" + productId;
            }      

            var order = "Ordering ASC";

            return GetProductAttributesDyn(select, where, order);
        }

        public override int DeleteProductAttributeDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_ProductAttribute_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ProductAttributeDBSproc", "DeleteProductAttributeDyn where= " + where);
                return -1;
            }
        }
        public override int DeleteProductAttribute ( int productAttributeId )
        {
            string where = "Id = " + productAttributeId;
            return DeleteProductAttributeDyn ( where );
        }

        public override int DeleteProductAttributeByProductId ( int productId )
        {
            string where = "ProductId = " + productId;
            return DeleteProductAttributeDyn ( where );
        }

        public override int DeleteProductAttributeByProductId ( string listProductIds )
        {
            string where = "ProductId IN ( " + listProductIds + ")";
            return DeleteProductAttributeDyn ( where );
        }

        #endregion
    }
}
