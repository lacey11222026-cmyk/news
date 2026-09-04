var GET_URL = '/Get/';
var POST_URL = '/Post/';
var DEBUG_MODE = false;

if ((window.location.href).search('http://localhost') != -1)
{
    GET_URL = 'http://localhost:9163/Get/';
    POST_URL = 'http://localhost:9163/Post/';
     IMAGE_UPLOAD_URL = '/Images/Upload/';
     DEBUG_MODE = true;
}
