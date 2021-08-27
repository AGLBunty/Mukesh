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
        url: domainname+"admin/GetModeldashbaord",
        data: JSON.stringify(objmodel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var dataHtml = '<ul>';
            $.each(response, function (index, item) {
                var str = item.SMAS_MODEL_DESC;
                str.replace(' ', '');
                var img = pagevariable.imageurl + $.trim(str) + '.png';
                dataHtml += '<li><a href="' + domainname + 'admin/Modeldetails/' + item.SMAS_MODEL_CD + '?langugeid=' + lid + '&status=Y"><span><img src="' + img + '" style="height: 110px;"></span><div class="carFeature"><h3>' + item.SMAS_MODEL_DESC + '</h3><div class="carFeatureIcon">';
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
        url: domainname+"admin/GetModeldashbaord",
        data: JSON.stringify(objmodel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var dataHtml = '<ul>';
            $.each(response, function (index, item) {
                var str = item.SMAS_MODEL_DESC;
                str.replace(' ', '');
               
                var img = pagevariable.imageurl + $.trim(str) + '.png';
                dataHtml += '<li><a href="' + domainname + 'admin/Modeldetails/' + item.SMAS_MODEL_CD + '?langugeid=' + lid + '&status=N"><span><img src="' + img + '" style="height: 110px;"></span><div class="carFeature"><h3>' + item.SMAS_MODEL_DESC + '</h3><div class="carFeatureIcon">';
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

function fn_dealers(obj)
{
    var objmodel = {};
    objmodel.Regionid = $("#REGION_CD option:selected").val();

    $('#drpDealer').html('');
    $("#drpDealer").append('<option value="" selected="selected">Select Dealer</option>');
    $.ajax({
        type: "POST",
        url: domainname + "admin/GetDealers",
        data: JSON.stringify(objmodel),
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        async: "true",
        success: function (response) {
           
            $.each(response, function (index, item) {
                $("#drpDealer").append('<option value="' + item.WORKSHOP_CD + '"> ' + item.WORKSHOP_NAME +' ('+item.WORKSHOP_CD +')</option>');
            });
            //var ss = response;
            //alert(response)

            //var i;
            //for (i = 0; i < response.dealers.items.length; i++) {
            //    $("#drpDealer").append('<option value="' + response.dealers.items[i].WORKSHOP_CD + '"> ' + response.dealers.items[i].WORKSHOP_NAME + '</option>');
            //}
        },
        error: function (response) {
        }
    });

}


function fn_downloadconsolidatereport()
{
    $('#errmsg').text('');
    var isvalid = true;
    var objdatamodel = {};
    objdatamodel.Fromdate = $('#from2').val();
    objdatamodel.Todate = $('#to2').val();

    if (objdatamodel.Fromdate == "")
    {
        $('#errmsg').text('Please select from date.');
        isvalid = false;
        return true;
    }

    if (objdatamodel.Todate == "") {
        $('#errmsg').text('Please select to date.')
        isvalid = false;
        return true;
    }

    if (isvalid) {
        $.ajax({
            type: "POST",
            url: domainname + "admin/DownloadReport",
            data: JSON.stringify(objdatamodel),
            contentType: "application/json; charset=utf-8",
            async: "true",
            beforeSend: function () {
                // Show image container
                //$("#loader").show();
                $("#btnconsolidate").text('Please wait...');
            },

            success: function (response) {
                if (response) {
                    var mytime = new Date().getTime();
                    var blob = new Blob([response], { type: 'application/ms-excel' });
                    var downloadUrl = URL.createObjectURL(blob);
                    var a = document.createElement("a");
                    a.href = downloadUrl;
                    a.download = "DTSMP Usage Report" + mytime + ".xls";
                    document.body.appendChild(a); a.click();
                }
                else {
                    alert("We have no data found. Please choose another filter data.")
                }


            },
            complete: function (data) {

                $("#btnconsolidate").text('Download Report');
                // Hide image container
                //$("#loader").hide();
            },
            error: function (response) {
            }
        });
    }

}

function fn_downloadusagereport()
{
    var isvalidate = true;
    region = $("#REGION_CD option:selected").val();
    dealer = $("#drpDealer option:selected").val();
    
    if (region == '') {
        $('#errre').show();
        isvalidate = false;
        return true;
    } else {
        $('#errre').hide();
        isvalidate = true;
    }

    if (dealer == '') {
        $('#errd').show();
        isvalidate = false;
        return true;
    } else {
        $('#errd').hide();
        isvalidate = true;
    }

    if (isvalidate) {
        var objdatamodel = {};
        objdatamodel.DealerName = $("#drpDealer option:selected").text();
        objdatamodel.DealerCode = $("#drpDealer option:selected").val();
        objdatamodel.Fromdate = $('#from').val();
        objdatamodel.Todate = $('#to').val();
        $.ajax({
            type: "POST",
            url: domainname + "admin/DownloadExcel",
            data: JSON.stringify(objdatamodel),
            contentType: "application/json; charset=utf-8",
            async: "true",
            success: function (response) {
                if (response) {
                    var mytime = new Date().getTime();
                    var blob = new Blob([response], { type: 'application/ms-excel' });
                    var downloadUrl = URL.createObjectURL(blob);
                    var a = document.createElement("a");
                    a.href = downloadUrl;
                    a.download = "Service Manual Usage Report"+ mytime +".xls";
                    document.body.appendChild(a); a.click();
                }
                else {
                    alert("We have no data found. Please choose another dealer or filter data.")
                }

                //$.each(response, function (index, item) {
                //    $("#drpDealer").append('<option value="' + item.WORKSHOP_CD + '"> ' + item.WORKSHOP_NAME + '</option>');
                //});
                //var ss = response;
                //alert(response)

                //var i;
                //for (i = 0; i < response.dealers.items.length; i++) {
                //    $("#drpDealer").append('<option value="' + response.dealers.items[i].WORKSHOP_CD + '"> ' + response.dealers.items[i].WORKSHOP_NAME + '</option>');
                //}
            },
            error: function (response) {
            }
        });
    }
}

