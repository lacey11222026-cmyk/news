using System;
using DATA;

namespace BIZ.Entity
{
    [Serializable]
    public class PROMOTION_FULL: Promotion
    {
        public string CategoryName
        {
            get
            {
                var category = new CategoryBO ().GetCategoryFull ( Convert.ToInt32 ( CategoryId ) );
                if ( category == null )
                    return string.Empty;
                return category.Name;
            }
        }

        public Promotion ConvertToBase ()
        {
            Promotion promotion = new Promotion ();
            promotion.Id = Id;
            promotion.CategoryId = CategoryId;
            promotion.PromotionCode = PromotionCode;
            promotion.IntroText = IntroText;
            promotion.FullText = FullText;
            promotion.StartDate = StartDate;
            promotion.EndDate = EndDate;
            promotion.BonusType = BonusType;
            promotion.BonusValue = BonusValue;
            promotion.Published = Published;

            return promotion;
        }
    }
}
