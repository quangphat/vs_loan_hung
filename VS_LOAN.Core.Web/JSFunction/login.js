$("#userName").focus();
$("#userName").keypress(function (e) {
    if (e.which == 13) {
        $("#password").focus();
    }
});
$("#password").keypress(function (e) {
    if (e.which == 13) {
        DoLogint(e);
    }
});

$('#btn_login').click(function (evt) {
    DoLogint(evt);
});
function addAlert(message) {
    $('#login_alert').empty();
    $('#login_alert').append(
            '<div class="alert alert-danger">' +
            '<button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button>' +
            '<strong><i class="ace-icon fa fa-exclamation-triangle bigger-120"></i></strong>' + message + '</div>');
}

function DoLogint(event) {
    if ($("#form_login").validate()) {
        showBlock($('#login-box'), strLoading);
        $.ajax({
            type: "POST",
            datatype: "JSON",
            url: '/Employee/DangNhap',
            data: $('#form_login').serialize(),
            success: function (data) {
                if (data.Message.Result == false) {
                    addAlert(data.Message.ErrorMessage);
                    $("#userName").focus();
                }
                else {
                    window.location = data.newurl;
                }
            },
            error: function (jqXHR, exception) {
                showError(jqXHR, exception);
                $('#login-box').unblock();
            },
            complete: function () {
                $('#login-box').unblock();
            },
        });

    }
}