function GetLoanCities(controlId, defaultValue = 0) {
    $(controlId).empty();
    $.ajax({
        type: "GET",
        url: '/MCredit/GetMCSimpleList?type=1',
        data: {},
        success: function (data) {
            $(controlId).append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
                });
                if (defaultValue > 0) {
                    $(controlId).val(defaultValue);
                }

                $(controlId).chosen().trigger("chosen:updated");
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetLoanPeriods(controlId, defaultValue = 0) {
    $(controlId).empty();
    $.ajax({
        type: "GET",
        url: '/MCredit/GetMCSimpleList?type=2',
        data: {},
        success: function (data) {
            $(controlId).append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
                });
                if (defaultValue > 0) {
                    $(controlId).val(defaultValue);
                }

                $(controlId).chosen().trigger("chosen:updated");
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetLocSigns(controlId, defaultValue = 0) {
    $(controlId).empty();
    $.ajax({
        type: "GET",
        url: '/MCredit/GetMCSimpleList?type=3',
        data: {},
        success: function (data) {
            $(controlId).append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
                });
                if (defaultValue > 0) {
                    $(controlId).val(defaultValue);
                }

                $(controlId).chosen().trigger("chosen:updated");
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetLoanProducts(controlId, defaultValue = 0) {
    $(controlId).empty();
    $.ajax({
        type: "GET",
        url: '/MCredit/GetMCSimpleList?type=4',
        data: {},
        success: function (data) {
            $(controlId).append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
                });
                if (defaultValue > 0) {
                    $(controlId).val(defaultValue);
                }

                $(controlId).chosen().trigger("chosen:updated");
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function getCICMessage(data) {
    if (data == null || data == undefined)
        return ''
    if (data.status == "error")
        return data.msg
    return data.Result
}
function checkDup(controlId, value) {
        var objectSend = JSON.stringify({
            'Value': value
        });
        $.ajax({
            traditional: true,
            url: '/MCredit/CheckDupApi',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: objectSend,
            success: function (data) {
                if (data.success == true) {
                    if (data.data.status == "success") {
                        document.getElementById(controlId).innerHTML = `${data.data.result} - ${data.data.desc}`;
                        swal({
                            title: "Thành công",
                            text: `${data.data.result} - ${data.data.desc}`,
                            type: "success",
                            timer: 4000,
                            showConfirmButton: true,
                        }, function () {

                        });
                    }
                    else {
                        document.getElementById(controlId).innerHTML = data.code;
                        swal({
                            title: "Đã có lỗi xảy ra",
                            text: data.code,
                            type: "error",
                            timer: 4000,
                            showConfirmButton: true,
                        });
                    }
                    
                }
                else {
                    swal({
                        title: "Đã có lỗi xảy ra",
                        text: data.code,
                        type: "error",
                        timer: 4000,
                        showConfirmButton: true,
                    });
                }
            },
            error: function (jqXHR, exception) {
                showError(jqXHR, exception);
            },
            complete: function () {
                $('#panel_body').unblock();
            },
        });
    

}
function checkCIC(controlId, value) {
    if (isNullOrWhiteSpace(value))
        return;
        var objectSend = JSON.stringify({
            'Value': value
        });
        $.ajax({
            traditional: true,
            url: '/MCredit/CheckCICApi',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: objectSend,
            success: function (data) {
                if (data.success == true) {
                    swal({
                        title: "Thành công",
                        text: `${getCICMessage(data.data)}`,
                        type: "success",
                        timer: 4000,
                        showConfirmButton: true,
                    }, function () {

                    });
                }
                else {
                    swal({
                        title: "Đã có lỗi xảy ra",
                        text: data.code,
                        type: "error",
                        timer: 4000,
                        showConfirmButton: true,
                    });
                }
            },
            error: function (jqXHR, exception) {
                showError(jqXHR, exception);
            },
            complete: function () {
                $('#panel_body').unblock();
            },
        });
   

}
function checkCAT(controlId, value) {
   
        var objectSend = JSON.stringify({
            'Value': value
        });
        $.ajax({
            traditional: true,
            url: '/MCredit/CheckCatApi',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: objectSend,
            success: function (data) {
                if (data.success == true) {
                    swal({
                        title: "Thành công",
                        text: data.code,
                        type: "success",
                        timer: 4000,
                        showConfirmButton: true,
                    }, function () {

                    });
                }
                else {
                    swal({
                        title: "Đã có lỗi xảy ra",
                        text: data.code,
                        type: "error",
                        timer: 4000,
                        showConfirmButton: true,
                    });
                }
            },
            error: function (jqXHR, exception) {
                showError(jqXHR, exception);
            },
            complete: function () {
                $('#panel_body').unblock();
            },
        });
}
function checkSale(controlId, value) {

    var objectSend = JSON.stringify({
        'Value': value
    });
    $.ajax({
        traditional: true,
        url: '/MCredit/CheckSaleApi',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: objectSend,
        success: function (data) {
            if (data.success == true) {
                if (data.data.status == "success") {
                    swal({
                        title: "Thành công",
                        text: data.code,
                        type: "success",
                        timer: 4000,
                        showConfirmButton: true,
                    }, function () {

                    });
                }
                else {
                    swal({
                        title: "Không thành công",
                        text: data.code,
                        type: "error",
                        timer: 4000,
                        showConfirmButton: true,
                    }, function () {

                    });
                }
                document.getElementById(controlId).innerHTML = data.code;
            }
            else {
                swal({
                    title: "Đã có lỗi xảy ra",
                    text: data.code,
                    type: "error",
                    timer: 4000,
                    showConfirmButton: true,
                });
            }
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        },
        complete: function () {
            $('#panel_body').unblock();
        },
    });
}