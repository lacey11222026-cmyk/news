using System;
using System.Configuration;
namespace cms.libs
{
    /// <summary>
    /// Summary description for Config
    /// </summary>
    public sealed class Key
    {
        private static readonly Key instance = new Key();
        private string _sKey;
        public static string sKey
        { 
            get { 
                return instance._sKey; 
            }
        }
        Key()
        {
            _sKey = "7516cf6c49efdf48b828b991dca7cc36" + DateTime.Now.Month.ToString();
        }
        public static Key Instance
        {
            get { return instance; }
        }
    }
}