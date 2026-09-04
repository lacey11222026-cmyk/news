using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car.Data.Service;


namespace SMS.Data.Factory
{
    public abstract class AbstractDAOFactory
    {
        public static AbstractDAOFactory Instance()
        {
            try
            {
                return (AbstractDAOFactory)new ADODAOFactory();
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't create AbstractDAOFactory: ");
            }
        }
        public abstract IPlansService PlansService();
        public abstract IPlanItemsService PlanItemsService();
       

    }
}

