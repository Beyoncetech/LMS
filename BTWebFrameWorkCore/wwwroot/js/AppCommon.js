function ShowAppLoader() {
    $('#CusAppLoader').show();
}
function HideAppLoader() {
    $('#CusAppLoader').hide();
}

function ConfirmPopupForSubmit(formId, ConfirmMsg, beforeSubmit, onSuccess, onError) {
    debugger;
    var frmID = "#" + formId;
    var isFormValid = $(frmID).valid();
    if (isFormValid) {
        ezBSAlert({
            type: "confirm",
            messageText: ConfirmMsg,
            alertType: "info"
        }).done(function (e) {
            if (e) {
                if (beforeSubmit !== undefined) {
                    beforeSubmit();
                }
                debugger;
                //$("#" + formId).submit();   
                var frmUrl = $(frmID).attr('action');
                var frmMethod = $(frmID).attr('method');
                var frmModel = $(frmID).serialize();
                $.ajax({
                    url: frmUrl,
                    type: frmMethod,
                    headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                    dataType: 'json',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: frmModel,
                    success: function (result) {
                        if (onSuccess !== undefined) {
                            onSuccess(result);
                        }
                    },
                    error: function (result) {
                        if (onError !== undefined) {
                            onError(result);
                        }
                    }
                });
            }
        });
    }
    else {
        return false;
    }
}

function AjaxSubmit(formId, beforeSubmit, onSuccess, onError) {
    var frmID = "#" + formId;
    var isFormValid = $(frmID).valid();
    if (isFormValid) {
        if (beforeSubmit !== undefined) {
            beforeSubmit();
        }
        debugger;
        //$("#" + formId).submit();   
        var frmUrl = $(frmID).attr('action');
        var frmMethod = $(frmID).attr('method');
        var frmModel = $(frmID).serialize();
        $.ajax({
            url: frmUrl,
            type: frmMethod,
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: frmModel,
            success: function (result) {
                if (onSuccess !== undefined) {
                    onSuccess(result);
                }
            },
            error: function (result) {
                if (onError !== undefined) {
                    onError(result);
                }
            }
        });
    }
    else {
        return false;
    }
}

function AppCommonAjaxPost(actionUrl, dataModel, onSuccess, onError) {
    
    $.ajax({
        url: actionUrl,
        type: 'POST',        
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: dataModel,
        success: function (result) {
            if (onSuccess !== undefined) {
                onSuccess(result);
            }
        },
        error: function (result) {
            if (onError !== undefined) {
                onError(result);
            }
        }
    });
}

function AppCommonAjaxPostPartial(actionUrl, dataModel, onSuccess, onError) {
    $.ajax({
        url: actionUrl,
        type: 'POST',
        dataType: 'html',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: dataModel,
        success: function (result) {
            if (onSuccess !== undefined) {
                onSuccess(result);
            }
        },
        error: function (result) {
            if (onError !== undefined) {
                onError(result);
            }
        }
    });
}