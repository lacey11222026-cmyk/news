using System;
namespace DATA.ContentDB
{
    public class VanThu
    {
        public int Id { get; set; }
        /// <summary>Bước 0: Đơn vị nhập</summary>
        public int? GroupId { get; set; }
        /// <summary>Bước 0: Mã báo cáo</summary>
        public string ReportCode { get; set; }
        /// <summary>Bước 0: Mã đơn tra cứu</summary>
        public string DocumentCode { get; set; }
        /// <summary>Bước 0: Bước đang thực hiện</summary>
        public int? Step { get; set; }
        /// <summary>Bước 0: Đã kết thúc</summary>
        public bool? IsCompleted { get; set; }
        /// <summary>Bước 0: Thời gian kết thúc</summary>
        public DateTime? CompletedAt { get; set; }
        /// <summary>Bước 1: Ngày báo cáo</summary>
        public DateTime? NgayBaoCao { get; set; }
        /// <summary>Bước 1: Ngày công văn đến</summary>
        public DateTime? NgayCvd { get; set; }
        /// <summary>Bước 1: Số công văn đến</summary>
        public string SoCvDen { get; set; }
        /// <summary>Bước 1: Nguồn tiếp nhận</summary>
        public int? NguonTiepNhan { get; set; }
        /// <summary>Bước 1: Số và ký hiệu văn bản</summary>
        public string SoKyHieuVb { get; set; }
        /// <summary>Bước 1: Cán bộ được giao giải quyết</summary>
        public string CbDuocGiao { get; set; }
        /// <summary>Bước 1: Tên người gửi đơn</summary>
        public string TenNguoiGui { get; set; }
        /// <summary>Bước 1: Mã số thuế</summary>
        public string Mst { get; set; }
        /// <summary>Bước 1: Phòng/Thuế cơ sở theo địa bàn bị KN/TC/PAKN (danh sách ID)</summary>
        public string AffectedUnits { get; set; }
        /// <summary>Bước 1: Nội dung đơn</summary>
        public string NoiDungDon { get; set; }
        /// <summary>Bước 1: Đơn đã có bản án của Tòa</summary>
        public bool? DonDaCoBanAn { get; set; }
        /// <summary>Bước 1: 1. Chế độ/chính sách</summary>
        public bool? RelatedField1 { get; set; }
        /// <summary>Bước 1: 2. QĐ hành chính của CQT</summary>
        public bool? RelatedField2 { get; set; }
        /// <summary>Bước 1: 3. Hành vi HC của CBCC</summary>
        public bool? RelatedField3 { get; set; }
        /// <summary>Bước 1: 4. Tham nhũng.</summary>
        public bool? RelatedField4 { get; set; }
        /// <summary>Bước 1: 5. Tư pháp.</summary>
        public bool? RelatedField5 { get; set; }
        /// <summary>Bước 1: 6. Trốn thuế.</summary>
        public bool? RelatedField6 { get; set; }
        /// <summary>Bước 1: 7. Đảng/đoàn thể; </summary>
        public bool? RelatedField7 { get; set; }
        /// <summary>Bước 1: 8. Kiểm tra Thuế; </summary>
        public bool? RelatedField8 { get; set; }
        /// <summary>Bước 1: 9. Hoàn thuế</summary>
        public bool? RelatedField9 { get; set; }
        /// <summary>Bước 1: 10. Nợ thuế</summary>
        public bool? RelatedField10 { get; set; }
        /// <summary>Bước 1: 11. Đất đai.</summary>
        public bool? RelatedField11 { get; set; }
        /// <summary>Bước 1: 12. TNDN</summary>
        public bool? RelatedField12 { get; set; }
        /// <summary>Bước 1: 13. GTGT</summary>
        public bool? RelatedField13 { get; set; }
        /// <summary>Bước 1: 14. TNCN</summary>
        public bool? RelatedField14 { get; set; }
        /// <summary>Bước 1: 15. Hóa đơn</summary>
        public bool? RelatedField15 { get; set; }
        /// <summary>Bước 1: 16. Miễn giảm</summary>
        public bool? RelatedField16 { get; set; }
        /// <summary>Bước 1: 17. Khai khống TNCN</summary>
        public bool? RelatedField17 { get; set; }
        /// <summary>Bước 1: 18. Mạo danh thành lập DN</summary>
        public bool? RelatedField18 { get; set; }
        /// <summary>Bước 1: 19. Khác.</summary>
        public bool? RelatedField19 { get; set; }
        /// <summary>Bước 1: Lĩnh vực khác (ghi rõ)</summary>
        public string RelatedFieldOtherText { get; set; }
        /// <summary>Bước 1: CCCD</summary>
        public string CccdNguoiGuiDon { get; set; }
        /// <summary>Bước 1: Ngày ghi trên đơn</summary>
        public DateTime? NgayDonThu { get; set; }
        /// <summary>Bước 2: Kết quả đánh giá điều kiện</summary>
        public int? DieuKienXuLy { get; set; }
        /// <summary>Bước 2: KĐK-01 — Không đáp ứng yêu cầu tại điểm a Khoản 2 TT05</summary>
        public bool? Kdk01 { get; set; }
        /// <summary>Bước 2: KĐK-02 — Gửi nhiều cơ quan</summary>
        public bool? Kdk02 { get; set; }
        /// <summary>Bước 2: KĐK-03 — Đơn Khiếu nại đã được hướng dẫn</summary>
        public bool? Kdk03 { get; set; }
        /// <summary>Bước 2: KĐK-04 — Nội dung chống đối/xúc phạm</summary>
        public bool? Kdk04 { get; set; }
        /// <summary>Bước 2: KĐK-05 — Đơn không thể đọc</summary>
        public bool? Kdk05 { get; set; }
        /// <summary>Bước 2: Ghi chú điều kiện</summary>
        public string GhiChuDieuKien { get; set; }
        /// <summary>Bước 3: Kết luận thẩm quyền đơn</summary>
        public int? ThamQuyenDon { get; set; }
        /// <summary>Bước 3: Ghi chú thẩm quyền</summary>
        public string GhiChuThamQuyen { get; set; }
        /// <summary>Bước 4: Loại đơn chính</summary>
        public int? LoaiDon { get; set; }
        /// <summary>Bước 4: Phân loại chi tiết tố cáo</summary>
        public int? PhanLoaiTc { get; set; }
        /// <summary>Bước 4: Lần tố cáo</summary>
        public int? LanTc { get; set; }
        /// <summary>Bước 4: Ủy quyền khiếu nại</summary>
        public bool? UyQuyenKn { get; set; }
        /// <summary>Bước 4: Lần khiếu nại</summary>
        public int? LanKn { get; set; }
        /// <summary>Bước 4: Lĩnh vực theo biểu 02/XLD của Cục Thuế</summary>
        public int? LinhVuc02Xld { get; set; }
        /// <summary>Bước 4: Phân loại chi tiết kiến nghị phản ánh</summary>
        public int? KNPADetail { get; set; }
        /// <summary>Bước 5: Kết quả xử lý chính</summary>
        public int? KetQuaXuLyChinh { get; set; }
        /// <summary>Bước 5: Số Phiếu chuyển</summary>
        public string SoPhieuChuyen { get; set; }
        /// <summary>Bước 5: Ngày Phiếu chuyển</summary>
        public DateTime? NgayPhieuChuyen { get; set; }
        /// <summary>Bước 5: Nơi nhận Phiếu chuyển</summary>
        public string NoiNhanPhieuChuyen { get; set; }
        /// <summary>Bước 5: Số Phiếu hướng dẫn</summary>
        public string SoPhieuHuongDan { get; set; }
        /// <summary>Bước 5: Ngày ban hành Phiếu hướng dẫn</summary>
        public DateTime? NgayPhieuHuongDan { get; set; }
        /// <summary>Bước 5: Số Quyết định/Thông báo thụ lý</summary>
        public string SoQdTbThuLy { get; set; }
        /// <summary>Bước 5: Ngày Quyết định/Thông báo thụ lý</summary>
        public DateTime? NgayQdTbThuLy { get; set; }
        /// <summary>Bước 5: Số Quyết định/Thông báo không thụ lý</summary>
        public string NonAcceptanceNo { get; set; }
        /// <summary>Bước 5: Ngày Quyết định/Thông báo không thụ lý</summary>
        public DateTime? NonAcceptanceDate { get; set; }
        /// <summary>Bước 5: Số Quyết định đình chỉ</summary>
        public string SuspensionNo { get; set; }
        /// <summary>Bước 5: Ngày Quyết định đình chỉ</summary>
        public DateTime? SuspensionDate { get; set; }
        /// <summary>Bước 5: Số Quyết định tạm đình chỉ</summary>
        public string TemporarySuspensionNo { get; set; }
        /// <summary>Bước 5: Ngày Quyết định tạm đình chỉ</summary>
        public DateTime? TemporarySuspensionDate { get; set; }
        /// <summary>Bước 6: Số Quyết định/Kết luận giải quyết đơn khiếu nại/tố cáo đã thụ lý</summary>
        public string ResolutionNo { get; set; }
        /// <summary>Bước 6: Ngày Quyết định/Kết luận giải quyết đơn khiếu nại/tố cáo</summary>
        public DateTime? ResolutionDate { get; set; }
        /// <summary>Bước 6: Tình trạng hồ sơ</summary>
        public int? CaseStatus { get; set; }
        /// <summary>Bước 6: Đúng hạn/Quá hạn</summary>
        public int? DeadlineStatus { get; set; }
        /// <summary>Bước 6: Lý do tồn</summary>
        public string PendingReason { get; set; }
        /// <summary>Bước 6: Kiến nghị thu hồi tiền</summary>
        public decimal? RecommendedRecoveryMoney { get; set; }
        /// <summary>Bước 6: Kiến nghị thu hồi đất m2</summary>
        public decimal? RecommendedRecoveryLand { get; set; }
        /// <summary>Bước 6: Kiến nghị trả lại tiền</summary>
        public decimal? RecommendedReturnMoney { get; set; }
        /// <summary>Bước 6: Kiến nghị trả lại đất m2</summary>
        public decimal? RecommendedReturnLand { get; set; }
        /// <summary>Bước 6: Kiến nghị xử lý hành chính (Tổ chức/cá nhân)</summary>
        public string RecommendedAdministrativeAction { get; set; }
        /// <summary>Bước 6: Đã thu hồi tiền</summary>
        public decimal? ActualRecoveryMoney { get; set; }
        /// <summary>Bước 6: Đã thu hồi đất m2</summary>
        public decimal? ActualRecoveryLand { get; set; }
        /// <summary>Bước 6: Chuyển cơ quan điều tra</summary>
        public int? RecommendedAdministrativeOrganizations { get; set; }
        public int? RecommendedAdministrativePersons { get; set; }
        public int? RecommendedAdministrativeOfficials { get; set; }
        public int? ActualAdministrativeOrganizations { get; set; }
        public int? ActualAdministrativePersons { get; set; }
        public int? ActualAdministrativeOfficials { get; set; }
        public int? InvestigationCases { get; set; }
        public int? InvestigationPersons { get; set; }
        public int? InvestigationOfficials { get; set; }
        public int? ProsecutionCases { get; set; }
        public int? ProsecutionPersons { get; set; }
        public int? ProsecutionOfficials { get; set; }
        public bool? InvestigationReferral { get; set; }
        /// <summary>Bước 6: Kết quả giải quyết đơn</summary>
        public int? ResolutionResult { get; set; }
        /// <summary>Bước 6: Đã trả lại tiền</summary>
        public decimal? ActualReturnMoney { get; set; }
        /// <summary>Bước 6: Đã trả lại đất m2</summary>
        public decimal? ActualReturnLand { get; set; }
        /// <summary>Bước 6: Đã xử lý hành chính (Tổ chức/cá nhân)</summary>
        public string ActualAdministrativeAction { get; set; }
        /// <summary>Bước 6: Đã khởi tố</summary>
        public string ProsecutionStatus { get; set; }
        /// <summary>Bước 6: Số công văn trả lời</summary>
        public string ResponseLetterNo { get; set; }
        /// <summary>Bước 6: Ngày công văn trả lời</summary>
        public DateTime? ResponseLetterDate { get; set; }
        /// <summary>Bước 7: Phiếu chuyển bị trả lại</summary>
        public bool? PhieuChuyenBiTraLai { get; set; }
        /// <summary>Bước 7: Lý do trả lại Phiếu chuyển</summary>
        public string LyDoTraLaiPhieuChuyen { get; set; }
        /// <summary>Bước 7: Số công văn báo cáo kết quả giải quyết đơn</summary>
        public string FollowupReportNo { get; set; }
        /// <summary>Bước 7: Ngày công văn báo cáo kết quả giải quyết đơn</summary>
        public DateTime? FollowupReportDate { get; set; }
        /// <summary>Bước 1: Nơi chuyển đơn</summary>
        public string TransferSource { get; set; }
        /// <summary>Bước 1: Số phiếu chuyển nguồn</summary>
        public string TransferSourceNo { get; set; }
        /// <summary>Bước 1: Ngày phiếu chuyển nguồn</summary>
        public DateTime? TransferSourceDate { get; set; }
    }
}
