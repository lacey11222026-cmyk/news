
/**
* jQuery.services
* @date: 22/12/2010
* @author: manhcuong.phung
* @version: 2010.12.22
**/
(function ($)
{
    var Debug = false;
    if ((window.location.href).search('http://localhost') != -1)
        Debug = true;

    $.Services = function (o)
    {
        o = $.extend({
            // element options
            isCrossDomain: true,
            serviceDomain: GET_URL,
            serviceName: null,
            element: null,
            loadingImg: '<div align="left" style="width:100%;min-height:270px;padding:5px;">Đang tải dữ liệu ...</div>',
            noDataText: '<div align="left" style="width:100%;min-height:270px;padding:5px;">Không có dữ liệu</div>',
            // getJson options         
            type: 'POST',
            url: null,
            // ajax options
            async: true,
            beforeSend: null,
            cache: true,
            complete: null,
            contentType: 'application/x-www-form-urlencoded',
            context: null,
            data: null,
            dataFilter: null,
            dataType: 'Intelligent Guess (xml, json, script, or html)',
            error: null,
            global: true,
            ifModified: false,
            jsonp: 'callback',
            jsonpCallback: null,
            password: '',
            processData: true,
            scriptCharset: '',
            success: null,
            timeout: 6000,
            username: '',
            xhr: null,
            // jcache options
            jcacheKey: null,
            // jtemplate options
            isScriptTagTemplate: false,
            jTemplateUrl: null,
            jTemplateElement: null,
            responseData: null
        }, o || {});

        var servicesObj = $.ServicesObj;
        servicesObj._init(o, o.element);
    }

    $.ServicesObj = {
        settings: {},
        _init: function (options, element)
        {
            var self = this;
            self.settings = options;
            var settings = self.settings;

            if (element != undefined)
            {
                settings.element = element;
                if (settings.loadingImg != "none" && settings.loadingImg != "" && settings.loadingImg != null && settings.loadingImg != undefined && element != null && element != undefined)
                    $(element).html(settings.loadingImg);
            }

            this._callService(settings);
        },
        _getJcache: function (key)
        {
            if (key != null && key != undefined && key != "")
                return $.jCache.getItem(key);
            return null;
        },
        _setJcache: function (key, data)
        {
            try
            {
                if (key != null && key != undefined && key != "")
                {
                    var jcacheData = this._getJcache(key);
                    if (jcacheData == null || jcacheData == undefined || jcacheData == "" || typeof (jcacheData) == 'undefined')
                    {
                        $.jCache.setItem(key, data);
                        return;
                    }
                }
                return;
            }
            catch (e)
            {
                if (Debug)
                    alert("Set Jcache Exception" + e);
                return;
            }
        },
        _setJTemplate: function (element, data, settings)
        {
            try
            {
                if (settings.jTemplateUrl == null && settings.jTemplateElement == null)
                    return true;

                if (element != null)
                {
                    if (data == null)
                    {
                        $(element).html(settings.noDataText);
                        return true;
                    }

                    if (settings.jTemplateUrl != null)
                    {
                        $(element).setTemplateURL(settings.jTemplateUrl);
                        $(element).processTemplate(data);
                        return true;
                    }

                    if (settings.jTemplateElement != null)
                    {
                        if (!settings.isScriptTagTemplate)
                        {
                            $(element).setTemplateElement(settings.jTemplateElement);
                            $(element).processTemplate(data);
                        }
                        else
                        {
                            bindTemplateData($(element).attr('id'), settings.jTemplateElement, data);
                        }

                        return true;
                    }

                }

                return true;
            }
            catch (e)
            {
                if (Debug)
                    alert("Set JTemplate Exception :" + e);
                return false;
            }
        },
        _onSuccess: function (settings)
        {
            try
            {
                var responseData = (settings.responseData != undefined && settings.responseData != null && settings.responseData != "") ? settings.responseData : null;
                if (settings.success != null)
                {
                    if (responseData != null)
                        settings.success.call(settings, responseData);
                    else
                        settings.success.call(settings, "");
                }

                if (settings.complete != null)
                {
                    if (responseData != null)
                        settings.complete.call(settings, responseData);
                    else
                        settings.success.call(settings, "");
                }
            }
            catch (e)
            {
                if (Debug)
                    alert("Element : " + $(settings.element).attr("id") + " || On Success  Function || Complete Exception : " + e);
                return;
            }
        },
        _callService: function (settings)
        {
            var self = this;
            if (settings.isCrossDomain)
            {
                settings.responseData = self._getJcache(settings.jcacheKey);

                if (settings.responseData != null)
                {
                    if (self._setJTemplate(settings.element, settings.responseData, settings))
                    {
                        self._onSuccess(settings);
                        return;
                    }
                }
                settings.url = (settings.isCrossDomain) ? settings.serviceDomain + settings.serviceName : settings.serviceName;
                settings.url += "?jsoncallback=?";

                try
                {
                    $.getJSON(settings.url, settings.data, function (data)
                    {
                        settings.responseData = data;
                        self._setJcache(settings.jcacheKey, data);
                        if (self._setJTemplate(settings.element, settings.responseData, settings))
                            self._onSuccess(settings);
                        return;
                    });
                }
                catch (e)
                {
                    if (Debug)
                        alert("$.getJSON Exception " + e);
                    return;
                }

            }
            else
            {
                try
                {
                    $.ajax({
                        type: settings.type,
                        url: settings.url,
                        async: settings.async,
                        beforeSend: settings.beforeSend,
                        cache: settings.cache,
                        complete: settings.complate,
                        contentType: settings.contentType,
                        context: settings.context,
                        data: settings.data,
                        dataFilter: settings.dataFilter,
                        dataType: settings.dataType,
                        error: settings.error,
                        success: settings.success,
                        timeout: settings.timeout
                    });
                    return;
                }
                catch (e)
                {
                    if (Debug)
                        alert("$.Ajax Exception " + e);
                    return;
                }
            }
        }
    }
})(jQuery);
