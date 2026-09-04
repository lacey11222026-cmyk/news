using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DATA;

namespace BIZ.Entity
{
    
    [Serializable]
    public class Survey_Full : DATA.Survey
    {
        public List<SurveyItem> SurveyItems
        {
            get;
            set;
        }
      
    }
}
