var serviceURL = "http://localhost:2019/api/";
$.support.cors = true;
var ServerRequest = ServerRequest || {};

ServerRequest.prototype.sType;
ServerRequest.prototype.sMethod;
ServerRequest.prototype.sCallback;
ServerRequest.prototype.sParameter;
ServerRequest.prototype.sReturnObject

function parseJSONResponse(response) {
    var jsonData = JSON.stringify(response);
    return $.parseJSON(jsonData);
}

function ServerRequest(requestType, methodName, callbackResponse, parameterString, returnObject, ByPassParam) {
    this.sType = requestType;
    this.sMethod = methodName;
    this.sCallback = callbackResponse;
    this.sParameter = parameterString;
    this.sReturnObject = returnObject;
    this.sByPassParam = ByPassParam;
}

ServerRequest.prototype.makeRequest = function () {
    $.support.cors = true;
    var sCallback = this.sCallback;
    var sReturnObject = this.sReturnObject;
    var sByPassParam = this.sByPassParam;
    try {
        if (this.sParameter == "") {
            this.sParameter = "{}";
        }

        //$(document).ajaxStart(function () {
        //    loading();
        //});
        //$(document).ajaxStop(function () {
        //    unloading();
        //});
        var XHR = $.ajax({
            cache: false,
            type: this.sType,
            contentType: "application/json; charset=utf-8",
            url: serviceURL + "/" + this.sMethod,
            data: JSON.stringify(this.sParameter),
            dataType: "json",
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

ServerRequest.prototype.makeSyncRequest = function () {
    $.support.cors = true;
    var sCallback = this.sCallback;
    var sReturnObject = this.sReturnObject;
    var sByPassParam = this.sByPassParam;
    try {

        if (this.sParameter == "") {
            this.sParameter = "{}";
        }

        //$(document).ajaxStart(function () {
        //    loading();
        //});
        //$(document).ajaxStop(function () {
        //    unloading();
        //});
        var XHR = $.ajax({
            cache: false,
            type: this.sType,
            contentType: "application/json; charset=utf-8",
            url: serviceURL + "/" + this.sMethod,
            data: JSON.stringify(this.sParameter),
            dataType: "json",
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

// show loading passed by name
function loading() {
    
    $("#loading").show();
    $('.wrapper').fadeTo("fast", 0.1);
}
// hide loading and overlay
function unloading() {
    $("#loading").hide();
    $('.wrapper').fadeTo("fast", 5);
}

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

ServerRequest.prototype.makeRequestWithoutLoadingPanel = function () {
    $.support.cors = true;
    var sCallback = this.sCallback;
    var sReturnObject = this.sReturnObject;
    var sByPassParam = this.sByPassParam;
    try {
        if (this.sParameter == "") {
            this.sParameter = "{}";
        }

        //$(document).ajaxStart(function () {
        //    loading();
        //});
        //$(document).ajaxStop(function () {
        //    unloading();
        //});
        var XHR = $.ajax({
            cache: false,
            type: this.sType,
            contentType: "application/json; charset=utf-8",
            url: serviceURL + "/" + this.sMethod,
            data: JSON.stringify(this.sParameter),
            dataType: "json",
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
