using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIZ.Entity
{

    [Serializable]
    public class CateLite
    {
        public int ParrentId
        {
            get;
            set;
        }
        public string Language
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
    }
}
