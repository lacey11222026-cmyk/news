using Car.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Car.CMS.Helper
{
    public static class HtmlHelpers
    {
        public static string GetGroupyName(int? id, List<Groups> lstdata)
        {
            try
            {
                var obj = lstdata.Where(x => x.GroupID == id.GetValueOrDefault()).FirstOrDefault();
                if (obj == null)
                    return "";
                return obj.Name;
            }
            catch
            {

                return "";
            }
        }
        public static string GetBankName(string username)
        {
            string result = "";
            switch (username)
            {
                case "bidv":
                    result = "Ngân hàng TMCP Đầu tư và Phát triển Việt Nam";
                    break;

                default:
                    result = "Ngân hàng thương mại cổ phần Ngoại thương Việt Nam";
                    break;

            }

            return result;
        }
        public static string GetBankName2(string username)
        {
            string result = "";
            switch (username)
            {
                case "bidv":
                    result = "Ngân hàng BIDV";
                    break;

                default:
                    result = "Ngân hàng Vietcombank";
                    break;

            }

            return result;
        }
        public static string GetReportType(int type)
        {
            string result = "";
            switch (type)
            {
                case 1:
                    result = "Quý I";
                    break;
                case 2:
                    result = "Quý II";
                    break;
                case 3:
                    result = "Quý III";
                    break;
                case 4:
                    result = "Quý IV";
                    break;
                case 5:
                    result = "Năm";
                    break;

            }

            return result;
        }
        public static string GetResponeStatus(int type)
        {
            if (type == 0)
                return "Đợi gửi";
            if (type == 1)
                return "Đã gửi";
            if (type > 1)
                return "Thành công";
            if (type < 0)
                return "Thất bại";
            return type.ToString(); ;
        }
        public static string GetCamStatus(int type)
        {
            string result = "";
            switch (type)
            {
                case 0:
                    result = "Khởi tạo";
                    break;
                case 1:
                    result = "Đợi xử lý";
                    break;
                case 2:
                    result = "Đang xử lý";
                    break;
                case 3:
                    result = "Hoàn thành";
                    break;
                case 4:
                    result = "Khóa";
                    break;
            }
            return result;
        }
        public static string GetTranStatus(int type)
        {
            string result = "";
            switch (type)
            {
                case 1:
                    result = "<div class=\"label label-md label-warning\">Đợi xử lý</div>";
                    break;
                case 2:
                    result = "<div class=\"label label-md label-warning\">Đang xử lý</div>";
                    break;
                case 3:
                    result = "<div class=\"label label-md label-success\">Đã hoàn thành</div>";
                    break;
                case 0:
                    result = "<div class=\"label label-md label-warning\">Không sử dụng</div>";
                    break;
                case -1:
                    result = "<div class=\"label label-md label-warning\">Bỏ qua</div>";
                    break;
                case -2:
                    result = "<div class=\"label label-md label-danger\">Telco khóa</div>";
                    break;
                case -3:
                    result = "<div class=\"label label-md label-warning\">Đợi nạp</div>";
                    break;
                default:
                    result = type.ToString();
                    break;
            }

            return result;
        }
        public static string GetCol(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}