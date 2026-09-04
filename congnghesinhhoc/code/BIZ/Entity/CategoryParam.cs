using System;

namespace BIZ.Entity
{
    [Serializable]
    public class CategoryParam 
    {
        public byte IsHomepage
        {
            get;
            set;
        }

        public int IsRightCol
        {
            get;
            set;
        }

        public byte IsMainMenu
        {
            get;
            set;
        }

        public byte IsTopMenu
        {
            get;
            set;
        }

        public byte IsFooter
        {
            get;
            set;
        }
       

    }
}
