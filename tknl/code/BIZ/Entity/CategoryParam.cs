using System;

namespace BIZ.Entity
{
    [Serializable]
    public class CategoryParam 
    {
        public CategoryParam()
        {
            IsHomepage = false;
           
           
        }
        public bool IsHomepage
        {
            get;
            set;
        }

        //public int IsRightCol
        //{
        //    get;
        //    set;
        //}

        //public byte IsMainMenu
        //{
        //    get;
        //    set;
        //}

        //public byte IsTopMenu
        //{
        //    get;
        //    set;
        //}

        public bool IsFooter
        {
            get;
            set;
        }


    }
}
