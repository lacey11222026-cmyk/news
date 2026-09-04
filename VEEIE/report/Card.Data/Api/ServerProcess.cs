using Car.Data.DTO;
using Car.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car.Data.Api
{
    public class ServerProcess
    {
        private static readonly string Url = ConfigurationManager.AppSettings["Api"];
    }
}
