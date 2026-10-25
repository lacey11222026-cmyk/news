using Card.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Card.CMS.Helper
{
    public static class HtmlHelpers
    {
        public static string GetGroupyName(int? id, List<Groups> lstdata)
        {
            try
            {
                var obj = lstdata.Where(x => x.GroupID == id.GetValueOrDefault()).FirstOrDefault();
                if (obj == null)
                    return "N/A";
                return obj.Name;
            }
            catch
            {

                return "N/A";
            }
        }
        public static string GetTopupType(int type,string telco)
        {
            string result = "Nạp Sò";
            if(telco== "ZING")
                result = "VLTK Miễn Phí";
            if (telco == "VTC")
                result = "Nạp Vcoin";
            switch (type)
            {
                case 1:
                    result = "Trả trước";
                    break;
                case 2:
                    result = "Trả sau";
                    break;
                case 3:
                    result = "Internet";
                    break;
                case 4:
                    result = "Nạp Edu";
                    break;
                case 5:
                    result = "ĐT cố định";
                    break;
                case 6:
                    result = "Nhà thuốc";
                    break;
                case 7:
                    result = "Tiêm chủng";
                    break;
                case 8:
                    result = "Nạp ShopOne";
                    break;

                case 11:
                    result = "VLTK - Công Thành Chiến";
                    if (telco == "GARENA")
                        result = "Nạp Free Fire";
                    break;
                case 12:
                    result = "Võ Lâm Truyền Kỳ 1";
                    break;
                case 13:
                    result = "Kiếm Thế";
                    break;
                case 14:
                    result = "Tân Thiên Long 3D";
                    break;
                case 15:
                    result = "Võ Lâm Truyền Kỳ 2";
                    break;
            }

            return result;
        }
        public static string GetTelcoName(string telco)
        {
            var result = telco;
            switch (telco)
            {
                case "VTT":
                    result = "Viettel";
                    break;
                case "VNP":
                    result = "Vina";
                    break;
                case "VMS":
                    result = "Mobi";
                    break;
               

            }

            return result;
        }
        public static string GetTranStatusNormal(int type)
        {
            string result = "";
            switch (type)
            {
                case 1:
                    result = "Đợi xử lý";
                    break;
                case 2:
                    result = "Đang xử lý";
                    break;
                case 3:
                    result = "Hoàn thành";
                    break;
                case 0:
                    result = "Không sử dụng";
                    break;
                case -1:
                    result = "Bỏ qua";
                    break;
                case -2:
                    result = "Telco khóa";
                    break;
                case -3:
                    result = "Đợi nạp";
                    break;
                case -4:
                    result = "Telco hết lượt";
                    break;
                case -6:
                    result = "Đợi review";
                    break;
                default:
                    result = type.ToString();
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
                    result = "<div class=\"label label-md label-info\">Đợi xử lý</div>";
                    break;
                case 2:
                    result = "<div class=\"label label-md label-info\">Đang xử lý</div>";
                    break;
                case 3:
                    result = "<div class=\"label label-md label-success\">Hoàn thành</div>";
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
                    result = "<div class=\"label label-md label-danger\">Đợi nạp</div>";
                    break;
                case -4:
                    result = "<div class=\"label label-md label-default\">Telco hết lượt</div>";
                    break;
                    
                case -6:
                    result = "<div class=\"label label-md label-warning\">Đợi review</div>";
                    break;
                case -9:
                    result = "<div class=\"label label-md label-info\">Chờ xử lý</div>";
                    break;
                default:
                    result = type.ToString();
                    break;
            }

            return result;
        }
        public static string GetGroupName(int groupId)
        {
            switch (groupId)
            {
                case 1: return "Văn phòng";
                case 2: return "Phòng Tổ chức cán bộ";
                case 3: return "Phòng Quản trị dữ liệu và quản lý rủi ro";
                case 4: return "Phòng Thuế cá nhân, hộ kinh doanh và thu khác";
                case 5: return "Phòng Quản lý các khoản thu từ đất";
                case 6: return "Phòng Nghiệp vụ, dự toán, pháp chế";

                case 7: return "Phòng Quản lý, hỗ trợ doanh nghiệp số 1";
                case 8: return "Phòng Quản lý, hỗ trợ doanh nghiệp số 2";
                case 9: return "Phòng Quản lý, hỗ trợ doanh nghiệp số 3";
                case 10: return "Phòng Quản lý, hỗ trợ doanh nghiệp số 4";
                case 11: return "Phòng Quản lý, hỗ trợ doanh nghiệp số 5";

                case 12: return "Phòng Kiểm tra số 1";
                case 13: return "Phòng Kiểm tra số 2";
                case 14: return "Phòng Kiểm tra số 3";
                case 15: return "Phòng Kiểm tra số 4";
                case 16: return "Phòng Kiểm tra số 5";

                default:
                    if (groupId >= 17 && groupId <= 41)
                    {
                        return $"Thuế cơ sở {groupId - 16} TP Hà Nội";
                    }
                    return "";
            }
        }
        public static string GetGroupCode(int groupId)
        {
            switch (groupId)
            {
                case 1: return "VP";
                case 2: return "TCCB";
                case 3: return "QLRR";
                case 4: return "HKD";
                case 5: return "QLĐ";
                case 6: return "NVDTPC";

                case 7: return "QLDN1";
                case 8: return "QLDN2";
                case 9: return "QLDN3";
                case 10: return "QLDN4";
                case 11: return "QLDN5";

                case 12: return "KT1";
                case 13: return "KT2";
                case 14: return "KT3";
                case 15: return "KT4";
                case 16: return "KT5";

                default:
                    if (groupId >= 17 && groupId <= 41)
                    {
                        return $"TCS{groupId - 16}";
                    }
                    return "";
            }
        }
    }
}