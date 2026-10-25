using Card.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SMS.Data.Factory
{
    public class ADODAOFactory : AbstractDAOFactory
    {
      
        public override ITransactionsService TransactionService()
        {
            return new TransactionService();
        }
        public override IUsersService UsersService()
        {
            return new UsersService();
        }
    }
}
