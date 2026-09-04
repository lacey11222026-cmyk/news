<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMS.system_changepwd" Codebehind="system_changepwd.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Đổi mật khẩu người dùng</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="<%= UrlRoot%>css/css.css" rel="stylesheet" type="text/css" />
    <link href="<%= UrlRoot%>css/backend.css" rel="stylesheet" type="text/css" />
    <link href="<%= UrlRoot%>css/style_repeater.css" rel="stylesheet" type="text/css" />
    <link href="<%= UrlRoot%>css/paper.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%= UrlRoot%>js/md5.js"></script>
</head>
<body>
    <form id="form1" runat="server" style="margin-top:10px; margin-left:10px; margin-right:10px">
    <table cellpadding="1" cellspacing="1" border="0" class="text">
        <tr>
            <td style="width: 120px">
                Tên truy cập:
            </td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Mật khẩu mới:
            </td>
            <td>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Nhập lại mật khẩu:
            </td>
            <td>
                <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 10px" colspan="2">
            </td>
        </tr>
        <tr>
        <td></td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-right: 5px;">
                            <asp:Button ID="btnUpdate" runat="server" CssClass="button" Text="Ghi lại" OnClick="btnUpdate_Click"
                                OnClientClick="return ConfirmRequired();" />
                        </td>
                        <td>
                            <strong>
                                <asp:Literal ID="ltlError" runat="server"></asp:Literal></strong>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
<script type="text/javascript">
function ConfirmRequired()
{
   
    var objNewPass = document.getElementById("<%=txtNewPassword.ClientID %>");
    if(objNewPass.value == "")
    {
        alert("Phải nhập mật khẩu mới!"); 
        objNewPass.focus();
        return false;
    }
    if(objNewPass.length < 6)
    {
        alert("Mật khẩu ít nhất phải 6 kí tự!"); 
        objNewPass.focus();
        return false;
    }
    var objConfirmPass = document.getElementById("<%=txtConfirmNewPassword.ClientID %>");
     if(objConfirmPass.value == "")
    {
        alert("Phải nhập lại mật khẩu mới!"); 
        objConfirmPass.focus();
        return false;
    }
    if(objNewPass.value != objConfirmPass.value)
    {
        alert("Hai mật khẩu phải giống nhau");
        objConfirmPass.focus();
        return false;
    }
    return true;
}

    </script>
