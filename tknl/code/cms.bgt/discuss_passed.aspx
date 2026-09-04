<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" Inherits="CMS.discuss_passed" CodeBehind="discuss_passed.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="controls/discuss_menu.ascx" TagName="discuss_menu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top1.gif" /></td>
                                    <td class="title" style="background-image: url('<%# UrlRoot %>images/top2.gif'); padding-top: 3px; padding-left: 5px; color: #ffffff"
                                        id="tdParentTitle">
                                        <asp:Literal ID="LTL_HEADER" runat="server" Text="Tin bài đã được gửi lên"></asp:Literal>
                                    </td>
                                    <td style="width: 10px">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top3.gif" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top" style="background-color: #354157;">
                        <td style="padding-left: 1px; padding-right: 1px">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #ffffff">
                                <tr valign="top">
                                    <td style="padding-left: 10px; padding-right: 10px">
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" LoadingPanelID="AjaxLoadingPanel1" runat="server"
                                            Width="100%" ClientEvents-OnRequestStart="OnRequestStart"
                                            ClientEvents-OnResponseEnd="OnResponseEnd">
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr valign="top">
                                                    <td align="center">
                                                        <table cellpadding="1" cellspacing="1" border="0" class="text">
                                                            <tr>
                                                                <td>Từ ngày:
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDateTimePicker ID="txtDateFrom" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss"
                                                                        DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                                                    </telerik:RadDateTimePicker>
                                                                </td>
                                                                <td>Đến ngày:
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDateTimePicker ID="txtDateTo" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss"
                                                                        DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                                                    </telerik:RadDateTimePicker>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Cuộc giao lưu:
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:DropDownList ID="ddlDiscuss" runat="server" DataTextField="Title" DataValueField="DiscussId"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDiscuss_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="right">
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Nhân vật giao lưu</td>
                                                                <td colspan="2">
                                                                    <asp:DropDownList ID="ddlGuest" runat="server" Width="100%"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlGuest_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td><asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" OnClick="btnSearch_Click" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 10px"></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <table cellpadding="0" cellspacing="0" class="text">
                                                            <tr>
                                                                <td style="padding-right: 10px">
                                                                    <strong>
                                                                        <asp:Literal ID="ltlTotal1" runat="server"></asp:Literal></strong>
                                                                </td>
                                                                <td valign="middle">
                                                                    <asp:DataList ID="dlPaper1" runat="server" RepeatColumns="9" OnItemCreated="dlPaper_ItemCreated"
                                                                        OnItemDataBound="dlPaper_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbtPage" runat="server" Text="1"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:DataList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 10px"></td>
                                                </tr>
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
                                                                            <asp:Literal ID="ltlHeaderQuestion" runat="server" Text="Câu hỏi"></asp:Literal>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="ltlHeaderAnswer" runat="server" Text="Nội dung trả lời"></asp:Literal>
                                                                        </td>
                                                                        <td align="center" style="width: 45px">
                                                                            <asp:Literal ID="ltlHeaderMoveUp" runat="server" Text="Gửi lên"></asp:Literal>
                                                                        </td>
                                                                        <td align="center" style="width: 45px">
                                                                            <asp:Literal ID="ltlHeaderMoveDown" runat="server" Text="Trả lại"></asp:Literal>
                                                                        </td>
                                                                        <td align="center" style="width: 45px">
                                                                            <asp:Literal ID="ltlHeaderEdit" runat="server" Text="Trả lời"></asp:Literal>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="Literal1" runat="server" Text="Mới nhất"></asp:Literal>
                                                                        </td>
                                                                        <td align="center" style="width: 45px">
                                                                            <asp:Literal ID="ltlHeaderDelete" runat="server" Text="Xóa"></asp:Literal>
                                                                        </td>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="item" id="trItem" runat="server">
                                                                    <td align="center" style="width: 30px">
                                                                        <asp:CheckBox ID="cbxSelect" runat="server" /><asp:HiddenField ID="hiddenID" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltlQuestion" runat="server" Text="Câu hỏi"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <div><asp:Literal ID="ltlAnswer" runat="server" Text="Trả lời"></asp:Literal></div>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="iBtnMoveUp" runat="server" BorderWidth="0" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="iBtnMoveDown" runat="server" BorderWidth="0" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:HyperLink ID="hlEdit" runat="server"><img src="<%# UrlRoot %>icons/edit.gif" style="border:0px" alt="Sửa" /></asp:HyperLink>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton ID="hlUpdate" runat="server">Mới nhất</asp:LinkButton>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="iBtnDelete" runat="server" BorderWidth="0" />
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
                                                    <td style="height: 10px"></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <table cellpadding="0" cellspacing="0" class="text">
                                                            <tr>
                                                                <td style="padding-right: 10px">
                                                                    <strong>
                                                                        <asp:Literal ID="ltlTotal" runat="server"></asp:Literal></strong>
                                                                </td>
                                                                <td valign="middle">
                                                                    <asp:DataList ID="dlPaper" runat="server" RepeatColumns="9"
                                                                        OnItemCreated="dlPaper_ItemCreated"
                                                                        OnItemDataBound="dlPaper_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbtPage" runat="server" Text="1"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:DataList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadAjaxPanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="bottom" style="height: 5px;">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td style="width: 1px">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom1.gif" /></td>
                                    <td style="background-image: url('<%# UrlRoot %>images/bottom2.gif'); width: 100%"></td>
                                    <td style="width: 1px">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom3.gif" /></td>
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
            var c = theForm.getElementsByTagName('input');
            var l = c.length;
            for (var i = 0; i < l; i++) {
                if (c[i].name.indexOf(objRelated) >= 0)
                    c[i].checked = chk;
            }
        }

        function OnRequestStart(sender, args) {
        }
        function OnResponseEnd(sender, args) {
            ActualResize('tblContent');
        }
    </script>
</asp:Content>

