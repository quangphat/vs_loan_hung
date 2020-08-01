function renderStatusList(profileType, value = null) {
    if (isNullOrWhiteSpace(profileType))
        return
    $("#ddlStatus").empty();
    $.ajax({
        type: "GET",
        url: `/Common/GetStatusList/${profileType}`,
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
            if (value != null)
            $("#ddlStatus").val(value).change();
        },
        error: function (jqXHR, exception) {
            //showError(jqXHR, exception);
        }
    });

}
function renderStatus(MaTrangThai, TrangThaiHS) {
    var statusString = "";
    if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.Nhap)')
        statusString = "<span class='label label-sm label-light arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.NhapLieu)')
        statusString = "<span class='label label-sm label-purple arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.ThamDinh)')
        statusString = "<span class='label label-sm label-danger arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.DaDoiChieu)')
        statusString = "<span class='label label-sm label-dadoichieu arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.TuChoi)')
        statusString = "<span class='label label-sm label-inverse arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.Cancel)')
        statusString = "<span class='label label-sm label-cancel arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.PCB)')
        statusString = "<span class='label label-sm label-pcb arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.BoSungHoSo)')
        statusString = "<span class='label label-sm label-info arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.GiaiNgan)')
        statusString = "<span class='label label-sm label-success arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    else if (MaTrangThai == '@((int)VS_LOAN.Core.Entity.TrangThaiHoSo.Finish)')
        statusString = "<span class='label label-sm label-success arrowed arrowed-righ'>" + TrangThaiHS + "</span>";
    return "<td class='text-left'>" + statusString + "</td>";;
}
function GetGroupByUser(control = null, defaultValue = 0) {
    if (control == null)
        control = $("#groupId");
    control.empty();
    $.ajax({
        type: "GET",
        url: '/Groups/GetGroupsByUserId',
        data: {},
        success: function (data) {
            control.append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "(" + item.ShortName + ")</option>");
                });
               //control.chosen().trigger("chosen:updated").change();
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetMemberByGroup(groupId, control = null, defaultValue = 0) {
    if (isNullOrUndefined(groupId))
        return;
    if (control == null)
        control = $("#memberId");
    control.empty();
    $.ajax({
        type: "GET",
        url: `/Employees/GetByGroupId/${groupId}`,
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn thành viên</option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                //control.chosen().trigger("chosen:updated").change();
            }
           
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function renderStatusDisplay(statusName) {
    
    if (isNullOrWhiteSpace(statusName))
        return "<td class='text-left'></td>";
    let firstChar = statusName[0].toLowerCase();
    let greenGroup = ['a','b','c','d','đ'];
    let danger = ['e','f','g','t'];
    let succsess = ['i','k','m'];
    let cancel = ['o','p','q'];
    let inverse = ['j', 'z', 'w'];
    let colorClass = 'label-temp'
    if (greenGroup.indexOf(firstChar) >= 0)
        colorClass = 'label-green';
    if (danger.indexOf(firstChar) >= 0)
        colorClass = 'label-orrange'
    if (succsess.indexOf(firstChar) >= 0)
        colorClass = 'label-success'
    if (inverse.indexOf(firstChar) >= 0)
        colorClass = 'label-inverse'
    if (cancel.indexOf(firstChar) >= 0)
        colorClass = 'label-cancel'

    var statusString = `<span class='label label-sm ${colorClass} arrowed arrowed-righ'>${statusName}</span>`;
    return "<td class='text-left'>" + statusString + "</td>";
}