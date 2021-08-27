$(document).ready(function () {
    fn_runningdata();
    $("#txtrunningSearch").keypress(function (e) {

        if (e.which == 13) {
            fn_runningdata($(this).val());
            return false;
        }
    });

    $("#txtobsoleteSearch").keypress(function (e) {
        if (e.which == 13) {
            fn_obsoletedata($(this).val());
            return false;
        }
    });



});

var lid = $("#hid_lid").val();

function get_hostname(url) {
	var m = url.match(/^http:\/\/[^/]+/);
	return m ? m[0] : null;
}

var pagevariable = {};
var domainname = $('#domainname').val();
pagevariable.imageurl = domainname+'images/cars/';
pagevariable.pdficonurl = domainname +'images/model-box-icon1.png';
pagevariable.videoiconeurl = domainname + 'images/model-box-icon2.png';

function imageExists(url, callback) {
    var img = new Image();
    img.onload = function () { callback(true); };
    img.onerror = function () { callback(false); };
    img.src = url;
}

function imageExistsnew(image_url) {

    var http = new XMLHttpRequest();

    http.open('HEAD', image_url, false);
    http.send();

    return http.status != 404;

}

var noimgpath = "";


function fn_runningdata(searchtxt) {
    var objmodel = {};
    objmodel.mstatus = "Y";
    objmodel.lid = lid;
    objmodel.search = searchtxt;
    $.ajax({
        type: "POST",
        url: domainname+"dealer/GetModeldashbaord",
        data: JSON.stringify(objmodel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var dataHtml = '<ul>';
            $.each(response, function (index, item) {
                var str = item.SMAS_MODEL_DESC;
                str.replace(' ', '');
                var img = pagevariable.imageurl + $.trim(str) + '.png';
               
                dataHtml += '<li><a href="' + domainname + 'dealer/Modeldetails/' + item.SMAS_MODEL_CD + '?langugeid=' + lid + '&status=Y"><span><img src="' + img + '" style="height: 110px;"></span><div class="carFeature"><h3>' + item.SMAS_MODEL_DESC + '</h3><div class="carFeatureIcon">';
				dataHtml += '<div class="carIcon"><a href="#"><img src="' + pagevariable.pdficonurl +'"></a></div><div class="carIconTxt"><a href="#">' + item.PDF + '</a></div>';
				dataHtml += '<div class="carIcon"><a href="#"><img src="' + pagevariable.videoiconeurl +'"></a></div><div class="carIconTxt"><a href="#">' + item.Video + '</a></div>';
                dataHtml += '</div></div></a></li>';
            });
            dataHtml += '</ul>';
            $('#runningdivdata').html(dataHtml);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function fn_obsoletedata(searchtxt)
{

	
    var objmodel = {};
    objmodel.mstatus = "N";
    objmodel.lid = lid;
    objmodel.search = searchtxt;
    $.ajax({
        type: "POST",
        url: domainname + "dealer/GetModeldashbaord",
        data: JSON.stringify(objmodel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var dataHtml = '<ul>';
            $.each(response, function (index, item) {
                var str = item.SMAS_MODEL_DESC;
                str.replace(' ', '');
                var img = pagevariable.imageurl + $.trim(str) + '.png';

                dataHtml += '<li><a href="' + domainname + 'dealer/Modeldetails/' + item.SMAS_MODEL_CD + '?langugeid=' + lid + '&status=N"><span><img src="' + img + '" style="height: 110px;"></span><div class="carFeature"><h3>' + item.SMAS_MODEL_DESC + '</h3><div class="carFeatureIcon">';
				dataHtml += '<div class="carIcon"><a href="#"><img src="' + pagevariable.pdficonurl+'"></a></div><div class="carIconTxt"><a href="#">' + item.PDF + '</a></div>';
				dataHtml += '<div class="carIcon"><a href="#"><img src="' + pagevariable.videoiconeurl +'"></a></div><div class="carIconTxt"><a href="#">' + item.Video + '</a></div>';
                dataHtml += '</div></div></a></li>';
            });
            dataHtml += '</ul>';
            $('#obsoletedivdata').html(dataHtml);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}