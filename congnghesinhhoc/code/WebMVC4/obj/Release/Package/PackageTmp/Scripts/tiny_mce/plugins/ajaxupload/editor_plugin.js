/**
* editor_plugin_src.js
*
* Copyright 2009, Moxiecode Systems AB
* Released under LGPL License.
*
* License: http://tinymce.moxiecode.com/license
* Contributing: http://tinymce.moxiecode.com/contributing
* 
* Created by: manhcuong.phung
* Description: combine ajaxupload plugin to tinyCME
* 15/10/2011
*/

(function () {
    // Load plugin specific language pack
    //tinymce.PluginManager.requireLangPack('example');

    tinymce.create('tinymce.plugins.ajaxUpload', {
        /**
        * Initializes the plugin, this will be executed after the plugin has been created.
        * This call is done before the editor instance has finished it's initialization so use the onInit event
        * of the editor instance to intercept that event.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function (ed, url) {

            // Register the command so that it can be invoked by using tinyMCE.activeEditor.execCommand('mceExample');
            ed.addCommand('openPopup', function () {

                var button = {
                    "Chèn ảnh": function () {

                        var mceContent = '';
                        var width = '';
                        var height = '';
                        var align = '';
                        var margin = "margin:0;";
                        var style = '';

                        if ($("#margin").val() != '') {
                            margin = "margin:" + $("#margin").val() + "px ;";
                        }

                        if ($("#width").val() != '') {
                            width = "width=\" " + $("#width").val() + "\"";
                        }

                        if ($("#height").val() != '') {
                            width = "height=\" " + $("#height").val() + "\"";
                        }



                        if ($('#align option:selected').val() != '0') {
                            align = "data-mce-style=\" vertical-align:" + $('#align option:selected').val() + ";" + margin + "\"";
                            style = "style=\" vertical-align:" + $('#align option:selected').val() + ";" + margin + "\"";

                            if ($('#align option:selected').val() == 'center') {
                                align = "data-mce-style=\"  margin-left: auto; margin-right: auto ;display: block" + ";" + margin + "\"";
                                style = "style=\"  margin-left: auto;margin-right: auto ;display: block" + ";" + margin + "\"";

                            }
                            if ($('#align option:selected').val() == 'right' || $('#align option:selected').val() == 'left') {
                                align = "data-mce-style=\" float:" + $('#align option:selected').val() + ";" + margin + "\"";
                                style = "style=\" float:" + $('#align option:selected').val() + ";" + margin + "\"";
                            }

                        }
                        else {

                            align = "data-mce-style=\"" + margin + "\"";
                            style = "style=\"" + margin + "\"";
                        }

                        $("#tinymce_ajaxupload_img_cont").find('input[name="image_name"]:checked').each(function (i, e) {
							
                            mceContent += '<img src="' + imageUrl + $(e).val() + '" ' + width + ' ' + height + ' ' + align + ' ' + style + '  /><br/>';
                        });

                        ed.execCommand('mceInsertContent', false, mceContent);
                        DestroyDialog();
                    },
                    "Đóng": function () {
                        DestroyDialog();
                    }
                };

                var html = _renderHtml();
                OpenDialog(true, button, 'Chèn ảnh', html, 915);

                _loadImages();
                _renderAjaxUpload("#tinymce_ajaxupload_img_btn");

            });

            // Register example button
            ed.addButton('ajaxupload', {
                title: 'Tải ảnh từ máy tính ',
                cmd: 'openPopup',
                image: url + '/img/upload.jpg'
            });

            // Add a node change handler, selects the button in the UI when a image is selected
            ed.onNodeChange.add(function (ed, cm, n) {
                cm.setActive('openPopup', n.nodeName == 'IMG');
            });

            function _renderHtml() {

                var html = '';
                html += '<input id="tinymce_ajaxupload_img_btn" type="button" value="Tải ảnh từ máy tính"  />';

                html += '<div id="tinymce_ajaxupload_img_cont" style="border:1px solid #CCCCCC;width:880px;min-height:400px;padding:5px;margin-top:5px;">';
                html += '</div>';

                html += '<div style="clear:both"></div><div>';
                html += 'Điều chỉnh&nbsp;&nbsp;<select id=\"align\" ><option value=\"0\">-- Not set --</option><option value=\"baseline\">Baseline</option><option value=\"top\">Top</option><option value=\"middle\">Middle</option><option value=\"bottom\">Bottom</option><option value=\"text-top\">Text top</option><option value=\"text-bottom\">Text bottom</option><option value=\"center\">Center</option><option value=\"right\">Right</option><option value=\"left\">Left</option>	</select>&nbsp;';
                html += 'Kích thước ảnh&nbsp;&nbsp;<input id=\"width\" type=\"text\" maxlength=\"5\" size=\"3\" value=\"\" name=\"width\">x<input id=\"height\" type=\"text\" maxlength=\"5\" size=\"3\" value=\"\" name=\"height\">';
                html += '&nbsp;Khảng cách&nbsp;&nbsp;<input id=\"margin\" type=\"text\" maxlength=\"5\" size=\"3\" value=\"\" name=\"margin\"></div>';
                return html;

            }

            //var imageUrl = "/Images/Upload/tinycme/0/0/1/";

            var imageUrl = "/Images/Upload/" + EditorUploadUrl + '/';
            function _loadImages() {

                $.Services({
                    serviceName: 'ImageService.ashx',
                    data: {
                        __m: 'tinycme',
                        _id: 1,
                        _url: EditorUploadUrl

                    },
                    success: function (data) {
                        if (data != undefined && data != null && data != '') {

                            $.each(data, function (i, item) {

                                var html = '<div style="float:left;width:100px;margin:5px;">';
                                html += '<img width="100" height="100" src="' + imageUrl + item + '"  />';
                                html += '<input name="image_name" type="checkbox" value="' + item + '"  />';
                                //                                html += '<input type="button" value="Xóa" onclick="DeleteImage(\'' + item + '\',this)" />';
                                html += '</div>';

                                $('#tinymce_ajaxupload_img_cont').append(html);

                            });
                        }

                    }

                });

            }
            function RenderPhotoUpload(eid) {
                new AjaxUpload(eid, {
                    action: window.PostUrl + 'PhotoService.ashx',
                    data: {
                        __m: 'save',
                        title: '',
                        aid: 0,
                        ordering: 0,
                        published: 1
                    },
                    autoSubmit: false,
                    onChange: function (file, ext) {
                        var ajaxupload = this;
                        ajaxupload._settings.data.ext = ext;
                        ajaxupload.submit();
                    },
                    onSubmit: function (file, ext) {

                        if (!(ext && /^(jpg|png|jpeg|gif|bmp)$/.test(ext))) {
                            OpenAlertDialog('File ảnh chỉ cho phép định dạng có đuôi mở rộng là: .jpg');
                            return false;
                        }

                        this.disable();
                    },
                    onComplete: function (file, response) {
                        DestroyDialog();
                        var responseJson = $.parseJSON(response.substring(response.indexOf("{"), response.lastIndexOf("}") + 1));
                        //responseJson = eval('(' + responseJson + ')');
                        OpenAlertDialog(responseJson.Text);


                    }
                });

            }

            function _renderAjaxUpload(eid) {

                new AjaxUpload(eid, {

                    action: POST_URL + 'ImageService.ashx',

                    data: {
                        __m: 'upl',
                        _id: 1,
                        _en: 'tinycme',
                        _url: EditorUploadUrl,
                        ext: ''
                    },
                    autoSubmit: false,
                    onChange: function (file, ext) {

                        var ajaxupload = this;
                        ajaxupload._settings.data.ext = ext;
                        ajaxupload.submit();

                    },
                    onSubmit: function (file, ext) {

                        if (!(ext && /^(jpg|png|jpeg|gif|bmp)$/.test(ext))) {
                            OpenAlertDialog('File ảnh chỉ cho phép định dạng có đuôi mở rộng là: .jpg, .png, .jpeg, .gif, .bmp');
                            return false;
                        }

                        this.disable();
                    },
                    onComplete: function (file, response) {

                        var responseJson = $.parseJSON(response.substring(response.indexOf("{"), response.lastIndexOf("}") + 1));

                        if (responseJson.Success) {

                            var html = '<div style="float:left;width:100px;margin:5px;">';
                            html += '<img width="100" height="100" src="' + imageUrl + responseJson.Value + '"  />';
                            html += '<input name="image_name" type="checkbox" value="' + responseJson.Value + '"  />';
                            //                            html += '<input type="button" value="Xóa" onclick="DeleteImage(\'' + responseJson.Value + '\',this)" />';
                            html += '</div>';

                            $('#tinymce_ajaxupload_img_cont').append(html);
                        }


                        this.enable();
                    }
                });

            }


        },

        /**
        * Creates control instances based in the incomming name. This method is normally not
        * needed since the addButton method of the tinymce.Editor class is a more easy way of adding buttons
        * but you sometimes need to create more complex controls like listboxes, split buttons etc then this
        * method can be used to create those.
        *
        * @param {String} n Name of the control to create.
        * @param {tinymce.ControlManager} cm Control manager to use inorder to create new control.
        * @return {tinymce.ui.Control} New control instance or null if no control was created.
        */
        //        createControl: function (n, cm) {
        //            return null;
        //        },

        /**
        * Returns information about the plugin as a name/value array.
        * The current keys are longname, author, authorurl, infourl and version.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function () {
            return {
                longname: 'Ajax Upload Plugin',
                author: 'manhcuong.phung',
                authorurl: 'www.go.vn',
                infourl: 'www.go.vn',
                version: "1.0"
            };
        }




    });

    // Register plugin
    tinymce.PluginManager.add('ajaxUpload', tinymce.plugins.ajaxUpload);
})();