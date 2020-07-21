$(document).ready(function e() {
    $('.select2').select2();
    //$('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
  
});
function isNullOrNoItem(arr) {
    if (arr === null || arr === undefined || arr.length === 0)
        return true;
    return false;
}
function isNullOrUndefined(value) {
    if (value === null || value === undefined)
        return true;
    return false;
}
function isNullOrWhiteSpace(text) {
    if (text === null || text === undefined || text === '' || text.toString().trim() === '')
        return true;
    return false;
}
function renderStatusList(value = [0]) {
    $("#ddlStatus").empty();
    $.ajax({
        type: "GET",
        url: '/Common/GetStatusList',
        data: {},
        success: function (data) {
            $('#ddlStatus').append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    $('#ddlStatus').append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });

            }
        },
        complete: function () {
            $("#ddlStatus").val([1, 2, 3]).change();
        },
        error: function (jqXHR, exception) {
            //showError(jqXHR, exception);
        }
    });


    //$.each(lstStatus, function (index, item) {
    //    $("#ddlStatus").append("<option value='" + item.value + "'>" + item.display + "</option>")
    //})
    //$('#ddlStatus').chosen().trigger("chosen:updated");
    //$('#ddlStatus').trigger("change");

}