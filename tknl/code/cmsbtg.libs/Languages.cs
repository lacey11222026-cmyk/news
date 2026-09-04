using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace cms.libs
{
    public class Languages
    {
        private XmlDocument _doc = null;
        private XmlNode _pagePointer = null;
        private string _fileName = "";
        private string _currentPage = "";
        private string _code = "";

        public Languages()
        {
        }

        public Languages(string FileName)
        {
            _fileName = FileName;
            LoadFile();
        }
        public Languages(string FileName, string Page)
        {
            _fileName = FileName;
            LoadFile();
            SetPage(Page);
        }


        private void LoadFile()
        {
            if (_fileName == "" || !System.IO.File.Exists(_fileName))
                throw (new ApplicationException("Invalid language file " + _fileName));

            if (_doc == null)
                _doc = new XmlDocument();

            _doc.Load(_fileName);
            try
            {
                _doc.Load(_fileName);
                if (_doc.DocumentElement.Attributes["code"] != null)
                    _code = _doc.DocumentElement.Attributes["code"].Value;
                else
                    _code = "en";
            }
            catch
            {
                _doc = null;
            }
        }

        public void LoadFile(string FileName)
        {
            _fileName = FileName;
            LoadFile();
        }

        public void SetPage(string Page)
        {
            if (_currentPage == Page)
                return;

            _pagePointer = null;
            _currentPage = "";

            if (_doc != null)
            {
                _pagePointer = _doc.SelectSingleNode(string.Format("//page[@name='{0}']", Page.ToUpper()));
                _currentPage = Page;
            }
        }

        public string GetText(string text)
        {
            text = text.ToUpper(new System.Globalization.CultureInfo("en"));
            if (_doc == null)
                return "";
            XmlNode el = null;
            if (_pagePointer != null)
            {
                el = _pagePointer.SelectSingleNode(string.Format("Resource[@tag='{0}']", text));
                if (el == null)
                    el = _doc.SelectSingleNode(string.Format("//Resource[@tag='{0}']", text));
            }
            else
            {
                el = _doc.SelectSingleNode(string.Format("//Resource[@tag='{0}']", text));
            }

            if (el != null)
            {
                //return el.InnerText;
                string sRet = el.InnerText;
                sRet = sRet.Replace("[", "<");
                sRet = sRet.Replace("]", ">");
                return sRet;
            }
            else
                return null;
        }

        public string GetText(string page, string text)
        {
            SetPage(page);
            return GetText(text);
        }

        public string LanguageCode
        {
            get
            {
                return _code;
            }
        }
        public DataTable GetPage()
        {
            DataSet ds = new DataSet();
            XmlTextReader reader = new XmlTextReader(_pagePointer.InnerXml, XmlNodeType.Element, null);
            ds.ReadXml(reader);
            return ds.Tables[0];
        }
        public DataTable GetPage(string Page)
        {
            SetPage(Page);
            return GetPage();
        }
        public DataTable GetPage(string Page, string sort)
        {
            DataTable dt = GetPage(Page);
            return SortDataTable(dt, sort);
        }

        public static DataTable GetAllLanguages(string sLanguageDir)
        {
            using (DataTable dt = new DataTable("AllLanguages"))
            {
                dt.Columns.Add("code", typeof(string));
                dt.Columns.Add("caption", typeof(string));
                dt.Columns.Add("priority", typeof(int));
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sLanguageDir);
                System.IO.FileInfo[] files = dir.GetFiles("*.xml");
                int i = 1;
                foreach (System.IO.FileInfo file in files)
                {
                    try
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.Load(file.FullName);
                        DataRow dr = dt.NewRow();
                        dr["code"] = doc.DocumentElement.Attributes["code"].Value;
                        dr["caption"] = doc.DocumentElement.Attributes["language"].Value;
                        try
                        {
                            dr["priority"] = Convert.ToInt32(doc.DocumentElement.Attributes["priority"].Value);
                        }
                        catch
                        {
                            dr["priority"] = i;
                        }
                        dt.Rows.Add(dr);
                    }
                    catch (Exception)
                    {
                    }
                    i++;
                }
                return SortDataTable(dt, "priority ASC");

            }
        }
        private static DataTable SortDataTable(DataTable dt, string sort)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;

            DataTable newDT = dt.Clone();
            DataRow[] foundRows = dt.Select(null, sort);
            foreach (DataRow dr in foundRows)
            {
                newDT.ImportRow(dr);
            }
            return newDT;
        }
    }
}