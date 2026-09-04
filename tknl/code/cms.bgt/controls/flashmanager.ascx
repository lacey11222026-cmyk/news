<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMS.controls_flashmanager" Codebehind="flashmanager.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="<%# UrlRoot%>css/css.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/backend.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/style_repeater.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/paper.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
function GetRadWindow()
{
	var oWindow = null;
	if (window.radWindow) oWindow = window.radWindow;
	else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
	return oWindow;
}	
function ShowInfo(FlvPath, FlvWidth, FlvHeight)
{
    document.getElementById("FlvPath").value = FlvPath;
    document.getElementById("FlvWidth").value = '100%';
    document.getElementById("FlvHeight").value = '100%';
    
}
function OK_Clicked() {
    var FlvPath = document.getElementById("FlvPath").value;
    if (FlvPath == '') {
        alert("Phải chọn file!"); return;
    }
    var FlvWidth = document.getElementById("FlvWidth").value;
    var FlvHeight = document.getElementById("FlvHeight").value;
    var oWindow = GetRadWindow();
    var oSendArg = oWindow.Argument;
    var arg = new Object();
    if (oSendArg.InstanceId) {
        arg.InstanceId = oSendArg.InstanceId;
    }
    arg.returnValue = FlvPath;
    arg.returnExtension = FlvWidth + '|' + FlvHeight;
    oWindow.Close(arg);
	
}
function Cancel_Clicked()
{
	var oWindow = GetRadWindow();			
	oWindow.Close();
}	
</script>

<telerik:RadTabStrip Style="position: absolute; top: 10px;" ID="RadTabStrip1" SelectedIndex="0"
    runat="server" MultiPageID="RadMultiPage1" Skin="Outlook">
    <Tabs>
        <telerik:RadTab Text="Chọn File" Value="file_manager">
        </telerik:RadTab>
        <telerik:RadTab Text="Upload File" Value="file_upload">
        </telerik:RadTab>
    </Tabs>
</telerik:RadTabStrip>
<telerik:RadMultiPage ID="RadMultiPage1" Width="98%" Style="position: absolute; top: 35px;"
    runat="server" SelectedIndex="0">
    <telerik:RadPageView ID="RadPageView1" runat="server">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" LoadingPanelID="AjaxLoadingPanel1" runat="server"
            Width="100%" Height="100%">
            <hr />
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr valign="top">
                    <td>
                        <fieldset>
                            <legend class="text"><strong>Thông tin tìm kiếm</strong></legend>
                            <table cellpadding="1" cellspacing="1" border="0" class="text">
                                <tr>
                                    <td>
                                        Từ ngày:
                                    </td>
                                    <td align="left" style="width: 150px">
                                        <telerik:RadDateTimePicker ID="txtDateFrom" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss" DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                        </telerik:RadDateTimePicker>
                                    </td>
                                    <td>
                                        Đến ngày:
                                    </td>
                                    <td align="left" style="width: 150px">
                                        <telerik:RadDateTimePicker ID="txtDateTo" runat="server" Skin="Default"  DateInput-DateFormat="MM/dd/yyyy HH:mm:ss" DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss"></telerik:RadDateTimePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        Từ khóa:
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:TextBox ID="tbxKeyword" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px">
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr valign="top">
                                <td style="width: 49%">
                                    <fieldset>
                                        <legend class="text"><strong>Kết quả tìm kiếm</strong></legend>
                                        <div style="width: 100%; height: 265px; overflow: auto">
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
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
                                                        <asp:Repeater ID="rptData" runat="server" OnItemDataBound="rptData_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" cellspacing="1" cellpadding="1" style="background-color: #E8EDF6"
                                                                    class="text">
                                                                    <tr class="header">
                                                                        <td>
                                                                            <asp:Literal ID="ltlHeaderName" runat="server" Text="Name"></asp:Literal>
                                                                        </td>
                                                                        <td style="width: 60px">
                                                                            <asp:Literal ID="ltlHeaderSize" runat="server" Text="Size"></asp:Literal>
                                                                        </td>
                                                                        <td style="width: 120px">
                                                                            <asp:Literal ID="ltlHeaderCrTime" runat="server" Text="Upload Time"></asp:Literal>
                                                                        </td>
                                                                        <td align="center" style="width: 45px">
                                                                            <asp:Literal ID="ltlHeaderDelete" runat="server" Text="Xóa"></asp:Literal>
                                                                        </td>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="item" id="trItem" runat="server">
                                                                    <td>
                                                                        <asp:HyperLink ID="hlName" runat="server" CssClass="treebook"></asp:HyperLink>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltlSize" runat="server" Text="Size"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltlCrTime" runat="server" Text="Upload Time"></asp:Literal>
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
                                                    <td style="height: 10px">
                                                    </td>
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
                                                                    <asp:HiddenField ID="hfCurrPage" runat="server" Value="1" />
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
                                        </div>
                                    </fieldset>
                                </td>
                                <td style="width: 2%">
                                </td>
                                <td style="width: 49%">
                                    <fieldset>
                                        <legend class="text"><strong>Thuộc tính Flash</strong></legend>
                                        <table style="width: 100%" cellpadding="1" cellspacing="1" class="text">
                                            <tr>
                                                <td>
                                                    Đường dẫn:
                                                </td>
                                                <td>
                                                    <input type="text" id="FlvPath" style="width: 200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    Width:
                                                </td>
                                                <td>
                                                    <input type="text" id="FlvWidth" style="width: 200px" value="400" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Height:
                                                </td>
                                                <td>
                                                    <input type="text" id="FlvHeight" style="width: 200px" value="300" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="2">
                                                    <input type="button" value="Ghi lại" class="button" onclick="OK_Clicked()" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </telerik:RadPageView>
    <telerik:RadPageView ID="RadPageView2" runat="server">
        <telerik:RadProgressManager ID="RadProgressManager1" runat="server" Skin="Default" />
        <hr />
        <fieldset>
            <legend class="text"><strong>Upload file</strong></legend>
            <table width="98%" align="center" cellpadding="0" cellspacing="0" border="0" class="text">
                <tr>
                    <td>
                        File 1:
                    </td>
                    <td>
                        <input type="file" id="file1" runat="server" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 2:
                    </td>
                    <td>
                        <input type="file" id="file2" runat="server" name="file2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 3:
                    </td>
                    <td>
                        <input type="file" id="file3" runat="server" name="file3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 4:
                    </td>
                    <td>
                        <input type="file" id="file4" runat="server" name="file4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 5:
                    </td>
                    <td>
                        <input type="file" id="file5" runat="server" name="file5" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="left">
                        <asp:Button ID="btPost" runat="server" CssClass="button" Text="Upload" OnClick="btPost_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadProgressArea ID="RadProgressArea" runat="server" Skin="Default">
                        </telerik:RadProgressArea>
                    </td>
                </tr>
            </table>
        </fieldset>
    </telerik:RadPageView>
</telerik:RadMultiPage>
<telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server" Transparency="10"
    MinDisplayTime="300">
    <img src="<%# UrlRoot %>images/loading.gif" alt="Loading" style="border: 0px; vertical-align: middle;" />
</telerik:RadAjaxLoadingPanel>