<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMS.controls_workflow_menu" Codebehind="workflow_menu.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top" style="height: 20px">
        <td>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr valign="top">
                    <td style="width: 10px">
                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top1.gif" /></td>
                    <td class="title" style="background-image: url('<%# UrlRoot %>images/top2.gif'); padding-top: 3px;
                        padding-left: 5px; color: #ffffff">
                        <asp:Literal ID="LTL_HEADER" runat="server" Text="Biên tập nội dung"></asp:Literal></td>
                    <td align="right" style="background-image: url('<%# UrlRoot %>images/top2.gif'); padding-top: 2px">
                        <img alt="Thu nhỏ" id="imgDecrease" style="cursor: hand; cursor: pointer" onclick="Decrease()"
                            src="<%# UrlRoot %>images/minus.gif" />&nbsp;&nbsp;<img alt="Phóng to" id="imgEnLarge"
                                style="cursor: hand; cursor: pointer" onclick="EnLarge()" src="<%# UrlRoot %>images/plus.gif" /></td>
                    <td style="width: 10px">
                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/top3.gif" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr valign="top" style="background-color: #354157;">
        <td style="padding-left: 1px; padding-right: 1px; padding-top: 0px; padding-bottom: 0px;
            height: 100%">
            <table width="100%" style="background-color: #ffffff; height: 100%" cellpadding="0"
                cellspacing="0" border="0">
                <tr>
                    <td valign="top" style="padding-left: 10px; padding-right: 5px">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlPart" runat="server" DataTextField="Name" DataValueField="ID"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPart_SelectedIndexChanged">
                                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr><td style="height:5px"></td></tr>
                            <tr>
                                <td>
                                    <telerik:RadPanelBar ID="PanelMenu" runat="server" ExpandMode="SingleExpandedItem"
                                        Skin="Vista" Width="100%">
                                    </telerik:RadPanelBar>
                                </td>
                            </tr>
                        </table>
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
                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom1.gif" /></td>
                    <td style="background-image: url('<%# UrlRoot %>images/bottom2.gif'); width: 100%">
                    </td>
                    <td style="width: 1px">
                        <img alt="" style="border: 0px" src="<%# UrlRoot %>images/bottom3.gif" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="OnRequestStart1"
    ClientEvents-OnResponseEnd="OnResponseEnd1">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="ddlPart">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="ddlPart" LoadingPanelID="AjaxLoadingPanel1">
                </telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server" Transparency="10"
    MinDisplayTime="300">
    <img src="<%# UrlRoot %>images/loading.gif" alt="Loading" style="border: 0px; vertical-align: middle;" />
</telerik:RadAjaxLoadingPanel>
<script type="text/javascript">
function OnRequestStart1(sender, args)
{
}
function OnResponseEnd1(sender, args)
{
   location.href = '<%# UrlRoot %>workflow.aspx';
}
</script>