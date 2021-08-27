$('.radioButtonBx input[type="radio"]').click(function () {
    $('#errmsg').text("");
    $('#susmsg').text("");
    $('#otpmsg').hide();
    var radioId = $(this).data('id');
    $('.commonDiv').removeClass('active')
    $('#' + radioId + '').addClass('active');
})

var domainname = $('#domainname').val();




$('#sendotp').on("click", function () {

    $('#otpmsg').show();
    var objmodel = {};
    objmodel.PN_MSPIN = $('.mspinField').val();
    if (objmodel.PN_MSPIN == undefined || objmodel.PN_MSPIN == "") {
        $('#errmsg').text("MSPIN required.");
    }
    else
    {
        $.ajax({
            url: domainname + "/home/sendotp",
            data: JSON.stringify(objmodel),
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            success: function (response) {
                if (response != "202") {
                    $('#hidotp').val(response);
                    $('#susmsg').text("OTP sent successfully.");
                    $('#errmsg').text("");
                   

                }
                else {
                    $('#errmsg').text("Invalid MSPIN.");
                    $('#susmsg').text("");
                }
            },
            failure: function (response) {
                $('#errmsg').text("Invalid MSPIN.");
                $('#susmsg').text("");
            },
            error: function (response) {
                $('#errmsg').text("Invalid MSPIN.");
                $('#susmsg').text("");
            }

        });

    }
});

$(".mspinField,.otpField ").keypress(function (e) {
    if (e.which == 13) {
        mspinlogin();
    }
});


$('#btnmspinlogin').on("click", function () {
    mspinlogin();
});


function mspinlogin()
{
    $('#susmsg').text("");
    var Userlogin = {};
    Userlogin.EmailID = $('.mspinField').val();
    Userlogin.Password = $('.otpField').val();
    Userlogin.UserType = "M";
    if (Userlogin.EmailID == "") {
        $('#errmsg').text("MSPIN required.");
    }

    else if (Userlogin.Password == "") {
        $('#errmsg').text("OTP required.");
    }
    else {
        $.ajax({
            type: "POST",
            url: domainname + "/home/Index",
            data: JSON.stringify(Userlogin),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.toLowerCase().indexOf('index') > 0) {
                    window.location.href = domainname + response;
                }
                else {
                    $('#errmsg').text(response);
                   
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }

}


$(".loginField,.passField ").keypress(function (e) {
    if (e.which == 13) {
        msilloginfn();
    }
});







$('#btnmsillogin').on("click", function () {
    msilloginfn();
});


function msilloginfn()
{
    var Userlogin = {};
    Userlogin.EmailID = $('#EmailID').val();
    Userlogin.Password = $('#Password').val();
    Userlogin.UserType = "D";
    if (Userlogin.EmailID == "") {
        $('#errmsg').text("Username required.");
    }

    else if (Userlogin.Password == "") {
        $('#errmsg').text("Password required.");
    }
    else {
        $.ajax({
            type: "POST",
            url: domainname + "/home/Index",
            data: JSON.stringify(Userlogin),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.toLowerCase().indexOf('index') > 0) {
                    window.location.href = domainname + response;
                }
                else {
                    $('#errmsg').text(response);
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }

}