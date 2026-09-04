<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true"
    Inherits="CMS.system_user" Title="Untitled Page" Codebehind="system_user.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="controls/system_menu.ascx" TagName="system_menu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0"
        class="text" id="tblContent">
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
                                    <td style="width: 10px"><img alt="" style="border: 0px" src="<%# UrlRoot %>images/top1.gif" /></td>
                                    <td class="title" style="background-image: url('<%# UrlRoot %>images/top2.gif');
                                        padding-top: 3px; padding-left: 5px; color: #ffffff" id="tdParentTitle">
                                        <asp:Literal ID="LTL_HEADER" runat="server" Text="Quản lý người dùng"></asp:Literal>
                                    </td>
                                    <td style="width: 10px"><img alt="" style="border: 0px" src="<%# UrlRoot %>images/top3.gif" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top" style="background-color: #354157;">
                        <td style="padding-left: 1px; padding-right: 1px;">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #ffffff">
                                <tr valign="top">
                                    <td style="padding-left: 10px; padding-right: 10px">
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" ClientEvents-OnRequestStart="OnRequestStart"
                                            ClientEvents-OnResponseEnd="OnResponseEnd" LoadingPanelID="AjaxLoadingPanel1">
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr valign="top">
                                                    <td align="center">
                                                        <table cellpadding="1" cellspacing="1" border="0" class="text">
                                                            <tr>
                                                                <td>
                                                                    Đơn vị:
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:DropDownList ID="ddlPart" runat="server"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPart_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Từ khóa
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="tbxKeyword" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Trạng thái:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlStatus" runat="server">
                                                                        <asp:ListItem Text="Tất cả" Value="-1"></asp:ListItem>
                                                                        <asp:ListItem Text="Đã cho phép" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="Chưa cho phép" Value="0"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" CssClass="button" OnClick="btnSearch_Click" />
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
                                                    <td style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr valign="top">
                                                    <td>
                                                        <asp:Repeater ID="rptUsers" runat="server" 
                                                            OnItemDataBound="rptUsers_ItemDataBound" onitemcreated="rptUsers_ItemCreated">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" cellspacing="1" cellpadding="1" style="background-color: #E8EDF6"
                                                                    class="text">
                                                                    <tr class="header">
                                                                        <td align="center" style="width: 30px">
                                                                            <asp:CheckBox ID="cbxHeaderSelect" runat="server" AutoPostBack="false" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="ltlHeaderFullname" runat="server" Text="Họ tên"></asp:Literal>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="ltlHeaderUserId" runat="server" Text="Tên đăng nhập"></asp:Literal>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="ltlHeaderEmail" runat="server" Text="Email"></asp:Literal>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="ltlHeaderTelephone" runat="server" Text="Điện thoại"></asp:Literal>
                                                                        </td>
                                                                        <td align="center" style="width: 60px">
                                                                            <asp:Literal ID="ltlHeaderStatus" runat="server" Text="Trạng thái"></asp:Literal>
                                                                        </td>
                                                                        <td align="center" style="width: 45px">
                                                                            <asp:Literal ID="ltlEdit" runat="server" Text="Sửa"></asp:Literal>
                                                                        </td>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="item" id="trItem" runat="server">
                                                                    <td align="center" style="width: 30px">
                                                                        <asp:CheckBox ID="cbxSelect" runat="server" /><asp:HiddenField ID="hiddenID" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltlFullname" runat="server" Text="Họ tên"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltlUserId" runat="server" Text="Tên đăng nhập"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltlEmail" runat="server" Text="Email"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltlTelephone" runat="server" Text="Điện thoại"></asp:Literal>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="iBtnStatus" runat="server" BorderWidth="0" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:HyperLink ID="hlEdit" runat="server"><img src="<%# UrlRoot %>icons/edit.gif" style="border:0px" alt="Sửa" /></asp:HyperLink>
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
                                                    <td style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
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
                                                                <td align="right">
                                                                    <table cellpadding="0" cellspacing="0" class="text">
                                                                        <tr>
                                                                            <td style="padding-right: 10px">
                                                                                <strong>
                                                                                    <asp:Literal ID="ltlTotal" runat="server"></asp:Literal></strong>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <asp:DataList ID="dlPaper" runat="server" RepeatColumns="9" OnItemCreated="dlPaper_ItemCreated"
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
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadAjaxPanel>
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
                                    <td style="width: 1px"><img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom1.gif" /></td>
                                    <td style="background-image: url('<%# UrlRoot %>images/bottom2.gif'); width: 100%"></td>
                                    <td style="width: 1px"><img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom3.gif" /></td>
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
    <telerik:RadWindowManager ID="Singleton" runat="server">
        <Windows>
            <telerik:RadWindow ID="DialogWindow" Behaviors="Close" Skin="Vista" Top="22" Modal="true"
                runat="server" Title="Đổi mật khẩu">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <script type="text/javascript">
var theForm = document.forms[0];
function SelectAll(objClick, objRelated)
{
    var obj = document.getElementById(objClick);
    var chk = obj.checked;
    var len = theForm.elements.length;
	for (var j = 0; j < len; j++) {	
		var e = theForm.elements[j];
		if (e.name.indexOf(objRelated)>=0) {
			e.checked = chk; 
		}
	}
}
function CheckSelected(objRelated)
{
    var bRet = false;
    var len = theForm.elements.length;
	for (var j = 0; j < len; j++) {	
		var e = theForm.elements[j];
		if (e.name.indexOf(objRelated)>=0 && e.checked) {
			bRet = true;
			break;
		}
	}
	if(!bRet)
	{
	    alert("Phải chọn ít nhất 1 bản ghi!");
	}
	return bRet;
}
function ConfirmDelete(objRelated)
{
    if(CheckSelected(objRelated))
    {
        return confirm("Bạn có thực sự muốn xóa không?");
    }
    return false;
}
//
function ChangePwd(m_UserId)
{
    var sUrl = "<%# UrlRoot %>system_changepwd.aspx?UserId="+m_UserId;
    var oWnd = window.radopen(sUrl, "DialogWindow");
    oWnd.SetSize(400,250);
    oWnd.Center();

   return false;
}

function OnRequestStart(sender, args)
{
    if(args.EventTarget.indexOf("btnDelete") >= 0)
    {
        return ConfirmDelete('cbxSelect');
    }
};

function OnResponseEnd(sender, args)
{
  ActualResize('tblContent');
};
    </script>

</asp:Content>
