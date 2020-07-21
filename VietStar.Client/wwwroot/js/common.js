$(document).ready(function e() {
    $('.select2').select2();
    //$('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
  
});
function renderStatusList() {
    $("#ddlStatus").empty();
    $.ajax({
        type: "POST",
        url: '@Url.Action("LayDSTrangThai", "QuanLyHoSo")',
        data: {},
        success: function (data) {
            $('#ddlStatus').append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    $('#ddlStatus').append("<option value='" + item.ID + "'>" + item.Ten + "</option>");
                });

                var status = decodeURI(queries.get("status"))
                if (!isNullOrWhiteSpace(status)) {
                    let arrStatus = status.split(',')
                    $('#ddlStatus').val(arrStatus).chosen().trigger("chosen:updated")
                }
                $('#ddlStatus').chosen().trigger("chosen:updated");
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });


    //$.each(lstStatus, function (index, item) {
    //    $("#ddlStatus").append("<option value='" + item.value + "'>" + item.display + "</option>")
    //})
    //$('#ddlStatus').chosen().trigger("chosen:updated");
    //$('#ddlStatus').trigger("change");

}