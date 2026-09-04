using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car.Data.Service;


namespace SMS.Data.Factory
{
    public class ADODAOFactory : AbstractDAOFactory
    {
        public override IPlansService PlansService()
        {
            return new PlansService();
        }
        public override IPlanItemsService PlanItemsService()
        {
            return new PlanItemsService();
        }
        
    }
}
