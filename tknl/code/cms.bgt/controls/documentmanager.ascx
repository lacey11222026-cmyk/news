<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMS.controls_documentmanager" Codebehind="documentmanager.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="<%# UrlRoot%>css/css.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/backend.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/style_repeater.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/paper.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }
    function OK_Clicked(DocPath, DocExtension) {
        var oWindow = GetRadWindow();
        var oSendArg = oWindow.Argument;
        var arg = new Object();
        if (oSendArg.InstanceId) {
            arg.InstanceId = oSendArg.InstanceId;
        }
        arg.returnValue = DocPath;
        arg.returnExtension = DocExtension;
        oWindow.Close(arg);
    }
    function Cancel_Clicked() {
        var oWindow = GetRadWindow();
        oWindow.Close();
    }	
</script>

<telerik:RadTabStrip Style="position: absolute; top: 10px;" ID="RadTabStrip1" SelectedIndex="1"
    runat="server" MultiPageID="RadMultiPage1" Skin="Outlook">
    <Tabs>
        <telerik:RadTab Text="Chọn File" Value="file_manager">
        </telerik:RadTab>
        <telerik:RadTab Text="Upload File" Value="file_upload">
        </telerik:RadTab>
        <telerik:RadTab Text="Upload nhiều file" Value="file_multi_upload"></telerik:RadTab>
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
                                    <td align="left" style="width: 105px">
                                        <telerik:RadDateTimePicker ID="txtDateFrom" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss" DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                          </telerik:RadDateTimePicker>
                                    </td>
                                    <td>
                                        Đến ngày:
                                    </td>
                                    <td align="left" style="width: 105px">
                                        <telerik:RadDateTimePicker ID="txtDateTo" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss" DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                          </telerik:RadDateTimePicker>
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
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </telerik:RadPageView>
    <telerik:RadPageView ID="RadPageView2" runat="server" Selected="true">
        <telerik:RadProgressManager ID="RadProgressManager1" runat="server" Skin="Default" />
        <hr />
        <fieldset>
            <legend class="text"><strong>Upload file</strong></legend>
            <table width="98%" align="center" cellpadding="0" cellspacing="0" border="0" class="text">
                <tr>
                    <td colspan="2">
                    <asp:Literal ID="ltldocumentsFilters" runat="server" Text="Hệ thống chỉ cập nhật các file có định dạng sau: .doc .docx .xls .pdf .rar"></asp:Literal>                        
                    </td>
                </tr>
                <tr>
                    <td>
                        Chọn file:
                    </td>
                    <td>
                        <input type="file" id="file1" runat="server" name="file1" style="width:220px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Mô tả:
                    </td>
                    <td>
                        <asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine" Width="220px" Height="40px"></asp:TextBox>
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
                        <asp:Button ID="btPost" runat="server" CssClass="button" Text="Upload file" OnClick="btPost_Click" />
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
    <telerik:RadPageView ID="RadPageView3" runat="server">
        <telerik:RadProgressManager ID="RadProgressManager2" runat="server" Skin="Default" />
        <hr />
        <fieldset>
            <legend class="text"><strong>Upload nhiều file</strong></legend>
            <table width="98%" align="center" cellpadding="0" cellspacing="0" border="0" class="text">
                <tr>
                    <td colspan="2">
                        <asp:Literal ID="ltlMultiUploaddocumentsFilters" runat="server" Text="Hệ thống chỉ cập nhật các file có định dạng sau: .doc .docx .xls .pdf .rar"></asp:Literal>
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
