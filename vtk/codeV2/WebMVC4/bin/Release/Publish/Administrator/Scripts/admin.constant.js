var GET_URL = '/Get/';
var POST_URL = '/Post/';
var IMAGE_UPLOAD_URL = '/Images/Upload/';
var DEBUG_MODE = false;


if ((window.location.href).search('http://localhost') != -1)
{
    GET_URL = 'http://localhost:64696/Get/';
    POST_URL = 'http://localhost:64696/Post/';
     IMAGE_UPLOAD_URL = '/Images/Upload/';
     DEBUG_MODE = true;
}
