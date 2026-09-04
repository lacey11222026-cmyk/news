<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMS.common_flashmanager" Codebehind="flashmanager.aspx.cs" %>

<%@ Register Src="../controls/flashmanager.ascx" TagName="flashmanager" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Nhập flash</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="content-language" content="en" />
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <meta name="description" content="Dao Trung Hieu, Đào Trung Hiếu" />
    <meta name="keywords" content="hieudt, hieudtvn, hieu.dao" />
    <meta name="author" content="hieu.dao@vtc.vn" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <uc1:flashmanager ID="flashmanager1" runat="server" />
    </form>
</body>
</html>
