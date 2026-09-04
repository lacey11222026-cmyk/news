using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace UTILS
{
    public class ExceptionHandling
    {
        public static void ExecptionHandling ( Exception exp )
        {
            //var strMessage = HttpContext.Current.Response.Output;
            var expCustom = new Exception ( exp.StackTrace );
            ExceptionPolicy.HandleException ( exp, Global.Configuration.ExceptionPolicy );
        }


    }
}
