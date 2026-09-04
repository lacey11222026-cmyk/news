<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMS.login" CodeBehind="login.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>..: Online News Management System :..</title>
    <link href="<%# UrlRoot %>css/login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%# UrlRoot %>js/md5.js"></script>
</head>
<body style="margin-bottom: 0px; margin-left: 0px; margin-right: 0px; margin-top: 0px;
    margin: 0px; bottom: 0px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <table border="0" cellpadding="0" cellspacing="0" style="height: 100%; width: 100%">
        <tr>
            <td align="center" valign="middle">
                <table border="0" cellpadding="0" cellspacing="0" width="547">
                    <tr>
                        <td style="padding-right: 50px; padding-left: 0px; padding-bottom: 0px; padding-top: 40px">
                            <table border="0" cellpadding="0" cellspacing="0" width="508">
                                <tr>
                                    <td colspan="3">
                                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bn1.gif" width="508" height="54" /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url('<%# UrlRoot %>images/login_07.gif'); width: 25px">
                                    </td>
                                    <td style="background-color: #ffffff; width: 455px;">
                                        <table style="width: 433px" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top" style="width: 81px;">
                                                    &nbsp;
                                                </td>
                                                <td align="right" style="width: 352px;">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top">
                                                    <img alt="" style="border: 0px;" src="<%#  UrlRoot %>images/khoa.gif" width="68"
                                                        height="156" />
                                                </td>
                                                <td align="right" style="background-color: #6390b9; width: 352px;" id="login_form_td">
                                                    <div style="color: #003399">
                                                        <br />
                                                        <center>
                                                            <span style="font-size: 9pt"><b>ĐĂNG NHẬP HỆ THỐNG</b></span></center>
                                                        <table style="margin: 18px" border="0" cellpadding="0" cellspacing="0" width="313"
                                                            id="table2">
                                                            <tr>
                                                                <td class="login_form_txt" style="width: 140px; color: #003399">
                                                                    Tên truy cập:
                                                                </td>
                                                                <td class="login_form_td" style="width: 158px">
                                                                    <input type="text" id="txtUser" runat="server" onkeydown="OnKeydown(event)" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="login_form_txt" style="color: #003399">
                                                                    Mật khẩu:
                                                                </td>
                                                                <td class="login_form_td">
                                                                    <input type="password" id="txtPass" runat="server" onkeydown="OnKeydown(event)" />
                                                                </td>
                                                            </tr>                                                        
                                                           
                                                            <tr>
                                                                <td class="login_form_td" colspan="2">
                                                                    <asp:Button ID="btnLogin" runat="server" CssClass="login" Text="Đăng nhập" OnClick="btnLogin_Click" /><asp:HiddenField
                                                                        ID="hfResult" runat="server" Value="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" class="login_error">
                                                                    <asp:Literal ID="ltlError" runat="server"></asp:Literal>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="background-image: url('<%# UrlRoot %>images/login_09.gif'); width: 28px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25px">
                                        <img src="<%# UrlRoot %>images/login_10.gif" alt="" style="border: 0px" height="24"
                                            width="25" />
                                    </td>
                                    <td style="background-image: url('<%# UrlRoot %>images/login_11.gif'); width: 455px;">
                                    </td>
                                    <td style="width: 28px">
                                        <img src="<%# UrlRoot %>images/login_12.gif" alt="" style="border: 0px" height="24"
                                            width="28" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="OnRequestStart"
        ClientEvents-OnResponseEnd="OnResponseEnd">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnLogin">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ltlError" LoadingPanelID="AjaxLoadingPanel1">
                    </telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hfResult"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Transparency="10"
        MinDisplayTime="300">
        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/loading.gif" BorderWidth="0px"
            AlternateText="Loading"></asp:Image>
    </telerik:RadAjaxLoadingPanel>
    </form>
</body>
</html>
<script type="text/javascript">

    function OnRequestStart(sender, args) {
        var objUserName = document.getElementById("<%# txtUser.ClientID %>");
        if (objUserName.value == "") {
            alert("Phải nhập tên truy cập!");
            objUserName.focus();
            return false;
        }
        var objPassword = document.getElementById("<%# txtPass.ClientID %>");
        if (objPassword.value == "") {
            alert("Phải nhập mật khẩu!");
            objPassword.focus();
            return false;
        }
    }

    function OnResponseEnd(sender, args) {
        var result = document.getElementById("<%# hfResult.ClientID %>").value;
        if (result == "1")
            location.href = '<%# Url %>';
    }
    function OnKeydown(evt) {
        a = evt.keyCode;
        if (a == 13) {
            document.getElementById("<%# btnLogin.ClientID %>").click();
        }
    }
</script>