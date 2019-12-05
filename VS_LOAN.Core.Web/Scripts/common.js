﻿//function showBlock(div, text) {
//    div.block({
//        css: {
//            border: 'none',
//            padding: '15px',
//            backgroundColor: '#000',
//            '-webkit-border-radius': '10px',
//            '-moz-border-radius': '10px',
//            opacity: 1,
//            color: '#fff'
//        },
//        message: '<h2 style="color:#fff">' + text + ' ...</h2>'
//    });
//}
function renderOneItemFile(key, fileId, titleName, isRequire = false, className = '', generateInput = false,
    _initialPreview = [],
    _initialPreviewConfig = [],
    allowUpload = false,
    isFileExist = false,
    onUpload = null,
    onDelete = null,
    filesUploaded = []) {

    let content = "<div class='col-sm-3' style='width:20%'>";

    if (!isNullOrUndefined(titleName)) {
        if (isRequire) {
            content += '<h5  class="header green ' + className + '">' + titleName + '<span class="required">(*)</span></h5>';
        }
        else {
            content += '<h5  class="header green ' + className + '">' + titleName + '<span > </span></h5>';
        }
    }

    content += "<div class=\"file-loading\">";
    content += "<input class='attachFile' key=" + key + " id=\"attachFile-" + fileId + "\" type=\"file\">";
    content += "</div>";
    content += "</div>";
    $('#tailieu-' + key).append(content);
    if (generateInput === true) {
        let item = $("#attachFile-" + fileId);

        fileId = (isFileExist === true) ? fileId : getNewGuid();
        let uploadUrl = isFileExist === true ? '/Hoso/UploadFile?key=' + key + "&fileId=" + fileId : '/Hoso/UploadFile?key=' + key + "&fileId=0";
        $(item).fileinput({
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
            showUpload: false, // hide upload button
            showRemove: false, // hide remove button
            browseOnZoneClick: true,
            removeLabel: '',
            fileId: fileId,
            btnDeleteId: 'btn-remove-file-' + fileId,
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
            if (countFilesByKey(filesUploaded, parseInt(key)) >= 50)
                return;
            $(item).fileinput("upload");
        }).on("filebeforedelete", function (event, key2, fileId) {

            if (onDelete !== null) {
                onDelete(key, fileId, isFileExist);
            }
        }).on('filebatchuploadsuccess', function (event, data) {
            if (onUpload !== null) {
                onUpload(key, fileId, data.response, isFileExist);
            }
        });

    }

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
function getNewGuid() {
    const s4 = () => {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    };
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}
function setCheckedValueOfRadioButtonGroup(name, vValue) {
    var radios = document.getElementsByName(name);
    for (var j = 0; j < radios.length; j++) {
        if (radios[j].value === vValue) {
            radios[j].checked = true;
            break;
        }
    }
}
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

function showBlock(div, text) {
    div.block({
        css: {
            border: 'none',
            padding: '15px',
            //backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            //color: '#fff'
        },
        message: '<img src="/Content/images/busy.gif" />' + text
    });
}

function showBlockUI(text) {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            //backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            //color: '#fff'
        },
        message: '<img src="/Content/images/busy.gif" />' + text
    });
}


function addDefaultOption(selectList, objectText) {
    selectList.empty();
    selectList
        .append($("<option></option>")
                .attr("value", "-1")
                .text("Chọn " + objectText));
}

function addSelectListWithDefaultValue(selectList, results, defaultValue) {
    //debugger;
    if (results.length == 0) {
        selectList.html('<option value="-1">Không có dữ liệu</option>');
        return;
    }
    $.each(results, function (i, value) {
        var transformedResult = value.Text;
        var option = $("<option></option>").attr("value", value.Value).text(transformedResult);
        if (defaultValue != null && defaultValue == value.Value) {
            option.attr('selected', 'selected');
        }
        if (defaultValue == null) {

        }
        selectList.append(option);
    });
}

function addSelectListItems(selectList, results) {
    if (results.length == 0) {
        selectList.html('<option value="-1">Không có dữ liệu</option>');
        return;
    }
    if (results.length == 1) {
        selectList
                .append($("<option></option>")
                .attr("value", results[0].Value)
                .attr("selected", true)
                .text(results[0].Text));
        selectList.change();
        return;
    }
    $.each(results, function (i, value) {
        selectList
                .append($("<option></option>")
                .attr("value", value.Value)
                .text(value.Text));
    });
}

function shortDateFormat(intDate) {
    if (intDate != null && intDate != '')
        return kendo.toString(new Date(parseInt(intDate) + (new Date().getTimezoneOffset()) * 60 * 1000), "dd/MM/yyyy");
    return intDate;
}

function fullDateFormat(intDate) {
    if (intDate != null && intDate != '')
        return kendo.toString(new Date(parseInt(intDate) + (new Date().getTimezoneOffset()) * 60 * 1000), "dd/MM/yyyy HH:mm");
    return intDate;
}

function FormatString(alias) {
    var str = alias;
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ắ|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");
    /* tìm và thay thế các kí tự đặc biệt trong chuỗi sang kí tự - */
    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    str = str.replace(/^\-+|\-+$/g, ""); //cắt bỏ ký tự - ở đầu và cuối chuỗi 
    str = str.replace(/[^a-zA-Z0-9\-]/g, '');//halm 11/10/2014: cắt bỏ ký tự đặc biệt
    return str;
}

function addAlert(elementId, message, success) {
    $('#' + elementId).empty();
    if (success == true) {
        $('#' + elementId).append(
          '<div class="alert alert-block alert-success"><button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button>'
          + '<p><strong><i class="ace-icon fa fa-check"></i></strong> ' + message + '</p>'
          + '</div>');
    } else {
        $('#' + elementId).append(
           '<div class="alert alert-danger"><button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button>'
           + '<p><strong><i class="ace-icon fa fa-times"></i></strong> ' + message + '</p>'
           + '</div>');
    }
}
function FormatDateTimeDMYHM(datetime) {
    try {
        var valueDate = parseInt(datetime.substr(6));
        if (valueDate < 0)
            return "";
        else {
            var dateObj = new Date(valueDate);
            var dateStr = ('00' + dateObj.getDate()).slice(-2) + "/" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "/" + dateObj.getFullYear() + " " + ('00' + dateObj.getHours()).slice(-2) + ":" + ('00' + dateObj.getMinutes()).slice(-2);
            return dateStr;
        }
        return dateStr;
    } catch (e) {
        return "";
    }
}
function FormatDateTimeDMY(datetime) {
    try {
        var valueDate = parseInt(datetime.substr(6));
        if (valueDate < 0)
            return "";
        else {
            var dateObj = new Date(valueDate);
            var dateStr = ('00' + dateObj.getDate()).slice(-2) + "/" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "/" + dateObj.getFullYear();
            return dateStr;
        }
        return dateStr;
    } catch (e) {
        return "";
    }
}
function SetFormatDateTime(datetime) {
    try {
        var valueDate = parseInt(datetime.substr(6));
        if (valueDate < 0)
            return "";
        else {
            var dateObj = new Date(valueDate);
            var dateStr = ('00' + dateObj.getDate()).slice(-2) + "-" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "-" + dateObj.getFullYear();
            return dateStr;
        }
        return dateStr;
    } catch (e) {
        return "";
    }
}
function SetFormatDateTimeDMY(datetime) {
    try {
        var valueDate = parseInt(datetime.substr(6));
        if (valueDate < 0)
            return "";
        else {
            var dateObj = new Date(valueDate);
            var dateStr = ('00' + dateObj.getDate()).slice(-2) + "-" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "-" + dateObj.getFullYear();
            return dateStr;
        }
        return dateStr;
    } catch (e) {
        return "";
    }
}
$('.num').keyup(function () {
    this.value = this.value.replace(/[^0-9\.]/g, '');
});

function GetDiffDate(startDate, endDate) {
    var temp = endDate.setHours(0, 0, 0, 0) - startDate.setHours(0, 0, 0, 0);
    var diff_date = Math.round(temp / (24 * 60 * 60 * 1000));
    return diff_date;
}
function strip(html,numchar) {
    var tmp = document.createElement("DIV");
    tmp.innerHTML = html.replace(/<p[^>]*>/g, '').replace(/<\/p>/g, '<br/>');
    //var result = tmp.textContent || tmp.innerText || "";
    var result = tmp.textContent;
    if (result.length > numchar)
        return result.substring(0, numchar) + "...";
     return result
}
function formatCurrency(number) {
    var n = number.toString().split('').reverse().join("");
    var n2 = n.replace(/\d\d\d(?!$)/g, "$&.");
    return n2.split('').reverse().join('') + '';
}
function formatCurrencyVND(number) {
    var n = number.toString().split('').reverse().join("");
    var n2 = n.replace(/\d\d\d(?!$)/g, "$&.");
    return n2.split('').reverse().join('') + 'VND';
}
function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}
function isExtension(ext, extnArray) {
    var result = false;
    var i;
    if (ext) {
        ext = ext.toLowerCase();
        for (i = 0; i < extnArray.length; i++) {
            if (extnArray[i].toLowerCase() === ext) {
                result = true;
                break;
            }
        }
    }
    return result;
}