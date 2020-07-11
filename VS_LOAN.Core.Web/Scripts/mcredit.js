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
function GetLoanCities(controlId, defaultValue = 0) {
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