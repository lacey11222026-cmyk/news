var GET_URL = '/Get/';
var POST_URL = '/Post/';
// IMAGE_UPLOAD_URL = '/Images/Upload/';
var DEBUG_MODE = false;


if ((window.location.href).search('http://localhost') != -1)
{
    GET_URL = 'http://localhost:64698/Get/';
    POST_URL = 'http://localhost:64698/Post/';
//IMAGE_UPLOAD_URL = 'http://localhost:64696/Images/Upload/';
     DEBUG_MODE = true;
}
var NewsStatus = {
    Disable: 0,
    All : -1,
    Disable : 0,
    Publish : 1,
    Draft : 2,
    EditWait : 3,
    //Editting = 4,
    PublishWait : 5
};
var NewsAction = {
    Publish: "1",
    SendPublish: "2",
    Reject: "3",
    Save: "4",
    Delete: "5",
    SendEdit: "6",
    Down: "7",
    Restore: "8",
    RejectBT: "9",
    GetBack:"10"
};