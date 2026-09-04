<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeBehind="discuss_user_add.aspx.cs" Inherits="CMS2012.discuss_user_add" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/controls/discuss_menu.ascx" TagName="discuss_menu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                    <td class="title" style="background-image: url('<%# UrlRoot %>images/top2.gif'); padding-top: 3px; padding-left: 5px; color: #ffffff"
                                        id="tdParentTitle">
                                        <asp:Literal ID="LTL_HEADER" runat="server" Text="Thêm mới giao lưu"></asp:Literal>
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
                                                        <table cellpadding="1" cellspacing="1" border="0" class="text">
                                                            <tr>
                                                                <td style="width: 120px">Tiêu đề:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTitle" runat="server" Width="540px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">Nội dung
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <telerik:RadEditor ID="RadContent" runat="server" Width="670px" Height="400px" Skin="Default"
                                                                        ToolsFile="~/RadControls/Editor/CustomTools.xml">
                                                                        <Tools>
                                                                            <telerik:EditorToolGroup>
                                                                                <telerik:EditorTool Name="CustomImageManager" Text="Nhập ảnh" />
                                                                                <telerik:EditorTool Name="CustomFlashManager" Text="Nhập Flash" />
                                                                                <telerik:EditorTool Name="CustomMediaManager" Text="Nhập Clip" />
                                                                            </telerik:EditorToolGroup>
                                                                        </Tools>
                                                                        <Content>
                                                                        </Content>
                                                                    </telerik:RadEditor>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <fieldset id="fieldsetSelected">
                                                                        <legend class="text"><strong>Khách mời tham gia</strong></legend>
                                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="1" cellpadding="1" class="text">
                                                                                        <tr>
                                                                                            <td>Họ tên:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFullName" runat="server" Width="540px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Giới tính:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlGender">
                                                                                                    <asp:ListItem Selected="true" Value="1">Nam</asp:ListItem>
                                                                                                    <asp:ListItem Value="0">Nữ</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Nghề nghiệp:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtJob" runat="server" Width="540px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2" align="right">
                                                                                                <asp:Button ID="btnGuestAdd" runat="server" OnClick="btnGuestAdd_Click" CssClass="button"
                                                                                                    Text="Thêm" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Repeater ID="rptGuest" runat="server" OnItemDataBound="rptGuest_ItemDataBound">
                                                                                        <HeaderTemplate>
                                                                                            <table width="100%" border="0" cellspacing="1" cellpadding="1" style="background-color: #E8EDF6"
                                                                                                class="text">
                                                                                                <tr class="header">
                                                                                                    <td align="center" style="width: 30px">
                                                                                                        <asp:CheckBox ID="cbxHeaderRemove" runat="server" AutoPostBack="false" />
                                                                                                    </td>
                                                                                                    <td style="width: 40px">
                                                                                                        <asp:Literal ID="ltlHeaderHotOrder" runat="server" Text="STT"></asp:Literal>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Literal ID="ltlHeaderFullName" runat="server" Text="Họ tên"></asp:Literal>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Literal ID="ltlHeaderGender" runat="server" Text="Giới tính"></asp:Literal>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Literal ID="ltlHeaderJob" runat="server" Text="Nghề nghiệp"></asp:Literal>
                                                                                                    </td>
                                                                                                </tr>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <tr class="item" id="trItem" runat="server">
                                                                                                <td align="center" style="width: 30px">
                                                                                                    <asp:CheckBox ID="cbxRemove" runat="server" /><asp:HiddenField ID="hiddenID" runat="server" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Literal ID="ltlOrderNo" runat="server"></asp:Literal>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtFullName" runat="server" Width="200px"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList runat="server" ID="ddlGender">
                                                                                                        <asp:ListItem Selected="true" Value="1">Nam</asp:ListItem>
                                                                                                        <asp:ListItem Value="0">Nữ</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtJob" runat="server" Width="280px"></asp:TextBox>
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
                                                                                    <asp:Button ID="btnRemove" runat="server" CssClass="button" Text="Xóa" OnClick="btnRemove_Click" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
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
                                                                    <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Ghi lại" OnClick="btnAdd_Click" /><asp:HiddenField
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
                                    <td style="background-image: url('<%# UrlRoot %>images/bottom2.gif'); width: 100%"></td>
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
</asp:Content>
