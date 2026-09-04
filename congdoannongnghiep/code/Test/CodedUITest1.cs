using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using BIZ;
using DATA.SMS;
using UTILS;
using Constants = UTILS.Constants;
namespace Test
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
      

        [TestMethod]
        public void CodedUITestMethod1()
        {
            //var _staticCategoryList = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            //var _staticCategoryByUserList = new CategoryBO().GetCategoryByUserName(_staticCategoryList, "quantri",
            //                                                                  false);

            //var abc = _staticCategoryByUserList;
            //var obj = new SMSLog { Admin = "abc",PartnerCode="abc", Ip = "123", Message = "test", Mobile = "01256748678", Name = "cuongpm", Status = 0 };
            //new SMSLogDAL().InsertUpdate(obj);
            //int total=0;
            //var data = new SMSLogDAL().GetList(-999, "", "", "abc", 1, 5, ref total);
            //var x = data;
            //RedisCaching.Add("test_456","333");
            //RedisCaching.Add("test_222", "333");
            //Console.WriteLine(x);
            RedisCaching.RemoveGroup("test");
        }

    }


}

