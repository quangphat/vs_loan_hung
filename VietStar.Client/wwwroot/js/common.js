
function setLocalStorage(key, data) {
    window.localStorage.removeItem(key);
    window.localStorage.setItem(key, JSON.stringify(data));
}
function getLocalStorage(key) {
    return JSON.parse(window.localStorage.getItem(key))
}
function getComments(profileId, profileType, commentDisplayControl) {
    if (isNullOrUndefined(profileType) || isNullOrUndefined(profileId))
        return

    $.ajax({
        type: "GET",
        url: `/comment/${profileId}/${profileType}`,
        success: function (data) {

            if (data.data != null && data.success == true) {
                commentDisplayControl.empty();
                $.each(data.data, function (index, item) {
                    appendComment(commentDisplayControl, item.Content, item.Commentator, item.CommentTime)
                });
            }
        },
        complete: function () {

        },
        error: function (jqXHR, exception) {
            //showError(jqXHR, exception);
        }
    });

}
function appendComment(control, content, commenttator, commenttime) {
    if (isNullOrWhiteSpace(content))
        return

    control.append(
        '<div class="timeline-event-content  active">' +
        '<div class="timeline-item">' +
        '<div class="timeline-body">' +
        '<div class="timeline__message-container">' +
        '<strong>' + commenttator + ' (' + commenttime + '): </strong><span>' + content + '</span>' +
        '</div></div></div></div>'

    )
}
function AddNote(profileId, profileType, content, commentBox, commentDisplayControl) {
    if (isNullOrWhiteSpace(profileType) || isNullOrWhiteSpace(content))
        return

    let data = JSON.stringify({
        'ProfileId': profileId,
        'ProfileTypeId': profileType,
        'Content': content
    })
    $.ajax({
        type: "POST",
        url: `/comment`,
        data: data,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success == true) {
                swal({
                    title: "",
                    text: "Thành công",
                    type: "success",
                    timer: 4000,
                    showConfirmButton: true,
                });
                commentBox.val('')
                getComments(profileId, profileType, commentDisplayControl)
            }
            else {
                swal({
                    title: "",
                    text: data.error.code,
                    type: "error",
                    timer: 4000,
                    showConfirmButton: true,
                });
            }
        },
        complete: function () {

        },
        error: function (jqXHR, exception) {
            //showError(jqXHR, exception);
        }
    });

}
function renderStatusList(profileType, value = null) {
    if (isNullOrWhiteSpace(profileType))
        return
    let control = $('#ddlStatus')
    control.empty();
    let data = JSON.parse(window.localStorage.getItem('profile_statuses'));
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (value != null) {
            control.val(value);
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: `/Common/GetStatusList/${profileType}`,
        data: {},
        success: function (data) {
            $('#ddlStatus').append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                setLocalStorage('profile_statuses', data.data)
                $.each(data.data, function (index, item) {
                    $('#ddlStatus').append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (value != null) {
                    //let uniqueSet = new Set(value);
                    //let backToArray = [...uniqueSet]
                    $("#ddlStatus").val(value);
                }

            }
        },
        complete: function () {

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
function GetSales(control = null, defaultValue = 0) {
    if (control == null)
        control = $("#saleId");
    control.empty();
    control.append("<option value='0'>Chọn sale</option>");
    let data = JSON.parse(window.localStorage.getItem('salesbyuser'));
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null) {
            control.val(defaultValue);
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: '/common/sales',
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn sale</option>");
            if (data.data != null && data.success == true) {
                setLocalStorage('salesbyuser', data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetCouriers(control = null, defaultValue = 0) {
    if (control == null)
        control = $("#courierId");
    control.empty();
    control.append("<option value='0'>Chọn courier</option>");
    let data = JSON.parse(window.localStorage.getItem('couriersbyuser'));
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null) {
            control.val(defaultValue);
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: '/common/couriers',
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn courier</option>");
            if (data.data != null && data.success == true) {
                setLocalStorage('couriersbyuser', data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetPartners(control = null, defaultValue = 0, productId = 0) {
    if (control == null)
        control = $("#partnerId");
    control.empty();
    control.append("<option value='0'>Chọn tỉnh/thành</option>");
    let data = JSON.parse(window.localStorage.getItem('partners'));
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null) {
            control.val(defaultValue);
            GetProducts(defaultValue, null, productId)
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: '/common/partners',
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn đối tác</option>");
            if (data.data != null && data.success == true) {
                setLocalStorage('partners', data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    GetProducts(defaultValue, null, productId)
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetProducts(partnerId, control = null, defaultValue = 0) {
    if (control == null)
        control = $("#productId");
    control.empty();
    $.ajax({
        type: "GET",
        url: `/common/products/${partnerId}`,
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn sản phẩm</option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetProvinces(control = null, defaultValue = 0, districId = 0) {
    if (control == null)
        control = $("#provinceId");
    control.append("<option value='0'>Chọn tỉnh/thành</option>");
    let data = JSON.parse(window.localStorage.getItem('provinces'));
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null) {
            control.val(defaultValue);
            GetDistricts(defaultValue, null, districId)
        }
        return;
    }
    control.empty();
    $.ajax({
        type: "GET",
        url: '/common/provinces',
        data: {},
        success: function (data) {

            if (data.data != null && data.success == true) {
                window.localStorage.setItem('provinces', JSON.stringify(data.data))
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    GetDistricts(defaultValue, null, districId)
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetDistricts(provinceId, control = null, defaultValue = 0) {
    if (control == null)
        control = $("#districtId");
    control.empty();
    $.ajax({
        type: "GET",
        url: `/common/districts/${provinceId}`,
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn quận/huyện</option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
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
            control.append("<option value='0'>Chọn nhóm</option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "(" + item.ShortName + ")</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

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
    if (isNullOrUndefined(groupId) || isNullOrWhiteSpace(groupId))
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
                if (defaultValue != null) {
                    control.val(defaultValue);
                }

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
    let greenGroup = ['a', 'b', 'c', 'd', 'đ'];
    let danger = ['e', 'f', 'g', 't'];
    let succsess = ['i', 'k', 'm'];
    let cancel = ['o', 'p', 'q'];
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
function renderOneItemFile(model, className = '',
    _initialPreview = [],
    _initialPreviewConfig = [],
    allowUpload = true,
    continueUpload = true
) {

    let header = ''
    if (!isNullOrUndefined(model.titleName)) {
        if (model.isRequire) {
            header = '<h5  class="header green ' + className + '">' + model.titleName + '<span class="required">(*)</span></h5>';
        }
        else {
            header = '<h5  class="header green ' + className + '">' + model.titleName + '<span > </span></h5>';
        }
    }
    
    let guidId = isNullOrWhiteSpace(model.guidId) ? getNewGuid() : model.guidId;
    let content = header + "<div class='col-sm-3'> ";
    content += "<div class=\"file-loading\">";
    content += "<input class='attachFile' key=" + model.key + " id=\"attachFile-" + model.itemId + "\" type=\"file\">";
    content += "</div>";
    content += "</div>";
    $('#tailieu-' + model.key).append(content);
    
    let item = $("#attachFile-" + model.itemId);

    
    let uploadUrl = `/media/UploadFile/${model.key}/${model.type}/${model.profileId}/${model.fileId}/${guidId}`;
    item.fileinput({
        uploadUrl: allowUpload === true ? uploadUrl : null,
        validateInitialCount: true,
        maxFileSize: 25 * 1024,
        msgSizeTooLarge: 'File "{name}" (<b>{size} KB</b>)'
            + 'exceeds maximum allowed upload size of <b>{25} MB</b>. '
            + 'Please retry your upload!',
        allowedFileExtensions: ['png', 'jpg', 'pdf'],
        initialPreviewAsData: true, // identify if you are sending preview data only and not the raw markup
        initialPreviewFileType: 'image',
        overwriteInitial: false,
        showUploadedThumbs: false,
        uploadAsync: false,
        showClose: false,
        showCaption: false,
        showBrowse: false,
        showDownload: true,
        showUpload: false, // hide upload button
        showRemove: false, // hide remove button
        browseOnZoneClick: true,
        removeLabel: '',
        fileId: model.fileId,
        outsideGuidId: `'${guidId}'`,
        btnDeleteId: `btnRemoveFile-${guidId}`,
        dropZoneTitle: 'Kéo và thả tập tin vào đây',
        dropZoneClickTitle: '<br>(hoặc nhấp để chọn)',
        removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
        removeTitle: 'Cancel or reset changes',
        elErrorContainer: '#kv-avatar-errors-2',
        msgErrorClass: 'alert alert-block alert-danger',
        //layoutTemplates: { main2: '{preview} ' + btnCust + ' {remove} {browse}' },
        //layoutTemplates: { footer: '' },
        initialPreview: _initialPreview,
        initialPreviewDownloadUrl: _initialPreview,
        initialPreviewConfig: _initialPreviewConfig,
        fileActionSettings: {
            showDownload: true,
            showRemove: true,
            showUpload: true,
            showZoom: true,
            showDrag: false
        },
        append: true

    }).on("filebatchselected", function (event, files) {

        //if (countFilesByKey(filesUploaded, parseInt(model.key)) >= 50)
        //    return;
        $(item).fileinput("upload");
    }).on("filebeforedelete", function () {

        //if (onDelete !== null) {
        //    onDelete(key, fileId, isFileExist);
        //}
    }).on('filebatchuploadsuccess', function (event, data) {
        
        if (continueUpload === true && model.fileId<=0) {
            let newItem = { ...model }
            newItem.guidId = getNewGuid
            newItem.fileId = 0
            newItem.titleName =''
            renderOneItemFile({
                key: model.key,
                type: model.type,
                profileId: model.profileId,
                fileId: 0,
                isRequire: model.isRequire,
                titleName: model.titleName,
                guidId: ''
            }, '', [], [], true, continueUpload);
        }
    });

}
function isReach5Files(filesUpload, key) {
    if (isNullOrWhiteSpace(key))
        return true;
    if (isNullOrNoItem(filesUpload))
        return false;
    let sameKeyFile = filesUpload.find(p => p.key === key);
    if (isNullOrUndefined(sameKeyFile))
        return false;
    if (isNullOrNoItem(sameKeyFile.files))
        return 0;
    if (sameKeyFile.files.length === 50)
        return true;
    return false;
}
function countFilesByKey(filesUpload, key) {
    if (isNullOrWhiteSpace(key))
        return 0;
    let sameKeyFile = filesUpload.find(p => p.key === key);
    if (isNullOrUndefined(sameKeyFile))
        return 0;
    if (isNullOrNoItem(sameKeyFile.files))
        return 0;
    return sameKeyFile.files.length;
}
function onDeleteFile(fileId, guidId) {
    $.ajax({
        type: "POST",
        url: `/media/delete/${fileId}/${guidId}`,
        success: function (data) {
            
            if (data.data != null && data.success == true) {
                
            }

        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}