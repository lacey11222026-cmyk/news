using System;
using System.Collections.Generic;

namespace BIZ.Entity
{
    [Serializable]
    public class ArticleParam 
    {
        public List<RelatedNews> relatedNews
        {
            get;
            set;
        }

    }
    public class RelatedNews
    {
        public int Id
        {
            get;
            set;
        }
        public int Title
        {
            get;
            set;
        }
    }


}
