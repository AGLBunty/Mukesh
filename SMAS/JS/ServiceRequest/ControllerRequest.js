var controllerRequestURL = "http://localhost:2019/";
$.support.cors = true;
var ControllerRequest = ControllerRequest || {};

ControllerRequest.prototype.sType;
ControllerRequest.prototype.sMethod;
ControllerRequest.prototype.sCallback;
ControllerRequest.prototype.sParameter;
ControllerRequest.prototype.sReturnObject
ControllerRequest.prototype.sDataType;

function ControllerRequest(requestType, dataType, methodName, callbackResponse, parameterString, returnObject, ByPassParam) {
   this.sType = requestType;
   this.sMethod = methodName;
   this.sCallback = callbackResponse;
    this.sParameter = parameterString;
    this.sReturnObject = returnObject;
    this.sByPassParam = ByPassParam;
    this.sDataType = dataType;

}

 ControllerRequest.prototype.makeRequest = function () {
     $.support.cors = true;
     var sCallback = this.sCallback;
     var sReturnObject = this.sReturnObject;
     var sByPassParam = this.sByPassParam;
    try {
        if (this.sParameter == "") {
            this.sParameter = "{}";
        }
        var XHR = $.ajax({
            cache: false,
            type: this.sType,
            //contentType: "application/json; charset=utf-8",
            url: controllerRequestURL + "/" + this.sMethod,
            data: this.sParameter,
            dataType: this.sDataType,
            timeout: 360000,
            crossDomain: true
        }).done(function (data) { return setTimeout(function () { sCallback(data, sReturnObject, sByPassParam); }, 300); })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                jAlert(masterPageData.ConnectionError + ": " + errorThrown, masterPageData.WarningAlert);
            });

    } catch (e) {
        jAlert(masterPageData.ServiceError + ": " + e.message, masterPageData.WarningAlert);
    }
};

 ControllerRequest.prototype.makeSyncRequest = function () {
     $.support.cors = true;
     var sCallback = this.sCallback;
     var sReturnObject = this.sReturnObject;
     var sByPassParam = this.sByPassParam;
    try {

        if (this.sParameter == "") {
            this.sParameter = "{}";
        }
       
      
        var XHR = $.ajax({
            cache: false,
            type: this.sType,
            //contentType: "application/json; charset=utf-8",
            url: controllerRequestURL + "/" + this.sMethod,
            data: this.sParameter,
            dataType: this.sDataType,
            timeout: 360000,
            crossDomain: true,
            async: false
        }).done(function (data) { return setTimeout(function () { sCallback(data, sReturnObject, sByPassParam); }, 300); })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                jAlert(masterPageData.ConnectionError + ": " + errorThrown, masterPageData.WarningAlert);
            });

    } catch (e) {
        jAlert(masterPageData.ServiceError + ": " + e.message, masterPageData.WarningAlert);
    }
};


// show loading passed by name Popup
function loadingInner() {
   $('#overlayInner').css('opacity', 0.1).fadeIn('fast');
    $('#preloaderInner').fadeIn('fast');
}

// hide loading and overlay Popup
function unloadingInner() {
    $('#preloaderInner').fadeOut('fast');
    $('#overlayInner').fadeOut('fast');
}

function parseJSONResponse(response) {
    var jsonData = JSON.stringify(response);
    return $.parseJSON(jsonData);
}

ControllerRequest.prototype.makeRequestWithoutLoadingPanel = function () {
    $.support.cors = true;
    var sCallback = this.sCallback;
    var sReturnObject = this.sReturnObject;
    var sByPassParam = this.sByPassParam;
    try {
        if (this.sParameter == "") {
            this.sParameter = "{}";
        }
      
        var XHR = $.ajax({
            cache: false,
            type: this.sType,
            //contentType: "application/json; charset=utf-8",
            url: controllerRequestURL + "/" + this.sMethod,
            data: this.sParameter,
            dataType: this.sDataType,
            timeout: 360000,
            crossDomain: true
        }).done(function (data) { return setTimeout(function () { sCallback(data, sReturnObject, sByPassParam); }, 300); })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                jAlert(masterPageData.ConnectionError + ": " + errorThrown, masterPageData.WarningAlert);
            });

    } catch (e) {
        jAlert(masterPageData.ServiceError + ": " + e.message, masterPageData.WarningAlert);
    }
};


ControllerRequest.prototype.makefilerequest = function () {
    $.support.cors = true;
    var sCallback = this.sCallback;
    var sReturnObject = this.sReturnObject;
    var sByPassParam = this.sByPassParam;
    try {
        if (this.sParameter == "") {
            this.sParameter = "{}";
        }
        var XHR = $.ajax({
            cache: false,
            type: this.sType,
            url: controllerRequestURL + this.sMethod,
            data: this.sParameter,
            dataType: this.sDataType,
            timeout: 360000,
            crossDomain: true
        }).done(function (response, status, xhr) {
            // check for a filename
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            var type = xhr.getResponseHeader('Content-Type');
            var blob = new Blob([response], { type: type });

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    // use HTML5 a[download] attribute to specify filename
                    var a = document.createElement("a");
                    // safari doesn't support this yet
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
            }
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                jAlert(masterPageData.ConnectionError + ": " + errorThrown, masterPageData.WarningAlert);
            });

    } catch (e) {
        jAlert(masterPageData.ServiceError + ": " + e.message, masterPageData.WarningAlert);
    }
};