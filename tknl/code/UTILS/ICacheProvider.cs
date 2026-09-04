using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTILS
{
    public interface ICacheProvider
    {
        void Add(string key, object value);
        void Add(string key, object value, int timeout);
        void Add(string key, object value, string tagName, string regionName);
        void Add(string key, object value, int timeout, string tagName, string regionName);
        void Add(string key, object value, List<string> tagNames, string regionName);
        void Add(string key, object value, int timeout, List<string> tagNames, string regionName);

        void Put(string key, object value);
        void Put(string key, object value, int timeout);
        void Put(string key, object value, string regionName);
        void Put(string key, object value, string tagName, string regionName);
        void Put(string key, object value, int timeout, string tagName, string regionName);
        void Put(string key, object value, List<string> tagNames, string regionName);
        void Put(string key, object value, int timeout, List<string> tagNames, string regionName);

        object Get(string key);
        object Get(string key, string regionName);

        object this[string key] { get; set; }

        bool Remove(string key);
        bool Remove(string key, string regionName);

        void RemoveByTag(string tagName, string regionName);

        bool RemoveRegion(string regionName);

        void ClearByRegion(string regionName);

        void Flush();
    }
}
