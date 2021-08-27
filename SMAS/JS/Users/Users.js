function fn_AddMangeUser() {
    var UserId = "";
    if (UserId == "") {
        UserId = 0;
    }

    var requestData = {};
    requestData.UserId = $('#'+pagevariable.UserId).val()==""? 0 : $('#'+pagevariable.UserId).val();
    requestData.UserName = $('#'+pagevariable.UserName).val();
    requestData.UserFullName = $('#'+pagevariable.UserFullName).val();
    requestData.UserEmailAddress = $('#'+pagevariable.UserEmailAddress).val();
    requestData.UserPassword = $('#'+pagevariable.UserPassword).val();
    
    var strValidationMsglists = "";

    if (requestData.UserName == "") {
        strValidationMsglists = strValidationMsglists + "<li>User Name Required.</li>";
    }
    if (requestData.UserFullName == "") {
        strValidationMsglists = strValidationMsglists + "<li>Full Name Required.</li>";
    }

    if (requestData.UserEmailAddress == "") {
        strValidationMsglists = strValidationMsglists + "<li>Email Address Required.</li>";
    }

    if (requestData.UserEmailAddress != "") {
        if (fn_IsValidEmailAddress_common(requestData.UserEmailAddress) == false) {
            strValidationMsglists = strValidationMsglists + "<li>please enter email id in valid format</li>";

        }
    }


    if (requestData.UserPassword == "") {
        strValidationMsglists = strValidationMsglists + "<li>User Password Required.</li>";
    }
    if (requestData.UserPassword != "") {
        if (requestData.UserPassword.length < 6) {
            strValidationMsglists = strValidationMsglists + "<li>Please enter minimum six characters Password.</li>";
        }
    }

    if (requestData.UserPassword != $('#'+pagevariable.ConfirmPassword).val()) {
        strValidationMsglists = strValidationMsglists + "<li>User Password and confirm password not match.</li>";
    }


    if ((requestData.UserName != "") & (requestData.UserPassword != "")) {
        if (requestData.UserName == requestData.UserPassword) {
            strValidationMsglists = strValidationMsglists + "<li>User name and Password should not be same.</li>";
        }
    }

    if (strValidationMsglists != "") {
        strValidationMsglists = "<ul>" + strValidationMsglists + "</ul>"
        //fn_ShowHideUserMessageDivWithCssClass_common('divDisplayUsermessages', strValidationMsglists, true, 'message error'); //common function to display error message.
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', strValidationMsglists, true, true);
        return false;
    }


    var controllerRequest = new ControllerRequest("POST", "json", pagevariable.requesturl, LoadResponseCallManagerole, jQuery.toDictionary(requestData)); //, pagevariable.pagecounter
    controllerRequest.makeRequestWithoutLoadingPanel();
}

function LoadResponseCallManagerole(msg) {
    

    if (msg.message == 'inserted') {
        $("#hdnUserId").val(msg.id);
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, false);

        setTimeout(function () {
            location.href = pagevariable.returnurl;//ManageUsers/ManageUsers";
        }, 1000); // Delay for 2 sec


    }
    else if (msg.message == 'UserNameexist') {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, true);
    }
    else if (msg.message == 'emailexist') {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, true);
    }
    else if (msg.message == 'updated') {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, false);
        setTimeout(function () {
            location.href = pagevariable.returnurl;
        }, 1000); // Delay for 2 sec
    }
    else {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, true);
    }

}



//*** end manage role ***//

//*** start manageusers ***//
function loadUserListing() {
    UserPage(1);
}
function fn_ViewAll() {
    $("#txtSearch").val('');
    loadUserListing();
}
function UserPage(index) {
    
    $(document).ajaxStart(function () {
        $('#loading').show();
    })
    $(document).ajaxComplete(function () {
        $('#loading').hide();
    })
    $(document).ajaxError(function () {
        $('#loading').hide();
    });

    if (index) {
        PageNumber = index;
    }
    else if ($("#PageNo").length > 0) {
        PageNumber = $("#PageNo").val();
    }

    var requestData = {};
    requestData.UserId = $('#' + pagevariable.UserId).val();
    requestData.ContactId = null;
    requestData.UserTypeId = $('#' + pagevariable.UserTypeId).val();
    requestData.SearchText = $('#' + pagevariable.SearchText).val();
    requestData.PageNumber = PageNumber;//index;
    requestData.Pagesize = 10;// $("#ddlPageSize :selected").val();
    
    requestData.SortBy = $('#' + pagevariable.SortBy1).val() + "_" + $('#' + pagevariable.SortBy2).val();

    var controllerRequest = new ControllerRequest("GET", "html", pagevariable.requesturl, LoadResponsePartialPageCall, jQuery.toDictionary(requestData)); //, pagevariable.pagecounter
    controllerRequest.makeRequestWithoutLoadingPanel();
}

function LoadResponsePartialPageCall(msg) {

    $('#divUserListingPartial').html(msg);
    objdivhtml = $('#divUserListingPartial');//$('#divUserListingPartial');

        objdivhtml.find(".clsImgSortUpDownArrowShowHide").removeAttr("class");
        objdivhtml.find(".clsImgSortUpDownArrowShowHide").attr("class", "clsImgSortUpDownArrowShowHide");
        objdivhtml.find(".clsImgSortUpDownArrowShowHide").removeClass("asc").removeClass("desc");

        if ($("#hdnSortColumnOrder").val() == "asc") {
            objdivhtml.find("#sortLink_EmployeeDetailsList_" + $("#hdnSortColumnName").val()).addClass("asc");

        }
        else {
            objdivhtml.find("#sortLink_EmployeeDetailsList_" + $("#hdnSortColumnName").val()).addClass("desc");
        }
        $("#divMainPagePaginationHtml").html(objdivhtml.find("#divMainPagePaginationHtml").html());
        if (objdivhtml == '#divUserListingPartial') { ImgOrderHideShow($("#hdnSortColumnName").val() + "_" + $("#hdnSortColumnOrder").val()); }
       
        objdivhtml.find("#divMainPagePaginationHtml").html('');

}

function fn_EditEmployee(UserId) {
    
    fn_ShowHideUserMessageDivWithCssClass_common('divDisplayUsermessages', "", false, 'message error');
    $("#savediv").hide();
    $("#UpdateDiv").show();
    $("#CancelDiv").show();
    if (UserId != null) {
        var requestData = {};
        requestData.UserId = UserId;// $('#' + pagevariable.UserId).val();

        var controllerRequest = new ControllerRequest("GET", "json", pagevariable.editemployeeurl, LoadResponsePartialPageEditEmployee, jQuery.toDictionary(requestData)); //, pagevariable.pagecounter
        controllerRequest.makeRequestWithoutLoadingPanel();
       
    }
}
function fn_DeleteEmployee(UserId) 
{
    var promptdelete = confirm('Are you sure to delete this record?');
    if (promptdelete) {
        var requestData = {};
        requestData.UserId = UserId;

        var controllerRequest = new ControllerRequest("GET", "json", pagevariable.deletemployeeurl, LoadResponsePartialPageEditEmployee, jQuery.toDictionary(requestData)); //, pagevariable.pagecounter
        controllerRequest.makeRequestWithoutLoadingPanel();
    }
    return true;
}

function LoadResponsePartialPageEditEmployee(msg) {
    if(msg.message == 'deleted'){
        loadEmployeesListing();
        alert(msg.messagedata);
    }
  else{
        $("#" + UserId).val(msg.UserId);
        $("#"+UserName).val(msg.UserName).attr("disabled", "disabled");;
        $('#'+UserEmailAddress).val(msg.EmailAddress);
        $('#'+UserPassword).val(msg.UserPassword);
        $('#'+ConfirmPassword).val(msg.UserConfirmPassword);
        $('#'+EmployeeName).val(msg.EmployeeName);
    }
}

function fn_MasterChangeStatus(StatusId, Status, varObj) {
    
    pagevariable.varObj = varObj;
    pagevariable.statusroot = 'masterChangeStatus';
    var requestData = {};
    requestData.StatusId = StatusId;   
    requestData.IsActive = Status;
    var promptdelete = confirm('Are you sure to change status of record?');
    if (promptdelete) {

        var controllerRequest = new ControllerRequest("POST", "json", pagevariable.changestatusurl, HandleResponsePartialPageCall, jQuery.toDictionary(requestData)); //, pagevariable.pagecounter
        controllerRequest.makeRequestWithoutLoadingPanel();
    }
}

function HandleResponsePartialPageCall(response) {
    if (pagevariable.statusroot == "masterChangeStatus") {
        if (response.message == 'statuschanged') {
            if ($(pagevariable.varObj).find('i').hasClass('active-icon')) {
                $(pagevariable.varObj).find('i').addClass('inactive-icon').removeClass('active-icon');
            }
            else {
                $(pagevariable.varObj).find('i').addClass('active-icon').removeClass('inactive-icon');
            }

        }
        else if (response.message == 'error') {
            alert(response.messagedata);
        }
        else {
            alert(response.messagedata);
        }
    }
}
//*** end manageusers ***//

//*** start usercustomer ***//
function GetCustomerList(index) {
    if (index) {
        PageNumber = index;
    }
    else if ($("#PageNo").length > 0) {
        PageNumber = $("#PageNo").val();
    }

    pagevariable.listof = 'UserCustomer';
    pagevariable.index=index;
    var requestData = {};
    requestData.UserId = $("#UserId :selected").val();
    requestData.ContactId = null;
    requestData.SearchText = $('#' + pagevariable.SearchText).val();
    requestData.PageNumber = PageNumber;
    requestData.Pagesize = $('#' + pagevariable.Pagesize).val();
    requestData.SortBy = $('#' + pagevariable.SortBy1).val() + "_" + $('#' + pagevariable.SortBy2).val();

    var controllerRequest = new ControllerRequest("GET", "html", pagevariable.requesturl, LoadResponsePartialPageCall, jQuery.toDictionary(requestData)); 
    controllerRequest.makeRequestWithoutLoadingPanel();
}

function GetCustomerDetails() {
    var requestData = {};    
    requestData.ContactId = $('#' + pagevariable.ContactId).val();   
    requestData.SearchText ='';
    requestData.PageNumber = 1;
    requestData.Pagesize = null;
    requestData.SortBy = null;

    var controllerRequest = new ControllerRequest("GET", "html", pagevariable.basicdetailsurl, LoadResponsePartialPageCallCustomer, jQuery.toDictionary(requestData)); //, pagevariable.pagecounter
    controllerRequest.makeRequestWithoutLoadingPanel();   
}

function LoadResponsePartialPageCallCustomer(msg) {
    if (msg.message == 'success') {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, false);
    }
    else if (msg.message != 'success') {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, true);
    }
    else if (pagevariable.usercutomer = '1') {
        var id = msg.toLowerCase();
    }
    else {
        $('#' + pagevariable.responseBasicContactContent).html(msg);
        $("#chk-" + ContactId).prop("checked", true);
        $("#chk-" + ContactId).closest("li").addClass('selected');
    }
}


function fn_GetUserCustomer(UserId) {
    if (UserId == "") {
        alert('Please select User Properly.');
        return
    }
    if (UserId != '') {
        pagevariable.usercutomer = '1';
        var requestData = {};
        requestData.UserId = UserId;
        var controllerRequest = new ControllerRequest("GET", "html", pagevariable.usercustomerurl, LoadResponsePartialPageCallCustomer, jQuery.toDictionary(requestData)); 
        controllerRequest.makeRequestWithoutLoadingPanel();        
    }

}

function fn_SaveUserCustomer() {
    var UserId = $("#UserId :selected").val();
    var checkids = "";
    var uncheckids = "";

    try {
        $('.chk-col :checkbox:checked').each(function () {
            if ($(this).attr("isActive") == "True") {
                if (checkids == "") {
                    checkids = $(this).attr("contact");
                }
                else {
                    checkids += ',' + $(this).attr("contact");
                }

            }

        });

        $('.chk-col :checkbox:not(:checked)').each(function () {
            if ($(this).attr("isActive") == "True") {

                if (uncheckids == "") {
                    uncheckids = $(this).attr("contact");
                }
                else {
                    uncheckids += ',' + $(this).attr("contact");
                }

            }

        });

    }
    catch (err) {

    }
    if (UserId == "") {
        alert("Please select the User Properly.");
        return false;
    }
    if (UserId != '') {

        var requestData = {};
        requestData.UserId = UserId;
        requestData.checkids = checkids;
        requestData.uncheckids = uncheckids;

        var controllerRequest = new ControllerRequest("POST", "json", pagevariable.updatecustomerurl, LoadResponsePartialPageCallCustomer, jQuery.toDictionary(requestData)); //, pagevariable.pagecounter
        controllerRequest.makeRequestWithoutLoadingPanel();       
    }

}
//*** end usercustomer ***//

//*** start role listing  ***//

function fn_AddRoleName() {

    var UserTypeId = "";
    var UserTypeName = "";
    UserTypeId = $("#" + pagevariable.UserTypeId).val();
    UserTypeName = $("#" + pagevariable.UserTypeName).val();
    var IsAccountManager = $('#chkIsAccountManager').is(":checked");

    var strValidationMsglists = "";

    if ($('#'+pagevariable.UserTypeName).val() == "") {
        strValidationMsglists = strValidationMsglists + "<li>Please enter Role Name</li>";
    }


    if (strValidationMsglists != "") {
        strValidationMsglists = "<ul>" + strValidationMsglists + "</ul>"
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', strValidationMsglists, true, true);
        return false;
    }


    if (UserTypeId == "") {
        UserTypeId = 0;
    }

    var requestData = {};
    requestData.UserTypeId = UserTypeId;
    requestData.UserTypeName = UserTypeName;
    requestData.IsAccountManager = $('#' + pagevariable.IsAccountManager).is(":checked");

    if (UserTypeName != "") {
        pagevariable.addrolename='';
        var controllerRequest = new ControllerRequest("POST", "json", pagevariable.addroleurl, LoadResponseCallManageAddRoleName, jQuery.toDictionary(requestData)); //, pagevariable.pagecounter
        controllerRequest.makeRequestWithoutLoadingPanel();
    }
}

function LoadResponseCallManageAddRoleName(msg) {
    if (msg.message == 'inserted') {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, false);        
        fn_ResetForm();
        loadRoleListing();

    }
    else if (msg.message == 'updated') {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, false);
        fn_ResetForm();
        loadRoleListing();


    }
    else if (msg.message == 'exists') {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, true);
    }

    else {
        fn_ShowHideUserMessageDivWithCssClass_commonNew('errormsg', msg.messagedata, true, true);
    }
}
//*** end role listing ***//