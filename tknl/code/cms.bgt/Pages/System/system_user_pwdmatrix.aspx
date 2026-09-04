<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true"
    CodeBehind="system_user_pwdmatrix.aspx.cs" Inherits="CMS2012.system_user_pwdmatrix" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellspacing="0" cellpadding="0" border="0">
        <tr style="height: 23px">
            <td style="width: 9px">
                <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top_left.gif" />
            </td>
            <td style="background-image: url('<%# UrlRoot %>images/bg_top.gif'); padding-left: 5px;
                color: #ffffff">
                <strong>
                    <asp:Literal ID="ltlHeader" runat="server" Text="Tạo mật khẩu ma trận"></asp:Literal></strong>
            </td>
            <td style="width: 9px">
                <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top_right.gif" />
            </td>
        </tr>
        <tr>
            <td style="background-image: url('<%# UrlRoot %>images/bg_left.gif')">
            </td>
            <td>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #ffffff">
                    <tr valign="top">
                        <td style="padding-left: 10px; padding-right: 10px">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td style="height: 40px">
                                        Đơn vị:
                                    </td>
                                    <td style="height: 40px">
                                        <asp:DropDownList ID="ddlPart" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 40px">
                                        Họ và tên:
                                    </td>
                                    <td style="height: 40px">
                                        <asp:TextBox ID="txtFullName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 40px">
                                        Username:
                                    </td>
                                    <td style="height: 40px">
                                        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 40px">
                                        &nbsp;
                                    </td>
                                    <td style="height: 40px">
                                        <asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 40px">
                                        &nbsp;
                                    </td>
                                    <td style="height: 40px">
                                        <asp:Button ID="btnCreate" runat="server" Text="Tạo ma trận" OnClick="btnCreate_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="background-image: url('<%# UrlRoot %>images/bg_right.gif')">
            </td>
        </tr>
        <tr style="height: 13px">
            <td style="width: 9px">
                <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom_left.gif" />
            </td>
            <td style="background-image: url('<%# UrlRoot %>images/bg_bottom.gif'); padding-top: 3px;
                padding-left: 5px; color: #ffffff">
            </td>
            <td style="width: 9px">
                <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom_right.gif" />
            </td>
        </tr>
    </table>
</asp:Content>