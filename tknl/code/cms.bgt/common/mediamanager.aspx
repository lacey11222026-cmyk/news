<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMS.common_mediamanager" Codebehind="mediamanager.aspx.cs" %>

<%@ Register src="../controls/mediamanager.ascx" tagname="mediamanager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Nhập Clip</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="description" content="Dao Trung Hieu, Đào Trung Hiếu" />
    <meta name="keywords" content="hieudt, hieudtvn, hieu.dao" />
    <meta name="author" content="hieu.dao@vtc.vn" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <uc1:mediamanager ID="mediamanager1" runat="server" />
    </form>
</body>
</html>
