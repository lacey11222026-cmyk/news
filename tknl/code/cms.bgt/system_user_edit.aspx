<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" Inherits="CMS.system_user_edit" Title="Untitled Page" CodeBehind="system_user_edit.aspx.cs" %>

<%@ Register Src="controls/system_menu.ascx" TagName="system_menu" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="text">
        <tr valign="top">
            <td id="tdLeft" style="width: 200px;">
                <uc1:system_menu ID="System_menu1" runat="server" />
            </td>
            <td id="tdRightContent" style="padding-left: 10px;" valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" class="text" border="0">
                    <tr valign="top" style="height: 20px">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr valign="top">
                                    <td style="width: 10px">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top1.gif" /></td>
                                    <td class="title" style="background-image: url('<%#UrlRoot %>images/top2.gif'); padding-top: 3px; padding-left: 5px; color: #ffffff"
                                        id="tdParentTitle">
                                        <asp:Literal ID="LTL_HEADER" runat="server" Text="Sửa thông tin người dùng"></asp:Literal></td>
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
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" ClientEvents-OnRequestStart="OnRequestStart"
                                            ClientEvents-OnResponseEnd="OnResponseEnd" LoadingPanelID="AjaxLoadingPanel1">
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr valign="top">
                                                    <td>
                                                        <table cellpadding="1" cellspacing="1" border="0" class="text">
                                                            <tr>
                                                                <td style="width: 120px">Tên đăng nhập (*):
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td style="color: Red;">
                                                                    <strong>
                                                                        <asp:Literal ID="ltlError" runat="server"></asp:Literal></strong>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Họ tên (*):
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFullname" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Email:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Ngày sinh (*):
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="txtDatebirth" runat="server" Skin="Default">
                                                                        <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr valign="middle">
                                                                <td>Giới tính:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboGender" runat="server">
                                                                        <asp:ListItem Selected="true" Value="1" Text="Nam"></asp:ListItem>
                                                                        <asp:ListItem Value="0" Text="Nữ"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Điện thoại:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTel" runat="server" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Địa chỉ
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAddress" TextMode="MultiLine" Width="300" runat="server" MaxLength="1000"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <fieldset>
                                                                        <legend class="text"><strong>Quản trị hệ thống</strong></legend>
                                                                        <div style="width: 375px; height: 200px; overflow: auto; text-align: justify">
                                                                            <asp:Repeater ID="rptSysFunc" runat="server" OnItemDataBound="rptSysFunc_ItemDataBound">
                                                                                <HeaderTemplate>
                                                                                    <table border="0" cellspacing="1" cellpadding="1" class="text">
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td style="width: 20px">
                                                                                            <asp:CheckBox ID="cbxFunctionID" runat="server" /><asp:HiddenField ID="hiddenID"
                                                                                                runat="server" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Literal ID="ltlFunctionName" runat="server"></asp:Literal>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    </table>
                                                                                </FooterTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                    </fieldset>
                                                                </td>
                                                                <%--  <td></td>
                                                            </tr>
                                                            <tr>--%>
                                                                <td colspan="2">
                                                                    <fieldset>
                                                                        <legend class="text"><strong>Biên tập tin bài</strong></legend>
                                                                        <div style="width: 375px; height: 200px; overflow: auto; text-align: justify">
                                                                            <asp:TreeView ID="treePartWorkflow" runat="server" OnTreeNodePopulate="treePartWorkflow_TreeNodePopulate">
                                                                                <NodeStyle CssClass="text" />
                                                                                <LeafNodeStyle CssClass="text" />
                                                                            </asp:TreeView>
                                                                        </div>
                                                                    </fieldset>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Nội dung giao lưu</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlDiscuss" runat="server" DataTextField="Title" DataValueField="DiscussId"
                                                                        AutoPostBack="true" Width="540px" OnSelectedIndexChanged="ddlDiscuss_SelectedIndexChanged">
                                                                    </asp:DropDownList></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Nhân vật giao lưu</td>
                                                                <td>
                                                                    <asp:DropDownList ID="dllGuest" runat="server" DataTextField="Title" DataValueField="DiscussId"
                                                                        AutoPostBack="false" Width="540px">
                                                                    </asp:DropDownList></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Ghi chú
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDesc" TextMode="MultiLine" Width="300" runat="server" MaxLength="1000"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Kích hoạt:
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkActive" runat="server" />
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 10px"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="padding-right: 5px;">
                                                                    <asp:Button ID="btnUpdate" runat="server" CssClass="button" Text="Ghi lại" OnClick="btnUpdate_Click" /><asp:HiddenField
                                                                        ID="hfResult" runat="server" Value="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 10px"></td>
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
                                    <td style="background-image: url('<%#UrlRoot %>images/bottom2.gif'); width: 100%"></td>
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
        function OnRequestStart(sender, args) {
            if (args.EventTarget.indexOf("btnUpdate") >= 0) {
                if (document.getElementById("<%# txtUsername.ClientID %>").value == "") {
                    alert("Phải nhập tên đăng nhập!");
                    document.getElementById("<%# txtUsername.ClientID %>").focus();
            return false;
        }

        if (document.getElementById("<%# txtFullname.ClientID %>").value == "") {
                    alert("Phải nhập họ tên!");
                    document.getElementById("<%# txtFullname.ClientID %>").focus();
            return false;
        }
        if (document.getElementById("<%# txtDatebirth.ClientID %>").value == "") {
                    alert("Phải nhập ngày sinh!");
                    document.getElementById("<%# txtDatebirth.ClientID %>").focus();
            return false;
        }
    }
}
function OnResponseEnd(sender, args) {
    var result = document.getElementById("<%# hfResult.ClientID %>").value;
    if (args.EventTarget.indexOf("btnUpdate") >= 0 && result == "1") {
        location.href = '<%# UrlRoot %>system/user/index.htm';
    }
}

    </script>

</asp:Content>
