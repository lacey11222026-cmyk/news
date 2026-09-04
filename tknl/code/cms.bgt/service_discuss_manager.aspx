<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" Inherits="CMS.service_discuss_manager" Codebehind="service_discuss_manager.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register Src="~/controls/service_menu.ascx" TagName="service_menu" TagPrefix="uc1" %>--%>
<%@ Register Src="~/controls/discuss_menu.ascx" TagName="discuss_menu" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="text" id="tblContent">
        <tr valign="top">
            <td id="tdLeft" style="width: 200px;">
                <uc1:discuss_menu ID="discuss_menu1" runat="server" />
            </td>
            <td id="tdRightContent" style="padding-left: 10px;" valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" class="text" border="0">
                    <tr valign="top" style="height: 20px">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr valign="top">
                                    <td style="width: 10px">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top1.gif" />
                                    </td>
                                    <td class="title" style="background-image: url('<%# UrlRoot %>images/top2.gif');
                                        padding-top: 3px; padding-left: 5px; color: #ffffff" id="tdParentTitle">
                                        <asp:Literal ID="LTL_HEADER" runat="server" Text="Quản lý thông tin giao lưu trực tuyến"></asp:Literal>
                                    </td>
                                    <td style="width: 10px">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top3.gif" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top" style="background-color: #354157;">
                        <td style="padding-left: 1px; padding-right: 1px">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #ffffff">
                                <tr valign="top">
                                    <td style="padding-left: 10px; padding-right: 10px">
                                     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" ClientEvents-OnRequestStart="OnRequestStart"
                                            ClientEvents-OnResponseEnd="OnResponseEnd" LoadingPanelID="AjaxLoadingPanel1">
                                         <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                             <tr valign="top">
                                                 <td>
                                                     <asp:Repeater ID="rptData" runat="server" OnItemDataBound="rptData_ItemDataBound"
                                                         OnItemCreated="rptData_ItemCreated">
                                                         <HeaderTemplate>
                                                             <table width="100%" border="0" cellspacing="1" cellpadding="1" style="background-color: #E8EDF6"
                                                                 class="text">
                                                                 <tr class="header">
                                                                     <td align="center" style="width: 30px">
                                                                         <asp:CheckBox ID="cbxHeaderSelect" runat="server" AutoPostBack="false" />
                                                                     </td>
                                                                     <td>
                                                                         <asp:Literal ID="ltlHeaderName" runat="server" Text="Tiêu đề"></asp:Literal>
                                                                     </td>
                                                                     <td align="center" style="width: 60px">
                                                                         <asp:Literal ID="ltlHeaderStatus" runat="server" Text="Trạng thái"></asp:Literal>
                                                                     </td>
                                                                     <td align="center" style="width: 45px">
                                                                         <asp:Literal ID="ltlEdit" runat="server" Text="Sửa"></asp:Literal>
                                                                     </td>
                                                                     <td>Xem trước</td>
                                                                 </tr>
                                                         </HeaderTemplate>
                                                         <ItemTemplate>
                                                             <tr class="item" id="trItem" runat="server">
                                                                 <td align="center" style="width: 30px">
                                                                     <asp:CheckBox ID="cbxSelect" runat="server" /><asp:HiddenField ID="hiddenID" runat="server" />
                                                                 </td>
                                                                 <td>
                                                                     <asp:Literal ID="ltlName" runat="server" Text="Tiêu đề"></asp:Literal>
                                                                 </td>
                                                                 <td align="center">
                                                                     <asp:ImageButton ID="iBtnStatus" runat="server" BorderWidth="0" />
                                                                 </td>
                                                                 <td align="center">
                                                                     <asp:HyperLink ID="hlEdit" runat="server"><img src="<%# UrlRoot %>icons/edit.gif" style="border:0px" alt="Sửa" /></asp:HyperLink>
                                                                 </td>
                                                                 <td>
                                                                     <a target="_blank" href="<%= ConfigurationManager.AppSettings["SITE_URL"] %>giao-luu-preview-id<%# Eval("DiscussId") %>.html">Xem trước</a>
                                                                 </td>
                                                             </tr>
                                                         </ItemTemplate>
                                                         <FooterTemplate>
                                                             </table>
                                                         </FooterTemplate>
                                                     </asp:Repeater>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td>
                                                     <table cellpadding="0" cellspacing="0">
                                                         <tr>
                                                             <td style="padding-right: 5px;">
                                                                 <asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Xóa" OnClick="btnDelete_Click" />
                                                             </td>
                                                         </tr>
                                                     </table>
                                                 </td>
                                             </tr>
                                         </table>
                                            </telerik:RadAjaxPanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="bottom" style="height: 5px;">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td style="width: 1px">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom1.gif" />
                                    </td>
                                    <td style="background-image: url('<%# UrlRoot %>images/bottom2.gif'); width: 100%">
                                    </td>
                                    <td style="width: 1px">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom3.gif" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server" Transparency="10"
        MinDisplayTime="300">
        <img src="<%# UrlRoot %>images/loading.gif" alt="Loading" style="border: 0px; vertical-align: middle;" />
    </telerik:RadAjaxLoadingPanel>
    <script type="text/javascript">
        var theForm = document.forms[0];
        function SelectAll(objClick, objRelated) {
            var obj = document.getElementById(objClick);
            var chk = obj.checked;
            var len = theForm.elements.length;
            for (var j = 0; j < len; j++) {
                var e = theForm.elements[j];
                if (e.name.indexOf(objRelated) >= 0) {
                    e.checked = chk;
                }
            }
        }
        function CheckSelected(objRelated) {
            var bRet = false;
            var len = theForm.elements.length;
            for (var j = 0; j < len; j++) {
                var e = theForm.elements[j];
                if (e.name.indexOf(objRelated) >= 0 && e.checked) {
                    bRet = true;
                    break;
                }
            }
            if (!bRet) {
                alert("Phải chọn ít nhất 1 bản ghi!");
            }
            return bRet;
        }
        function ConfirmDelete(objRelated) {
            if (CheckSelected(objRelated)) {
                return confirm("Bạn có thực sự muốn xóa không?");
            }
            return false;
        }
        function OnRequestStart(sender, args) {
            if (args.EventTarget.indexOf("btnDelete") >= 0) {
                return ConfirmDelete('cbxSelect');
            }
        };

        function OnResponseEnd(sender, args) {

        };
    </script>
</asp:Content>

