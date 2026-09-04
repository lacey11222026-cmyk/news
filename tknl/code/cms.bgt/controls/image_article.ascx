<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMS.controls_image_article"
    CodeBehind="image_article.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="<%# UrlRoot%>css/css.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/backend.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/style_repeater.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/paper.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="<%# UrlRoot %>js/tooltip.js"></script>
<script type="text/javascript">
    function SelectAll(objClick, objRelated) {
        var obj = document.getElementById(objClick);
        var chk = obj.checked;
        var f = document.getElementById(objRelated);
        var c = f.getElementsByTagName('input');
        var l = c.length;
        for (var i = 0; i < l; i++)
            c[i].checked = chk;
    }
    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

    function Cancel_Clicked() {
        var oWindow = GetRadWindow();
        oWindow.Close();
    }
    function ShowInfo(ImgPath, ImgDescription) {
        document.getElementById("ImgPath").value = ImgPath;
        document.getElementById("txtDescription").value = ImgDescription;

    }
    function OK_Clicked(returnValue) {
        var oWindow = GetRadWindow();
        var arg = new Object();
        arg.returnValue = returnValue;
        oWindow.Close(arg);

    }
    function OnRequestStart(sender, args) {

    }
    function OnResponseEnd(sender, args) {

        if (args.EventTarget.indexOf("btnFinish") >= 0) {
            var result = document.getElementById("<%# hfReturn.ClientID %>").value;
            OK_Clicked(result);
        }
    }
</script>
<telerik:RadTabStrip Style="position: absolute; top: 10px;" ID="RadTabStrip1" SelectedIndex="0"
    runat="server" MultiPageID="RadMultiPage1" Skin="Outlook">
    <Tabs>
        <telerik:RadTab Text="Chọn File" Value="file_manager">
        </telerik:RadTab>
        <telerik:RadTab Text="Upload nhiều file" Value="file_multi_upload">
        </telerik:RadTab>
    </Tabs>
</telerik:RadTabStrip>
<telerik:RadMultiPage ID="RadMultiPage1" Width="98%" Style="position: absolute; top: 35px;"
    runat="server" SelectedIndex="0">
    <telerik:RadPageView ID="RadPageView1" runat="server">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" LoadingPanelID="AjaxLoadingPanel1" runat="server"
            Width="100%" Height="100%" ClientEvents-OnRequestStart="OnRequestStart" ClientEvents-OnResponseEnd="OnResponseEnd">
            <hr />
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr valign="top">
                    <td>
                        <fieldset>
                            <legend class="text"><strong>Thông tin tìm kiếm</strong></legend>
                            <table cellpadding="1" cellspacing="1" border="0" class="text">
                                <tr>
                                    <td>
                                        Từ ngày (dd/MM/yyyy):
                                    </td>
                                    <td align="left" style="width: 150px">
                                        <telerik:RadDateTimePicker ID="txtDateFrom" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss"
                                            DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                        </telerik:RadDateTimePicker>
                                    </td>
                                    <td>
                                        Đến ngày (dd/MM/yyyy):
                                    </td>
                                    <td align="left" style="width: 150px">
                                        <telerik:RadDateTimePicker ID="txtDateTo" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss"
                                            DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                        </telerik:RadDateTimePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        Từ khóa:
                                    </td>
                                    <td colspan="2" align="right">
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
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr valign="top">
                                <td style="width: 49%">
                                    <fieldset id="fieldsetResult">
                                        <legend class="text"><strong>Kết quả tìm kiếm</strong></legend>
                                        <div style="width: 100%; height: 280px; overflow: auto">
                                            <table cellpadding="0" cellspacing="0" class="text" style="width: 100%">
                                                <tr>
                                                    <td align="right">
                                                        <table cellpadding="0" cellspacing="0" class="text">
                                                            <tr>
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
                                                                            <asp:Literal ID="ltlHeaderName" runat="server" Text="Name"></asp:Literal>
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
                                                                        <asp:HiddenField ID="HiddenFilePath" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:HyperLink ID="hlName" runat="server" CssClass="treebook"></asp:HyperLink>
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
                                <td style="width: 8%; vertical-align: middle" align="center">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnAdd" CssClass="button" Text=">>" runat="server" Width="30px" OnClick="btnAdd_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnRemove" CssClass="button" Text="<<" runat="server" Width="30px"
                                                    OnClick="btnRemove_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 46%">
                                    <fieldset id="fieldsetSelected">
                                        <legend class="text"><strong>Files được chọn</strong></legend>
                                        <div style="width: 100%; height: 280px; overflow: auto">
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Repeater ID="rptSelected" runat="server" OnItemDataBound="rptSelected_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" cellspacing="1" cellpadding="1" style="background-color: #E8EDF6"
                                                                    class="text">
                                                                    <tr class="header">
                                                                        <td align="center" style="width: 30px">
                                                                            <asp:CheckBox ID="cbxHeaderRemove" runat="server" AutoPostBack="false" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="ltlHeaderTitle" runat="server" Text="Tên File"></asp:Literal>
                                                                        </td>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="item" id="trItem" runat="server">
                                                                    <td align="center" style="width: 30px">
                                                                        <asp:CheckBox ID="cbxRemove" runat="server" /><asp:HiddenField ID="hiddenID" runat="server" />
                                                                        <asp:HiddenField ID="HiddenFilePath" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:HyperLink ID="hlTitle" runat="server" CssClass="treebook" Text="Tiêu đề bài viết"></asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3" style="padding-right: 3px; padding-top: 3px">
                                    <asp:HiddenField ID="hfReturn" runat="server" Value="0" />
                                    <asp:Button ID="btnFinish" runat="server" Text="Chọn xong" OnClick="btnFinish_Click"
                                        CssClass="button" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </telerik:RadPageView>
    <telerik:RadPageView ID="RadPageView3" runat="server">
        <telerik:RadProgressManager ID="RadProgressManager2" runat="server" Skin="Default" />
        <hr />
        <fieldset>
            <legend class="text"><strong>Upload nhiều file</strong></legend>
            <table width="98%" align="center" cellpadding="0" cellspacing="0" border="0" class="text">
                <tr>
                    <td colspan="2">
                        <asp:Literal ID="ltlMultiUploadimageFilters" runat="server" Text="Hệ thống chỉ cập nhật các file có định dạng sau: .gif .jpg .jpeg .bmp .psd .tiff .tif .png"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="left">
                        <asp:Button ID="Button1" runat="server" CssClass="button" Text="Upload" OnClick="btnUploadMulti_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 1:
                    </td>
                    <td>
                        <input type="file" id="file2" runat="server" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 2:
                    </td>
                    <td>
                        <input type="file" id="file3" runat="server" name="file2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 3:
                    </td>
                    <td>
                        <input type="file" id="file4" runat="server" name="file3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 4:
                    </td>
                    <td>
                        <input type="file" id="file5" runat="server" name="file4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 5:
                    </td>
                    <td>
                        <input type="file" id="file6" runat="server" name="file5" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 6:
                    </td>
                    <td>
                        <input type="file" id="file7" runat="server" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 7:
                    </td>
                    <td>
                        <input type="file" id="file8" runat="server" name="file2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 8:
                    </td>
                    <td>
                        <input type="file" id="file9" runat="server" name="file3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 9:
                    </td>
                    <td>
                        <input type="file" id="file10" runat="server" name="file4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 10:
                    </td>
                    <td>
                        <input type="file" id="file11" runat="server" name="file5" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 11:
                    </td>
                    <td>
                        <input type="file" id="file12" runat="server" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 12:
                    </td>
                    <td>
                        <input type="file" id="file13" runat="server" name="file2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 13:
                    </td>
                    <td>
                        <input type="file" id="file14" runat="server" name="file3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 14:
                    </td>
                    <td>
                        <input type="file" id="file15" runat="server" name="file4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 15:
                    </td>
                    <td>
                        <input type="file" id="file16" runat="server" name="file5" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 16:
                    </td>
                    <td>
                        <input type="file" id="file17" runat="server" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 17:
                    </td>
                    <td>
                        <input type="file" id="file18" runat="server" name="file2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 18:
                    </td>
                    <td>
                        <input type="file" id="file19" runat="server" name="file3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 19:
                    </td>
                    <td>
                        <input type="file" id="file20" runat="server" name="file4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        File 20:
                    </td>
                    <td>
                        <input type="file" id="file21" runat="server" name="file5" />
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
                        <asp:Button ID="btnUploadMulti" runat="server" CssClass="button" Text="Upload" OnClick="btnUploadMulti_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadProgressArea ID="RadProgressArea2" runat="server" Skin="Default">
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