<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" Inherits="CMS.service_discuss_add" CodeBehind="service_discuss_add.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register Src="~/controls/Service_menu.ascx" TagName="service_menu" TagPrefix="uc1" %>--%>
<%@ Register Src="~/controls/discuss_menu.ascx" TagName="discuss_menu" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                                                <td>Thời gian bắt đầu:</td>
                                                                <td>
                                                                    <telerik:RadDateTimePicker ID="startTime" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss" DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                                                    </telerik:RadDateTimePicker>
                                                                    Thời gian kết thúc:
                                                                    <telerik:RadDateTimePicker ID="endTime" runat="server" Skin="Default" DateInput-DateFormat="MM/dd/yyyy HH:mm:ss" DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm:ss">
                                                                    </telerik:RadDateTimePicker>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Ảnh đại diện:</td>
                                                                <td>
                                                                    <asp:TextBox ID="tbxIconPath" Width="80%" runat="server"></asp:TextBox><img id="imgSelectImage" alt="Chọn đường dẫn ảnh" style="cursor: pointer; cursor: pointer; border: 0px"
                                                                        onclick="SelectImage(); return false;" src="<%# UrlRoot %>icons/ImageManager.gif" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Mô tả tóm tắt</td>
                                                                <td>
                                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="tbxSummary" Width="80%" Height="75px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                            <td style="width: 105px">
                                                                                <div id="divImgSummary" runat="server">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>

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
                                                                                <telerik:EditorTool Name="CustomInsertGroupbox" Text="Insert Groupbox" />
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
    <telerik:RadWindowManager ID="Singleton" runat="server">
        <Windows>
            <telerik:RadWindow OnClientShow="OnClientShow" OnClientClose="SelectImageOnClientClose"
                ID="DialogWindow" Behaviors="Close" Skin="Default" Top="22" Modal="true" runat="server"
                Width="680px" Height="550px">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <script type="text/javascript">
        function SelectImage() {
            var sUrl = "<%# UrlRoot %>common/imagemanager.aspx?params=" + Math.random().toString();
            var oWnd = window.radopen(sUrl, "DialogWindow");
            oWnd.Center();
            oWnd.SetUrl(oWnd.GetUrl());
        }
        function SelectImageOnClientClose(radWindow, args) {
            var arg = args.get_argument();
            if (arg) {
                document.getElementById("<%# tbxIconPath.ClientID %>").value = arg.returnValue;
                document.getElementById("<%# divImgSummary.ClientID %>").innerHTML = '<img src="' + arg.returnValue + '" style="border:0px; width:105px; height:80px;" />';
            }
        }
        function OnClientClose(sender, args) {

        };
        function OnClientShow(radWindow) {
        }
        function OnRequestStart(sender, args) {
            if (args.EventTarget.indexOf("btnAdd") >= 0) {
                return true;
            }
        }
        function OnResponseEnd(sender, args) {
            var result = document.getElementById("<%# hfResult.ClientID %>").value;
            if (args.EventTarget.indexOf("btnAdd") >= 0 && result == "1") {
                location.href = '<%# UrlRoot %>service/discuss/index.htm';
            }
        }
        function SelectAll(objClick, objRelated) {
            var obj = document.getElementById(objClick);
            var chk = obj.checked;
            var f = document.getElementById(objRelated);
            var c = f.getElementsByTagName('input');
            var l = c.length;
            for (var i = 0; i < l; i++)
                c[i].checked = chk;
        }
        //
        Telerik.Web.UI.Editor.CommandList["CustomInsertGroupbox"] = function (commandName, editor, args) {
            var myCallbackFunction = function (sender, args) {
                var arrExtension = args.returnExtension.split('|');
                //var sFieldset = '<div style="border:1px solid Black; background-color:' + arrExtension[2] + '">' + args.returnValue + '&nbsp;</div>';
                if (arrExtension[1] == "left") {
                    editor.pasteHtml('<TABLE style="margin-left:7px;border:1px solid Black; background-color:' + arrExtension[2] + '" cellSpacing=3 cellPadding=3 width=' + arrExtension[0] + ' align=' + arrExtension[1] + '><TR><TD>' + args.returnValue + '</TD></TR></TABLE>');
                }
                if (arrExtension[1] == "center") {
                    editor.pasteHtml('<TABLE style="margin-left:7px;border:1px solid Black; background-color:' + arrExtension[2] + '" cellSpacing=3 cellPadding=3 width=' + arrExtension[0] + ' align=' + arrExtension[1] + '><TR><TD>' + args.returnValue + '</TD></TR></TABLE>');
                }
                if (arrExtension[1] == "right") {
                    editor.pasteHtml('<TABLE style="margin-left:7px;border:1px solid Black; background-color:' + arrExtension[2] + '" cellSpacing=3 cellPadding=3 width=' + arrExtension[0] + ' align=' + arrExtension[1] + '><TR><TD>' + args.returnValue + '</TD></TR></TABLE>');
                }
            };
            editor.showDialog("CustomInsertGroupbox", {}, myCallbackFunction);
        };
        Telerik.Web.UI.Editor.CommandList["CustomImageManager"] = function (commandName, editor, args) {
            var myCallbackFunction = function (sender, args) {
                var imgExtension = args.returnExtension.split('|');
                //                //var sImg = '<img src="' + args.returnValue + '" alt="" style="width: ' + imgExtension[0] + 'px; height: ' + imgExtension[1] + 'px;"  hspace="' + imgExtension[2] + '" vspace="' + imgExtension[3] + '" border="' + imgExtension[4] + '" align="' 
                //+ imgExtension[5] + '" />';
                //                //editor.pasteHtml('<div><TABLE cellSpacing=0 cellPadding=3 width=1 border=0 align=' + imgExtension[5] + '><TR><TD>' + sImg + '</TD></TR><TR><TD class=Image align=left>abc</TD></TR></TABLE></div>');
                var sImg = '<img src="' + args.returnValue + '" alt="" WIDTH="' + imgExtension[0] + '" HEIGHT="' + imgExtension[1] + '"  hspace="' + imgExtension[2] + '" vspace="' + imgExtension[3] + '" border="' + imgExtension[4] + '" />';
                editor.pasteHtml('<div style="text-align:' + imgExtension[5] + '">' + sImg + '<br />' + imgExtension[6] + '</div>');
            }
            editor.showDialog("CustomImageManager", {}, myCallbackFunction);
        };

        Telerik.Web.UI.Editor.CommandList["CustomMediaManager"] = function (commandName, editor, args) {
            var myCallbackFunction = function (sender, args) {
                var flvExtension = args.returnExtension.split('|');
                var file = args.returnValue;
                var sEmbed = '';
                if (file.indexOf(".flv") >= 0) {
                    sEmbed = '<object width="512" height="384" classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0"> <param name="flashvars" value="file=' + args.returnValue + '"><param name="movie" value="http://forum.videohelp.com/images/guides/315188/flvplayer.swf"><embed src="http://forum.videohelp.com/images/guides/315188/flvplayer.swf" originalattribute="src" originalpath="http://forum.videohelp.com/images/guides/315188/flvplayer.swf" width="512" height="384" bgcolor="#FFFFFF" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="file=' + args.returnValue + '"></object>';
                }
                else if (file.indexOf(".mp4") >= 0) {
                    sEmbed = ' <video class="video-js vjs-default-skin" controls="" preload="auto" width="' + flvExtension[0] + '" height="264" poster="path-to-poster.png" data-setup="{}">';
                    sEmbed += '<source src="' + args.returnValue + '" type="video/mp4"></video>';
                }
                else if (file.indexOf("youtube.com") >= 0) {
                    sEmbed = '<iframe width="' + flvExtension[0] + '" height="' + flvExtension[1] + '" frameborder="0" allowfullscreen="true" src="' + args.returnValue + '"></iframe>';
                }
                sEmbed += '<div class="urlVideo" style="display:none;">' + args.returnValue + '</div>';
                //var sEmbed = '<embed height="' + flvExtension[1] + '" width="' + flvExtension[0] + '" flashvars="file=' + args.returnValue + '&amp;width=' + flvExtension[0] + '&amp;height=' + flvExtension[1] + '&amp;autostart=false&amp;volume=100&amp;repeat=true&amp;bufferlength=10" allowscriptaccess="always" allowfullscreen="true" wmode="transparent" quality="hight" name="flvplayer" id="flvplayer" src="<%# mediaUrl %>" type="application/x-shockwave-flash"></embed>';
                editor.pasteHtml('<TABLE cellSpacing=0 cellPadding=3 width=1 border=0 align=' + flvExtension[5] + '><TR><TD>' + sEmbed + '</TD></TR><TR><TD align=center  style="font-family:Arial; font-size:10pt;color:#002060;"><i>' + flvExtension[6] + '</i>&nbsp;</TD></TR></TABLE>');
            }
            editor.showDialog("CustomMediaManager", {}, myCallbackFunction);
        };

        Telerik.Web.UI.Editor.CommandList["CustomFlashManager"] = function (commandName, editor, args) {
            var myCallbackFunction = function (sender, args) {
                var flvExtension = args.returnExtension.split('|');
                var sEmbed = '<embed src="' + args.returnValue + '" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="' + flvExtension[0] + '" height="' + flvExtension[1] + '" quality="High" wmode="transparent" />';
                editor.pasteHtml(sEmbed);
            }
            editor.showDialog("CustomFlashManager", {}, myCallbackFunction);
        };
        Telerik.Web.UI.Editor.CommandList["CustomRelatedArticle"] = function (commandName, editor, args) {
            var myCallbackFunction = function (sender, args) {
                editor.pasteHtml(args.returnValue);
            }
            editor.showDialog("CustomRelatedArticle", {}, myCallbackFunction);
        };
    </script>
</asp:Content>


