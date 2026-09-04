<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMS.keepalive" Codebehind="keepalive.aspx.cs" %>

<html>
<head>
    <meta http-equiv="Refresh" content="<%=10*60%>; URL='/keepalive.aspx'">
</head>
<body>
    <div style="display: none">

        <script type='text/javascript' language='JavaScript' src='http://xslt.alexa.com/site_stats/js/t/a?url=<%= Request.Url.Host.ToString()%>'></script>

    </div>
</body>
</html>
