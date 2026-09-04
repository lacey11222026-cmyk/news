<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" Inherits="CMS.system_user_changepwd"
    Title="Untitled Page" CodeBehind="system_user_changepwd.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                            <asp:Literal ID="LTL_HEADER" runat="server" Text="Đổi mật khẩu người dùng"></asp:Literal>
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
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr valign="top">
                                    <td>
                                        <table cellpadding="1" cellspacing="1" border="0" class="text">
                                            <tr>
                                                <td style="width: 130px">
                                                    Tên truy cập:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserName" runat="server" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mật khẩu cũ:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="password"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mật khẩu mới:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="password"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNewPassword"
                                                        ValidationExpression="(?=.{8,})[a-zA-Z]+[^a-zA-Z]+|[^a-zA-Z]+[a-zA-Z]+" Display="Dynamic"
                                                        ErrorMessage="Mật khẩu gồm 8 ký tự bao gồm chữ hoa, thường và số." />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Nhập lại mật khẩu mới:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="password"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtConfirmNewPassword"
                                                        ValidationExpression="(?=.{8,})[a-zA-Z]+[^a-zA-Z]+|[^a-zA-Z]+[a-zA-Z]+" Display="Dynamic"
                                                        ErrorMessage="Mật khẩu gồm 8 ký tự bao gồm chữ hoa, thường và số." />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px">
                                    </td>
                                </tr>
                                <tr>
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
                                <tr>
                                    <td style="height: 10px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
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
    <script type="text/javascript">
        var theForm = document.forms[0];

        function ConfirmRequired() {
            var objOldPass = document.getElementById("<%# txtOldPassword.ClientID %>");
            if (objOldPass.value == "") {
                alert("Phải nhập mật khẩu cũ!");
                objOldPass.focus();
                return false;
            }
            var objNewPass = document.getElementById("<%# txtNewPassword.ClientID %>");
            if (objNewPass.value == "") {
                alert("Phải nhập mật khẩu mới!");
                objNewPass.focus();
                return false;
            }
            if (objNewPass.length < 6) {
                alert("Mật khẩu ít nhất phải 6 kí tự!");
                objNewPass.focus();
                return false;
            }
            var objConfirmPass = document.getElementById("<%# txtConfirmNewPassword.ClientID %>");
            if (objConfirmPass.value == "") {
                alert("Phải nhập lại mật khẩu mới!");
                objConfirmPass.focus();
                return false;
            }
            if (objNewPass.value != objConfirmPass.value) {
                alert("Hai mật khẩu phải giống nhau");
                objConfirmPass.focus();
                return false;
            }
            return true;
        }

    </script>
</asp:Content>