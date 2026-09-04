<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="clearhtml.ascx.cs" Inherits="CMS2012.controls.clearhtml" %>
<link href="<%# UrlRoot%>css/css.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/backend.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/style_repeater.css" rel="stylesheet" type="text/css" />
<link href="<%# UrlRoot%>css/paper.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadEditor ID="RadContent" runat="server" Width="670px" Height="500px" Skin="Default"
    ToolsFile="~/RadControls/Editor/CustomTools.xml" ContentFilters="FixEnclosingP">
    <CssFiles>
        <telerik:EditorCssFile Value="~/css/common.css" />
    </CssFiles>
    <Content>                                                                         </Content>
</telerik:RadEditor>
<br />
<script type="text/javascript">
    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }
    function OK_Clicked() {

        var oWindow = GetRadWindow();
        var oSendArg = oWindow.Argument;
        var arg = new Object();
        if (oSendArg.InstanceId) {
            arg.InstanceId = oSendArg.InstanceId;
        }
        arg.returnValue = "<%=content %>";
        arg.returnExtension = "<%=content %>";
        oWindow.Close(arg);
    }

</script>
<asp:Button ID="btnRemove" runat="server" CssClass="button" Text="Xóa" OnClick="Removetagshtml_Click" />
<input type="button" class="button" value="Ghi lại" onclick="OK_Clicked();" />