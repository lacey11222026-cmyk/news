using System;
using System.Collections.Generic;
using UTILS;
using System.Data;
using System.Data.SqlClient;
using DATA.SMS;
using System.Globalization;
using System.Linq;
using System.Diagnostics;

namespace DATA.ContentDB
{
    public class VanThuDAL
    {
        public static List<VanThu> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
        {
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);
                pars[3] = new SqlParameter("@PageIndex", CurrPage);
                pars[4] = new SqlParameter("@PageSize", PageSize);
                pars[5] = new SqlParameter("@TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<VanThu>("sp_VanThu_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<VanThu>();
            }
        }

        public static List<VanThu> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                return new DBHelper(Configuration.HomeConnectionString).GetListSP<VanThu>("sp_VanThu_SelectDynamic", pars);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return new List<VanThu>();
            }
        }

        public static int Delete(int Id)
        {
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@_Id", Id);
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_VanThu_Delete", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }

        public static int InsertUpdate(VanThu functions)
        {
            try
            {
                var pars = new SqlParameter[87];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@DutyDate", (object)functions.DutyDate ?? DBNull.Value);
                pars[2] = new SqlParameter("@DutyLeaderName", (object)functions.DutyLeaderName ?? DBNull.Value);
                pars[3] = new SqlParameter("@ClericalOfficerName", (object)functions.ClericalOfficerName ?? DBNull.Value);
                pars[4] = new SqlParameter("@IncomingSubmissionNo", (object)functions.IncomingSubmissionNo ?? DBNull.Value);
                pars[5] = new SqlParameter("@IncomingDocumentType", (object)functions.IncomingDocumentType ?? DBNull.Value);
                pars[6] = new SqlParameter("@IncomingDocumentDetail", (object)functions.IncomingDocumentDetail ?? DBNull.Value);
                pars[7] = new SqlParameter("@ReceivedDate", (object)functions.ReceivedDate ?? DBNull.Value);
                pars[8] = new SqlParameter("@ReceivedNo", (object)functions.ReceivedNo ?? DBNull.Value);
                pars[9] = new SqlParameter("@Sender", (object)functions.Sender ?? DBNull.Value);
                pars[10] = new SqlParameter("@IncomingDocumentNo", (object)functions.IncomingDocumentNo ?? DBNull.Value);
                pars[11] = new SqlParameter("@IncomingDocumentDate", (object)functions.IncomingDocumentDate ?? DBNull.Value);
                pars[12] = new SqlParameter("@IncomingDocumentSummary", (object)functions.IncomingDocumentSummary ?? DBNull.Value);
                pars[13] = new SqlParameter("@LeaderName", (object)functions.LeaderName ?? DBNull.Value);
                pars[14] = new SqlParameter("@LeaderReceivedDate", (object)functions.LeaderReceivedDate ?? DBNull.Value);
                pars[15] = new SqlParameter("@AssignedOfficerName", (object)functions.AssignedOfficerName ?? DBNull.Value);
                pars[16] = new SqlParameter("@AssignedOfficerSigned", (object)functions.AssignedOfficerSigned ?? DBNull.Value);
                pars[17] = new SqlParameter("@AssignedOfficerReceivedDate", (object)functions.AssignedOfficerReceivedDate ?? DBNull.Value);
                pars[18] = new SqlParameter("@OutgoingDocumentType", (object)functions.OutgoingDocumentType ?? DBNull.Value);
                pars[19] = new SqlParameter("@OutgoingDocumentNo", (object)functions.OutgoingDocumentNo ?? DBNull.Value);
                pars[20] = new SqlParameter("@OutgoingDocumentDate", (object)functions.OutgoingDocumentDate ?? DBNull.Value);
                pars[21] = new SqlParameter("@OutgoingRecipient", (object)functions.OutgoingRecipient ?? DBNull.Value);
                pars[22] = new SqlParameter("@IsDenunciation", (object)functions.IsDenunciation ?? DBNull.Value);
                pars[23] = new SqlParameter("@IsComplaint", (object)functions.IsComplaint ?? DBNull.Value);
                pars[24] = new SqlParameter("@IsFeedbackPetition", (object)functions.IsFeedbackPetition ?? DBNull.Value);
                pars[25] = new SqlParameter("@IsMultiContentPetition", (object)functions.IsMultiContentPetition ?? DBNull.Value);
                pars[26] = new SqlParameter("@PetitionerNameTaxCode", (object)functions.PetitionerNameTaxCode ?? DBNull.Value);
                pars[27] = new SqlParameter("@IsRepeatedDenunciation", (object)functions.IsRepeatedDenunciation ?? DBNull.Value);
                pars[28] = new SqlParameter("@IsAuthorizedComplaint", (object)functions.IsAuthorizedComplaint ?? DBNull.Value);
                pars[29] = new SqlParameter("@IsHandledByInspection2", (object)functions.IsHandledByInspection2 ?? DBNull.Value);
                pars[30] = new SqlParameter("@ComplaintArea", (object)functions.ComplaintArea ?? DBNull.Value);
                pars[31] = new SqlParameter("@ComplaintTarget", (object)functions.ComplaintTarget ?? DBNull.Value);
                pars[32] = new SqlParameter("@PetitionContent", (object)functions.PetitionContent ?? DBNull.Value);
                pars[33] = new SqlParameter("@CaseCount", (object)functions.CaseCount ?? DBNull.Value);
                pars[34] = new SqlParameter("@IsPreviouslyResolved", (object)functions.IsPreviouslyResolved ?? DBNull.Value);
                pars[35] = new SqlParameter("@FieldPolicy", (object)functions.FieldPolicy ?? DBNull.Value);
                pars[36] = new SqlParameter("@FieldAdministrativeDecision", (object)functions.FieldAdministrativeDecision ?? DBNull.Value);
                pars[37] = new SqlParameter("@FieldOfficialAdministrativeAct", (object)functions.FieldOfficialAdministrativeAct ?? DBNull.Value);
                pars[38] = new SqlParameter("@FieldCorruption", (object)functions.FieldCorruption ?? DBNull.Value);
                pars[39] = new SqlParameter("@FieldTaxEvasion", (object)functions.FieldTaxEvasion ?? DBNull.Value);
                pars[40] = new SqlParameter("@FieldJustice", (object)functions.FieldJustice ?? DBNull.Value);
                pars[41] = new SqlParameter("@FieldPartyOrganization", (object)functions.FieldPartyOrganization ?? DBNull.Value);
                pars[42] = new SqlParameter("@FieldInspection", (object)functions.FieldInspection ?? DBNull.Value);
                pars[43] = new SqlParameter("@FieldOther", (object)functions.FieldOther ?? DBNull.Value);
                pars[44] = new SqlParameter("@DetailTaxRefund", (object)functions.DetailTaxRefund ?? DBNull.Value);
                pars[45] = new SqlParameter("@DetailTaxDebt", (object)functions.DetailTaxDebt ?? DBNull.Value);
                pars[46] = new SqlParameter("@DetailLand", (object)functions.DetailLand ?? DBNull.Value);
                pars[47] = new SqlParameter("@DetailCIT", (object)functions.DetailCIT ?? DBNull.Value);
                pars[48] = new SqlParameter("@DetailVAT", (object)functions.DetailVAT ?? DBNull.Value);
                pars[49] = new SqlParameter("@DetailPIT", (object)functions.DetailPIT ?? DBNull.Value);
                pars[50] = new SqlParameter("@DetailInvoice", (object)functions.DetailInvoice ?? DBNull.Value);
                pars[51] = new SqlParameter("@DetailTaxExemption", (object)functions.DetailTaxExemption ?? DBNull.Value);
                pars[52] = new SqlParameter("@DetailOther", (object)functions.DetailOther ?? DBNull.Value);
                pars[53] = new SqlParameter("@ProposalSubmissionDate", (object)functions.ProposalSubmissionDate ?? DBNull.Value);
                pars[54] = new SqlParameter("@IsDuplicateResolvedPetition", (object)functions.IsDuplicateResolvedPetition ?? DBNull.Value);
                pars[55] = new SqlParameter("@EligibleCaseCount", (object)functions.EligibleCaseCount ?? DBNull.Value);
                pars[56] = new SqlParameter("@IneligibleCaseCount", (object)functions.IneligibleCaseCount ?? DBNull.Value);
                pars[57] = new SqlParameter("@WithinAuthorityCaseCount", (object)functions.WithinAuthorityCaseCount ?? DBNull.Value);
                pars[58] = new SqlParameter("@OutsideAuthorityCaseCount", (object)functions.OutsideAuthorityCaseCount ?? DBNull.Value);
                pars[59] = new SqlParameter("@IsPetitionFiled", (object)functions.IsPetitionFiled ?? DBNull.Value);
                pars[60] = new SqlParameter("@GuidanceDocumentNo", (object)functions.GuidanceDocumentNo ?? DBNull.Value);
                pars[61] = new SqlParameter("@GuidanceDocumentDate", (object)functions.GuidanceDocumentDate ?? DBNull.Value);
                pars[62] = new SqlParameter("@TransferSlipNo", (object)functions.TransferSlipNo ?? DBNull.Value);
                pars[63] = new SqlParameter("@TransferSlipDate", (object)functions.TransferSlipDate ?? DBNull.Value);
                pars[64] = new SqlParameter("@TransferRecipient", (object)functions.TransferRecipient ?? DBNull.Value);
                pars[65] = new SqlParameter("@ReminderDocumentNo", (object)functions.ReminderDocumentNo ?? DBNull.Value);
                pars[66] = new SqlParameter("@ReminderDocumentDate", (object)functions.ReminderDocumentDate ?? DBNull.Value);
                pars[67] = new SqlParameter("@ReportToInspection2No", (object)functions.ReportToInspection2No ?? DBNull.Value);
                pars[68] = new SqlParameter("@ReportToInspection2Date", (object)functions.ReportToInspection2Date ?? DBNull.Value);
                pars[69] = new SqlParameter("@NonAcceptanceNoticeNo", (object)functions.NonAcceptanceNoticeNo ?? DBNull.Value);
                pars[70] = new SqlParameter("@NonAcceptanceNoticeDate", (object)functions.NonAcceptanceNoticeDate ?? DBNull.Value);
                pars[71] = new SqlParameter("@AcceptanceDecisionNo", (object)functions.AcceptanceDecisionNo ?? DBNull.Value);
                pars[72] = new SqlParameter("@AcceptanceDecisionDate", (object)functions.AcceptanceDecisionDate ?? DBNull.Value);
                pars[73] = new SqlParameter("@VerificationDecisionNo", (object)functions.VerificationDecisionNo ?? DBNull.Value);
                pars[74] = new SqlParameter("@VerificationDecisionDate", (object)functions.VerificationDecisionDate ?? DBNull.Value);
                pars[75] = new SqlParameter("@IsComplexCase", (object)functions.IsComplexCase ?? DBNull.Value);
                pars[76] = new SqlParameter("@TemporarySuspensionDecisionNo", (object)functions.TemporarySuspensionDecisionNo ?? DBNull.Value);
                pars[77] = new SqlParameter("@TemporarySuspensionDecisionDate", (object)functions.TemporarySuspensionDecisionDate ?? DBNull.Value);
                pars[78] = new SqlParameter("@SuspensionDecisionNo", (object)functions.SuspensionDecisionNo ?? DBNull.Value);
                pars[79] = new SqlParameter("@SuspensionDecisionDate", (object)functions.SuspensionDecisionDate ?? DBNull.Value);
                pars[80] = new SqlParameter("@IsDialogue", (object)functions.IsDialogue ?? DBNull.Value);
                pars[81] = new SqlParameter("@ResolutionDecisionNo", (object)functions.ResolutionDecisionNo ?? DBNull.Value);
                pars[82] = new SqlParameter("@ResolutionDecisionDate", (object)functions.ResolutionDecisionDate ?? DBNull.Value);
                pars[83] = new SqlParameter("@IsComplaintCorrect", (object)functions.IsComplaintCorrect ?? DBNull.Value);
                pars[84] = new SqlParameter("@IsComplaintIncorrect", (object)functions.IsComplaintIncorrect ?? DBNull.Value);
                pars[85] = new SqlParameter("@IsComplaintPartiallyCorrect", (object)functions.IsComplaintPartiallyCorrect ?? DBNull.Value);
                pars[86] = new SqlParameter("@ProcessingStatusNote", (object)functions.ProcessingStatusNote ?? DBNull.Value);
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_VanThu_InsertUpdate", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }

        public static VanThu GetDetail(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var orderBy = string.Empty;
            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
    }
}
