<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" Inherits="CMS.discuss_add" CodeBehind="discuss_add.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
                                        <asp:Literal ID="LTL_HEADER" runat="server" Text="Cập nhật câu hỏi"></asp:Literal>
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="AjaxLoadingPanel1">
                                                        <table cellpadding="1" cellspacing="1" border="0" class="text">
                                                            <tr>
                                                                <td style="width: 120px">Cuộc giao lưu:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlDiscuss" runat="server" DataTextField="Title" DataValueField="DiscussId"
                                                                        AutoPostBack="true" Width="540px" OnSelectedIndexChanged="ddlDiscuss_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 120px">Nhân vật giao lưu:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="dllGuest" runat="server" DataTextField="Title" DataValueField="DiscussId"
                                                                        AutoPostBack="false" Width="540px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Họ tên:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFullName" runat="server" Width="440px"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFullName" runat="server"
                                                                        ErrorMessage="Hãy nhập tên!" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Tuổi:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAge" runat="server" Width="100px"></asp:TextBox>&nbsp;
                                                                    <asp:RequiredFieldValidator ID="rfvtxtAge" ControlToValidate="txtAge" runat="server"
                                                                        ErrorMessage="Hãy nhập tuổi!" />
                                                                    <asp:RegularExpressionValidator ID="revAge" ControlToValidate="txtAge" ErrorMessage="Tuổi phải là kiểu số!"
                                                                        ValidationExpression="\d+" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Giới tính:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList runat="server" ID="ddlSex">
                                                                        <asp:ListItem Selected="true" Value="1">Nam</asp:ListItem>
                                                                        <asp:ListItem Value="0">Nữ</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Địa chỉ:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAddress" runat="server" Width="540px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <strong>Nội dung câu hỏi</strong>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtQuestion" runat="server"
                                                                        ErrorMessage="Hãy nhập câu hỏi!" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtQuestion" runat="server" TextMode="MultiLine" Width="670px" Height="100px"></asp:TextBox>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <strong>Nội dung trả lời</strong>
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
                                                                        <CssFiles>
                                                                            <telerik:EditorCssFile Value="~/frontend_css/vtc_news.css" />
                                                                        </CssFiles>
                                                                        <Content>
                                                                        </Content>
                                                                    </telerik:RadEditor>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </telerik:RadAjaxPanel>
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
                                                                <asp:Button ID="btnUpdate" runat="server" CssClass="button" Text="Lưu lại" OnClick="btnUpdate_Click" />
                                                                &nbsp;&nbsp;
                                                                    <asp:Button ID="btnUpdateAndMoveUp" runat="server" CssClass="button"
                                                                        Text="Gửi lên" OnClick="btnUpdateAndMoveUp_Click" />&nbsp;&nbsp;
                                                                <asp:Button ID="Button1" runat="server" CssClass="button" Width="100px"
                                                                    Text="Gửi lên Xuất bản" OnClick="btnUpdateAndMoveUpMax_Click" />&nbsp;&nbsp;
                                                                    <asp:Button ID="btnMoveDown" runat="server" CssClass="button"
                                                                        Text="Trả lại" OnClick="btnMoveDown_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 10px"></td>
                                            </tr>
                                            <tr>
                                                <td><strong>
                                                    <asp:Literal ID="ltlError" runat="server"></asp:Literal></strong></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 10px"></td>
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
    <script type="text/javascript">
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
        
        /*Telerik.Web.UI.Editor.CommandList["CustomInsertLoinoi"] = function (commandName, editor, args) {
            var myCallbackFunction = function(sender, args) {
                var arrExtension = args.returnExtension.split('|');

                editor.pasteHtml('<table id="yahoo_boxloinoi" align=' + arrExtension[1] + ' border="0" cellspacing="0" cellpadding="0"><tbody><tr><td style="width:5px">&nbsp;</td><td><table width=' + arrExtension[0] + ' align=' + arrExtension[1] + ' style="background-color:#e3edf7" cellspacing="0" cellpadding="0"><tbody><tr><td style="background-color:#3D77B1;width:' + arrExtension[0] + ';height:18px" colspan="3"></td></tr><tr><td valign="top" style="padding-left:5px;width:31px;padding-top:5px"><img alt="" width="18" height="13" style="border:0px solid" src="http://static.vtc.vn/images/news-pbdes.gif"></td><td valign="top" style="text-align:justify;padding-bottom:5px;width:98%;font:italic 14px verdana;float:left;color:#333;padding-top:5px"><span style="color:#002060"><strong>' + args.returnValue + '</strong></span> </td><td valign="bottom" style="padding-bottom:5px;width:31px"><img alt="" width="18" height="13" style="float:right;border:0px solid" src="http://static.vtc.vn/images/news-pbdes-2.gif"></td></tr><tr><td style="width:' + arrExtension[0] + ';height:10px" colspan="3"></td></tr><tr><td align="right" style="padding-bottom:5px;padding-left:5px;width:' + arrExtension[0] + ';padding-right:5px;font:13px verdana" colspan="3"><span style="color:#3D77B1">' + arrExtension[2] + '</span></td></tr></tbody></table></td><td style="width:5px">&nbsp;</td></tr></tbody></table>');

            };
            editor.showDialog("CustomInsertLoinoi", {}, myCallbackFunction);
        };*/
        
        Telerik.Web.UI.Editor.CommandList["CustomImageManager"] = function (commandName, editor, args) {
            /*var myCallbackFunction = function (sender, args) {
                var strStyle = '';
                var imgExtension = args.returnExtension.split('|');
                var imgWidth = '';
                //                if (imgExtension[0] != '100%') {
                //                    imgWidth = 'width: ' + imgExtension[0] + '; height: ' + imgExtension[1] + ';';
                //                }
                var sImg = '<img src="' + args.returnValue + '" alt="" WIDTH="' + imgExtension[0] + '" HEIGHT="' + imgExtension[1] + '" hspace="' + imgExtension[2] + '" vspace="' + imgExtension[3] + '" border="' + imgExtension[4] + '" />';

                if (imgExtension[5] == "left") {
                    strStyle = 'margin-right:7px;border:1px solid #CACACA;';
                }
                if (imgExtension[5] == "center") {
                    strStyle = 'border:1px solid #CACACA;';
                }
                if (imgExtension[5] == "right") {
                    strStyle = 'margin-left:7px;border:1px solid #CACACA;';
                }
                editor.pasteHtml('<TABLE cellSpacing=3 cellPadding=3 style="' + strStyle + '" width=1 border=0 align="' + imgExtension[5] + '"><TR><TD>' + sImg + '</TD></TR><TR><TD align=center  style="font-family:Arial; font-size:10pt;color:#002060;"><i>' + imgExtension[6] + '</i>&nbsp;</TD></TR></TABLE>');
            }
            editor.showDialog("CustomImageManager", {}, myCallbackFunction);*/
            var myCallbackFunction = function (sender, args) {
                var imgExtension = args.returnExtension.split('|');
                //                //var sImg = '<img src="' + args.returnValue + '" alt="" style="width: ' + imgExtension[0] + 'px; height: ' + imgExtension[1] + 'px;"  hspace="' + imgExtension[2] + '" vspace="' + imgExtension[3] + '" border="' + imgExtension[4] + '" align="' 
                //+ imgExtension[5] + '" />';
                //                //editor.pasteHtml('<div><TABLE cellSpacing=0 cellPadding=3 width=1 border=0 align=' + imgExtension[5] + '><TR><TD>' + sImg + '</TD></TR><TR><TD class=Image align=left>abc</TD></TR></TABLE></div>');
                var sImg = '<img src="' + args.returnValue + '" alt="" WIDTH="' + imgExtension[0] + '" HEIGHT="' + imgExtension[1] + '"  hspace="' + imgExtension[2] + '" vspace="' + imgExtension[3] + '" border="' + imgExtension[4] + '" />';
                editor.pasteHtml('<div style="text-align:' + imgExtension[5] + '; float:' + imgExtension[5] + '">' + sImg + '<br />' + imgExtension[6] + '</div>');
            };
            editor.showDialog("CustomImageManager", {}, myCallbackFunction);
        };
        Telerik.Web.UI.Editor.CommandList["CustomMediaManager"] = function (commandName, editor, args) {
            var myCallbackFunction = function (sender, args) {
                var flvExtension = args.returnExtension.split('|');
                var sEmbed = '<embed height="' + flvExtension[1] + '" width="' + flvExtension[0] + '" flashvars="file=' + args.returnValue + '&amp;width=' + flvExtension[0] + '&amp;height=' + flvExtension[1] + '&amp;autostart=true&amp;volume=100&amp;repeat=true&amp;bufferlength=10" allowscriptaccess="always" allowfullscreen="true" wmode="transparent" quality="hight" name="flvplayer" id="flvplayer" src="<%# mediaUrl %>" type="application/x-shockwave-flash"></embed>';
                editor.pasteHtml(sEmbed);
            };
            editor.showDialog("CustomMediaManager", {}, myCallbackFunction);
        };

        Telerik.Web.UI.Editor.CommandList["CustomFlashManager"] = function (commandName, editor, args) {
            var myCallbackFunction = function (sender, args) {
                var flvExtension = args.returnExtension.split('|');
                var sEmbed = '<embed src="' + args.returnValue + '" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="' + flvExtension[0] + '" height="' + flvExtension[1] + '" quality="High" wmode="transparent" />';
                editor.pasteHtml(sEmbed);
            };
            editor.showDialog("CustomFlashManager", {}, myCallbackFunction);
        };
        Telerik.Web.UI.Editor.CommandList["CustomRelatedArticle"] = function (commandName, editor, args) {
            var myCallbackFunction = function (sender, args) {
                editor.pasteHtml(args.returnValue);
            };
            editor.showDialog("CustomRelatedArticle", {}, myCallbackFunction);
        };
    </script>
</asp:Content>


