<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="boxloinoi.ascx.cs" Inherits="CMS.controls.boxloinoi" %>
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
    function OK_Clicked() {
        var sTitle = document.getElementById("txtTitle").value;
        var txtcontent = document.getElementById("txtcontent").value;
        var sWidth = '230px';
        if (sWidth == '') {
            alert("Phải nhập độ rộng!"); return;
        }
        var sAlign = document.getElementById("selAlign").value;
        var oWindow = GetRadWindow();
        var oSendArg = oWindow.Argument;
        var arg = new Object();
        if (oSendArg.InstanceId) {
            arg.InstanceId = oSendArg.InstanceId;
        }
        arg.returnValue = sTitle;
        arg.returnExtension = sWidth + '|' + sAlign + '|' + txtcontent;
        oWindow.Close(arg);
    }

    function objHide(objId) {
        if (document.getElementById(objId))
            document.getElementById(objId).style.display = 'none';
    }
    function objShow(objId) {
        if (document.getElementById(objId))
            document.getElementById(objId).style.display = '';
    }
    function objTop(objId) {
        var obj = document.getElementById(objId);
        actb_toreturn = 0;
        while (obj) {
            actb_toreturn += obj.offsetTop;
            obj = obj.offsetParent;
        }
        return actb_toreturn;
    }
    function objLeft(objId) {
        var obj = document.getElementById(objId);
        actb_toreturn = 0;
        while (obj) {
            actb_toreturn += obj.offsetLeft;
            obj = obj.offsetParent;
        }
        return actb_toreturn;
    }
    function objWidth(objId) {
        return document.getElementById(objId).offsetWidth;
    }
    function objHeight(objId) {
        return document.getElementById(objId).offsetHeight;
    }
    function removeObj(objId) {
        if (document.getElementById(objId))
            document.body.removeChild(document.getElementById(objId));
    }
    function ColorPicker(objClick, objID) {
        removeObj('ColorPicker');
        var actb_timeOut = 7200;
        var actb_toid;
        var sHTML = '';
        sHTML += '<table width="272px" border="0" cellspacing="1" cellpadding="1" style="background-color:#354157">';
        sHTML += '<tr><td  style="background-color:#ffffff">';
        sHTML += '<table id="ColorTable" border="0" cellspacing="0" cellpadding="0" width="100%" style="cursor:pointer"></table>';
        sHTML += '</td></tr></table>';
        var div = document.createElement("div");
        div.id = "ColorPicker";
        div.style.position = 'absolute';

        var top = eval(objTop(objClick) + objHeight(objClick));
        if (top + 200 > screen.height)
            top = top - 200;
        var left = eval(objLeft(objClick) + objWidth(objClick));
        if (left + 275 > screen.width)
            left = left - 275;
        div.style.top = top + "px";
        div.style.left = left + "px";
        div.style.width = "272px";
        div.style.height = "197px";
        div.innerHTML = sHTML;
        document.body.appendChild(div);
        CreateColorTable(objID);
        if (actb_toid) clearTimeout(actb_toid);
        if (actb_timeOut > 0) actb_toid = setTimeout("removeObj('ColorPicker')", actb_timeOut);
    }
    function CreateColorTable(objID) {
        // Get the target table.
        var oTable = document.getElementById('ColorTable');

        // Create the base colors array.
        var aColors = ['00', '33', '66', '99', 'cc', 'ff'];

        // This function combines two ranges of three values from the color array into a row.
        function AppendColorRow(rangeA, rangeB) {
            for (var i = rangeA; i < rangeA + 3; i++) {
                var oRow = oTable.insertRow(-1);

                for (var j = rangeB; j < rangeB + 3; j++) {
                    for (var n = 0; n < 6; n++) {
                        AppendColorCell(oRow, '#' + aColors[j] + aColors[n] + aColors[i]);
                    }
                }
            }
        }

        // This function create a single color cell in the color table.
        function AppendColorCell(targetRow, color) {
            var oCell = targetRow.insertCell(-1);
            oCell.style.width = "15px";
            oCell.style.height = "15px";
            oCell.bgColor = color;

            oCell.onmouseover = function () {
                document.getElementById(objID).value = this.bgColor;
                document.getElementById(objID).style.backgroundColor = this.bgColor;
            }
            oCell.onclick = function () {
                document.getElementById(objID).value = this.bgColor;
                removeObj('ColorPicker');
            }
        }

        AppendColorRow(0, 0);
        AppendColorRow(3, 0);
        AppendColorRow(0, 3);
        AppendColorRow(3, 3);

        // Create the last row.
        var oRow = oTable.insertRow(-1);

        // Create the gray scale colors cells.
        for (var n = 0; n < 6; n++) {
            AppendColorCell(oRow, '#' + aColors[n] + aColors[n] + aColors[n]);
        }

        // Fill the row with black cells.
        for (var i = 0; i < 12; i++) {
            AppendColorCell(oRow, '#000000');
        }
    }
</script>
<br />
<fieldset>
    <legend class="text"><strong>Thuộc tính Box</strong></legend>
    <table style="width: 100%" cellpadding="1" cellspacing="1" class="text">
        <tr>
            <td style="width: 100px">
                Nội dung:
            </td>
            <td>
                <textarea id="txtTitle" style="width: 400px; height: 150px"></textarea>
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                Người phát ngôn:
            </td>
            <td>
                <input type="text" id="txtcontent" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <td>
                Vị trí:
            </td>
            <td>
                <select id="selAlign" style="width: 205px">
                    <option value="left" selected="selected">Trái</option>
                    <option value="right">Phải</option>
                </select>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <input type="button" class="button" value="Ghi lại" onclick="OK_Clicked();" />
            </td>
        </tr>
    </table>
</fieldset>