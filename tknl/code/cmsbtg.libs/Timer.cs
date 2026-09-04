using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
namespace cms.libs
{
    public class Timer
    {
        public Timer()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static IList GetAllHour()
        {
            ArrayList m_ArrayList = new ArrayList();
            ListItem m_ListItem = null;
            for (int i = 0; i < 24; i++)
            {
                m_ListItem = new ListItem(string.Format("{0:D2}", i), string.Format("{0:D2}", i));
                m_ArrayList.Add(m_ListItem);
            }
            return m_ArrayList;
        }
        public static IList GetAllMinute()
        {
            ArrayList m_ArrayList = new ArrayList();
            ListItem m_ListItem = null;
            for (int i = 0; i < 60; i++)
            {
                m_ListItem = new ListItem(string.Format("{0:D2}", i), string.Format("{0:D2}", i));
                m_ArrayList.Add(m_ListItem);
            }
            return m_ArrayList;
        }
        public static IList GetAllSecond()
        {
            ArrayList m_ArrayList = new ArrayList();
            ListItem m_ListItem = null;
            for (int i = 0; i < 60; i++)
            {
                m_ListItem = new ListItem(string.Format("{0:D2}", i), string.Format("{0:D2}", i));
                m_ArrayList.Add(m_ListItem);
            }
            return m_ArrayList;
        }
    }
}
