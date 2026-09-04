using System;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for DateTimeUtil
/// </summary>
namespace cms.libs
{
    public class DateTimeUtil
    {
        public DateTimeUtil()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        ~DateTimeUtil()
        {
        }

        // Trả về thời gian dạng Thứ năm, ngày 22/03/2007 - 10:30
        public string ReadTime(DateTime d)
        {
            string s = "";
            int n = Convert.ToInt32(d.DayOfWeek);
            switch (n)
            {
                case 0:
                    s = "Chủ Nhật";
                    break;
                case 1:
                    s = "Thứ Hai";
                    break;
                case 2:
                    s = "Thứ Ba";
                    break;
                case 3:
                    s = "Thứ Tư";
                    break;
                case 4:
                    s = "Thứ Năm";
                    break;
                case 5:
                    s = "Thứ Sáu";
                    break;
                case 6:
                    s = "Thứ Bảy";
                    break;
                default:
                    break;
            }
            s = s + ", ngày " + d.ToString("dd/MM/yyyy") + " - " + d.ToString("hh") + ":" + d.ToString("mm");
            return s;
        }

        // Trả về thời gian dạng Thứ năm, ngày 22/03/2007 - 10:30
        public string ReadDay(DateTime d)
        {
            string s = "";
            int n = Convert.ToInt32(d.DayOfWeek);
            switch (n)
            {
                case 0:
                    s = "Chủ Nhật";
                    break;
                case 1:
                    s = "Thứ Hai";
                    break;
                case 2:
                    s = "Thứ Ba";
                    break;
                case 3:
                    s = "Thứ Tư";
                    break;
                case 4:
                    s = "Thứ Năm";
                    break;
                case 5:
                    s = "Thứ Sáu";
                    break;
                case 6:
                    s = "Thứ Bảy";
                    break;
                default:
                    break;
            }
            s = s + ", " + d.ToString("dd/MM/yyyy HH:mm") + "  GMT+7";
            return s;
        }

        // Trả về thời gian dạng Thứ năm, ngày 22/03/2007 - 10:30
        public string ReadHM(DateTime d)
        {
            return d.Hour.ToString() + "h" + d.ToString("mm") + " GMT+7";
        }
    }
}