
$(document).ready(function() {

// $(".leftPanelArrow").click(function(){
//   $(".leftPanel").toggleClass("active");
//   $(".rightPanel").toggleClass("active");
// });

  
$("#profileLink").click(function(){
	$("#profileLinkDrop").slideToggle();
});

$('.actionUpLink').click(function(){
    $('.uploadReplaceTabMain').toggleClass('active');
});
$(".rightSlideCrossBtn").click(function(){
  $(".uploadReplaceTabMain").removeClass("active");
});

$('.createVarialLink').click(function(){
    $('.createVariantTabMain').toggleClass('active');
});
$(".rightSlideCrossBtn").click(function(){
  $(".createVariantTabMain").removeClass("active");
});
});


var domainname = $('#domainname').val();


function fn_modelobsoletedata() {
    $("#obsolete").html('');
    $("#obsolete").toggleClass('active');
    $("#obsolete").append($('#running').html())
    $('#running').html('');
    $("#ROSTATUS").val("N");
    var ddlCustomers = $("#MID");
    var objmodel = {};
    objmodel.status = "N";
    $.ajax({
        type: "POST",
        url: domainname + "/admin/modellist",
        data: JSON.stringify(objmodel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            ddlCustomers.empty().append('<option selected="selected" value>Select Model</option>');
            $.each(response, function () {
                ddlCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}

function fn_modelrunningdata() {
    $("#running").html('');
    $("#running").toggleClass('active');
    $("#running").append($('#obsolete').html())
    $('#obsolete').html('');
    $("#ROSTATUS").val("Y");
    var ddlCustomers = $("#MID");

    var objmodel = {};
    objmodel.status = "Y";
    $.ajax({
        type: "POST",
        url: domainname + "/admin/modellist",
        data: JSON.stringify(objmodel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            ddlCustomers.empty().append('<option selected="selected" value>Select Model</option>');
            $.each(response, function () {
                ddlCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}




