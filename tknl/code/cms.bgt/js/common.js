// JScript File
function EnLarge()
{

    var tdLeft = document.getElementById('tdLeft');
    var tdRight = document.getElementById('tdRightContent');
    var nLeft = tdLeft.offsetWidth;
    if(nLeft < eval(screen.width - 500)){
        tdRight.style.width = eval(tdRight.offsetWidth - 50)+"px";
        tdLeft.style.width = eval(nLeft + 50)+"px";
        
    }
}
function Decrease()
{
    var tdLeft = document.getElementById('tdLeft');
    var tdRight = document.getElementById('tdRightContent');  
    nLeft = tdLeft.offsetWidth;
    if(nLeft > 250){    
        tdLeft.style.width = eval(nLeft - 50)+"px";
        tdRight.style.width = eval(tdRight.offsetWidth + 50)+"px";
    }
}
function removeObj(objId){
    if (document.getElementById(objId)) 
        document.body.removeChild(document.getElementById(objId));
}
function GetScreenWidth()
{
    return screen.width;
}
function GetScreenHeight()
{
    return screen.height;
}
function trim(str, chars) {
    return ltrim(rtrim(str, chars), chars);
}

function ltrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

function rtrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}
function __keyPress(event, href) {
    var keyCode;
    if (typeof(event.keyCode) != "undefined") {
        keyCode = event.keyCode;
    }
    else {
        keyCode = event.which;
    }

    if (keyCode == 13) {
        window.location = href;
    }
}
function ShowToolTip(objRelated,sTitle)
{
    var c = theForm.getElementsByTagName('input');
    var l = c.length;
    for(var i=0; i<l; i++)
    {
        if (c[i].name.indexOf(objRelated) >= 0)
        c[i].title  = sTitle; 
    }

}

