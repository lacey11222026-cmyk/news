using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.ContentDB
{
    public class VanThu
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 1. Ngày trực
        /// </summary>
        public DateTime? DutyDate { get; set; }

        /// <summary>
        /// 2. Tên Trưởng nhóm trực
        /// </summary>
        public string DutyLeaderName { get; set; }

        /// <summary>
        /// 3. Tên công chức trực văn thư
        /// </summary>
        public string ClericalOfficerName { get; set; }

        /// <summary>
        /// 4. Số phiếu trình văn bản đến của Phòng TTKT số 2
        /// </summary>
        public string IncomingSubmissionNo { get; set; }

        /// <summary>
        /// 5. Loại văn bản đến
        /// </summary>
        public int? IncomingDocumentType { get; set; }

        /// <summary>
        /// 6. Phân loại văn bản
        /// </summary>
        public int? IncomingDocumentDetail { get; set; }

        /// <summary>
        /// 7. Ngày đến văn thư Thuế TP Hà Nội
        /// </summary>
        public DateTime? ReceivedDate { get; set; }

        /// <summary>
        /// 8. Số đến Văn thư Thuế TP Hà Nội
        /// </summary>
        public string ReceivedNo { get; set; }

        /// <summary>
        /// 9. Nơi gửi
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 10. Số và ký hiệu văn bản
        /// </summary>
        public string IncomingDocumentNo { get; set; }

        /// <summary>
        /// 11. Ngày tháng văn bản
        /// </summary>
        public DateTime? IncomingDocumentDate { get; set; }

        /// <summary>
        /// 12. Trích yếu văn bản
        /// </summary>
        public string IncomingDocumentSummary { get; set; }

        /// <summary>
        /// 13. Lãnh đạo phụ trách - Tên
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// 14. Lãnh đạo phụ trách - Ngày nhận
        /// </summary>
        public DateTime? LeaderReceivedDate { get; set; }

        /// <summary>
        /// 15. Cán bộ được giao giải quyết - Tên cán bộ
        /// </summary>
        public string AssignedOfficerName { get; set; }

        /// <summary>
        /// 16. Cán bộ được giao giải quyết - Ký nhận
        /// </summary>
        public bool? AssignedOfficerSigned { get; set; }

        /// <summary>
        /// 17. Cán bộ được giao giải quyết - Ngày nhận
        /// </summary>
        public DateTime? AssignedOfficerReceivedDate { get; set; }

        /// <summary>
        /// 18. Loại văn bản đi
        /// </summary>
        public string OutgoingDocumentType { get; set; }

        /// <summary>
        /// 19. Số văn bản đi
        /// </summary>
        public string OutgoingDocumentNo { get; set; }

        /// <summary>
        /// 20. Ngày văn bản đi
        /// </summary>
        public DateTime? OutgoingDocumentDate { get; set; }

        /// <summary>
        /// 21. Nơi nhận
        /// </summary>
        public string OutgoingRecipient { get; set; }

        /// <summary>
        /// 22. Tố cáo
        /// </summary>
        public bool? IsDenunciation { get; set; }

        /// <summary>
        /// 23. Khiếu nại
        /// </summary>
        public bool? IsComplaint { get; set; }

        /// <summary>
        /// 24. Phản ánh, kiến nghị
        /// </summary>
        public bool? IsFeedbackPetition { get; set; }

        /// <summary>
        /// 25. Đơn nhiều nội dung
        /// </summary>
        public bool? IsMultiContentPetition { get; set; }

        /// <summary>
        /// 26. Tên tổ chức/cá nhân gửi đơn - MST
        /// </summary>
        public string PetitionerNameTaxCode { get; set; }

        /// <summary>
        /// 27. Tố cáo tiếp
        /// </summary>
        public bool? IsRepeatedDenunciation { get; set; }

        /// <summary>
        /// 28. Uỷ quyền KN, KNPA
        /// </summary>
        public bool? IsAuthorizedComplaint { get; set; }

        /// <summary>
        /// 29. Phòng TTKT2 giải quyết
        /// </summary>
        public bool? IsHandledByInspection2 { get; set; }

        /// <summary>
        /// 30. Địa bàn bị KN, TC, KNPA
        /// </summary>
        public string ComplaintArea { get; set; }

        /// <summary>
        /// 31. Đối tượng bị tố cáo / kiến nghị
        /// </summary>
        public string ComplaintTarget { get; set; }

        /// <summary>
        /// 32. Nội dung đơn
        /// </summary>
        public string PetitionContent { get; set; }

        /// <summary>
        /// 33. Số vụ việc KN, TC, PAKN
        /// </summary>
        public int? CaseCount { get; set; }

        /// <summary>
        /// 34. Đã được cơ quan có thẩm quyền giải quyết
        /// </summary>
        public bool? IsPreviouslyResolved { get; set; }

        /// <summary>
        /// 35. Chế độ, chính sách
        /// </summary>
        public bool? FieldPolicy { get; set; }

        /// <summary>
        /// 36. Quyết định hành chính bị khiếu nại
        /// </summary>
        public bool? FieldAdministrativeDecision { get; set; }

        /// <summary>
        /// 37. Hành vi hành chính của CBCC
        /// </summary>
        public bool? FieldOfficialAdministrativeAct { get; set; }

        /// <summary>
        /// 38. Hành vi tham nhũng
        /// </summary>
        public bool? FieldCorruption { get; set; }

        /// <summary>
        /// 39. Hành vi trốn thuế
        /// </summary>
        public bool? FieldTaxEvasion { get; set; }

        /// <summary>
        /// 40. Tư pháp
        /// </summary>
        public bool? FieldJustice { get; set; }

        /// <summary>
        /// 41. Đảng, đoàn thể
        /// </summary>
        public bool? FieldPartyOrganization { get; set; }

        /// <summary>
        /// 42. Thanh tra, kiểm tra
        /// </summary>
        public bool? FieldInspection { get; set; }

        /// <summary>
        /// 43. Lĩnh vực khác
        /// </summary>
        public bool? FieldOther { get; set; }

        /// <summary>
        /// 44. Hoàn thuế
        /// </summary>
        public bool? DetailTaxRefund { get; set; }

        /// <summary>
        /// 45. Nợ thuế
        /// </summary>
        public bool? DetailTaxDebt { get; set; }

        /// <summary>
        /// 46. Đất
        /// </summary>
        public bool? DetailLand { get; set; }

        /// <summary>
        /// 47. Thuế TNDN
        /// </summary>
        public bool? DetailCIT { get; set; }

        /// <summary>
        /// 48. Thuế GTGT
        /// </summary>
        public bool? DetailVAT { get; set; }

        /// <summary>
        /// 49. Thuế TNCN
        /// </summary>
        public bool? DetailPIT { get; set; }

        /// <summary>
        /// 50. Hóa đơn
        /// </summary>
        public bool? DetailInvoice { get; set; }

        /// <summary>
        /// 51. Miễn giảm
        /// </summary>
        public bool? DetailTaxExemption { get; set; }

        /// <summary>
        /// 52. Khác
        /// </summary>
        public bool? DetailOther { get; set; }

        /// <summary>
        /// 53. Ngày trình phiếu VB đi / đề xuất xử lý đơn
        /// </summary>
        public DateTime? ProposalSubmissionDate { get; set; }

        /// <summary>
        /// 54. Đơn trùng với nội dung đơn đã được giải quyết
        /// </summary>
        public bool? IsDuplicateResolvedPetition { get; set; }

        /// <summary>
        /// 55. Đơn đủ điều kiện xử lý - Số vụ việc
        /// </summary>
        public int? EligibleCaseCount { get; set; }

        /// <summary>
        /// 56. Đơn không đủ điều kiện xử lý - Số vụ việc
        /// </summary>
        public int? IneligibleCaseCount { get; set; }

        /// <summary>
        /// 57. Đơn thuộc thẩm quyền CQT nhận đơn - Số vụ việc
        /// </summary>
        public int? WithinAuthorityCaseCount { get; set; }

        /// <summary>
        /// 58. Đơn không thuộc thẩm quyền CQT nhận đơn - Số vụ việc
        /// </summary>
        public int? OutsideAuthorityCaseCount { get; set; }

        /// <summary>
        /// 59. Lưu đơn
        /// </summary>
        public bool? IsPetitionFiled { get; set; }

        /// <summary>
        /// 60. Công văn hướng dẫn / Phiếu hướng dẫn / CV trả lời PAKN - Số
        /// </summary>
        public string GuidanceDocumentNo { get; set; }

        /// <summary>
        /// 61. Công văn hướng dẫn / Phiếu hướng dẫn / CV trả lời PAKN - Ngày tháng ban hành
        /// </summary>
        public DateTime? GuidanceDocumentDate { get; set; }

        /// <summary>
        /// 62. Phiếu chuyển - Số
        /// </summary>
        public string TransferSlipNo { get; set; }

        /// <summary>
        /// 63. Phiếu chuyển - Ngày tháng ban hành
        /// </summary>
        public DateTime? TransferSlipDate { get; set; }

        /// <summary>
        /// 64. Phiếu chuyển - Nơi nhận văn bản
        /// </summary>
        public string TransferRecipient { get; set; }

        /// <summary>
        /// 65. Văn bản đôn đốc báo cáo - Số
        /// </summary>
        public string ReminderDocumentNo { get; set; }

        /// <summary>
        /// 66. Văn bản đôn đốc báo cáo - Ngày tháng ban hành
        /// </summary>
        public DateTime? ReminderDocumentDate { get; set; }

        /// <summary>
        /// 67. Văn bản báo cáo về Phòng TTKT2 - Số
        /// </summary>
        public string ReportToInspection2No { get; set; }

        /// <summary>
        /// 68. Văn bản báo cáo về Phòng TTKT2 - Ngày tháng ban hành
        /// </summary>
        public DateTime? ReportToInspection2Date { get; set; }

        /// <summary>
        /// 69. Thông báo không thụ lý - Số
        /// </summary>
        public string NonAcceptanceNoticeNo { get; set; }

        /// <summary>
        /// 70. Thông báo không thụ lý - Ngày tháng ban hành
        /// </summary>
        public DateTime? NonAcceptanceNoticeDate { get; set; }

        /// <summary>
        /// 71. Quyết định thụ lý TC / Thông báo thụ lý khiếu nại - Số
        /// </summary>
        public string AcceptanceDecisionNo { get; set; }

        /// <summary>
        /// 72. Quyết định thụ lý TC / Thông báo thụ lý khiếu nại - Ngày tháng ban hành
        /// </summary>
        public DateTime? AcceptanceDecisionDate { get; set; }

        /// <summary>
        /// 73. Quyết định xác minh nội dung KNTC - Số
        /// </summary>
        public string VerificationDecisionNo { get; set; }

        /// <summary>
        /// 74. Quyết định xác minh nội dung KNTC - Ngày tháng năm
        /// </summary>
        public DateTime? VerificationDecisionDate { get; set; }

        /// <summary>
        /// 75. Vụ việc phức tạp / đặc biệt phức tạp
        /// </summary>
        public bool? IsComplexCase { get; set; }

        /// <summary>
        /// 76. Quyết định tạm đình chỉ giải quyết đơn - Số
        /// </summary>
        public string TemporarySuspensionDecisionNo { get; set; }

        /// <summary>
        /// 77. Quyết định tạm đình chỉ giải quyết đơn - Ngày tháng ban hành
        /// </summary>
        public DateTime? TemporarySuspensionDecisionDate { get; set; }

        /// <summary>
        /// 78. Quyết định đình chỉ giải quyết đơn - Số
        /// </summary>
        public string SuspensionDecisionNo { get; set; }

        /// <summary>
        /// 79. Quyết định đình chỉ giải quyết đơn - Ngày tháng ban hành
        /// </summary>
        public DateTime? SuspensionDecisionDate { get; set; }

        /// <summary>
        /// 80. Đối thoại
        /// </summary>
        public bool? IsDialogue { get; set; }

        /// <summary>
        /// 81. QĐ giải quyết khiếu nại / KL giải quyết tố cáo - Số
        /// </summary>
        public string ResolutionDecisionNo { get; set; }

        /// <summary>
        /// 82. QĐ giải quyết khiếu nại / KL giải quyết tố cáo - Ngày tháng
        /// </summary>
        public DateTime? ResolutionDecisionDate { get; set; }

        /// <summary>
        /// 83. KNTC, PAKN đúng
        /// </summary>
        public bool? IsComplaintCorrect { get; set; }

        /// <summary>
        /// 84. KNTC, PAKN sai
        /// </summary>
        public bool? IsComplaintIncorrect { get; set; }

        /// <summary>
        /// 85. KNTC, PAKN đúng một phần
        /// </summary>
        public bool? IsComplaintPartiallyCorrect { get; set; }

        /// <summary>
        /// 86. Ghi chú rõ trạng thái hồ sơ đang giải quyết
        /// </summary>
        public string ProcessingStatusNote { get; set; }
    }
}