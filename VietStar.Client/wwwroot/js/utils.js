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
function showBlock(div, text = 'Vui lòng chờ trong giây lát') {
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
        message: '<img src="../img/busy.gif" /> ' + text
    });
}
function showError(jqXHR, exception) {
    if (jqXHR.status == 403) {
        if (jqXHR.responseJSON.Flag == "NoLogin" || jqXHR.responseJSON.Flag == "NoAuthor")
            window.location = jqXHR.responseJSON.Url;
        else
            alert(jqXHR.responseText);
    }
    else {
        alert(jqXHR.responseText);
    }
}
function getValueDisplay(value, type) {
    if (isNullOrWhiteSpace(type)) {
        if (isNullOrWhiteSpace(value))
            return '';
        return value
    }
        
    var display = null
    switch (type) {
        case 'datetime':
            display = FormatDateTimeDMY(value);
        default: break;
    }
    return display;
}
function renderPageList(page, limit, totalRc) {
    totalPage = getTotalPage(totalRc, limit)
    var startPage = page > pageMargin ? page - pageMargin : 1;
    var endPage = pageMargin + page > totalPage ? totalPage : pageMargin + page
    var paging = $("#pagination");
    paging.empty();
    var first = renderGoFirstPage(page)
    var next = renderGoNextPage(page);
    var prev = renderGoPreviousPage(page)
    var last = renderGoLastPage(page)
    paging.append(first)
    paging.append(prev)
    for (var i = startPage; i <= endPage; i++) {
        var active = page == i ? ' active' : '';
        var item = "<li class='paginate_button" + active + " aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + i + ")' >"
            + "<a href='javascript:;'>" + i + "</a>"
            + "</li>"
        paging.append(item)
    }
    paging.append(next);
    paging.append(last)
}
function renderTextLeft(value, type, className = '') {
    return "<td class='text-left " + className + "'>" + getValueDisplay(value, type) + "</td>";
}
function renderTextCenter(value, type) {
    return "<td class='text-center'>" + getValueDisplay(value, type) + "</td>";
}
function renderGoPreviousPage(page) {
    let newCurrentPage = page
    if (page > 1) {
        newCurrentPage = page - 1
        return "<li class='paginate_button previous' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + newCurrentPage + ")'>"
            + "<a href='javascript:;'>Trước</a>"
            + "</li>"

    } else {
        return ""
    }
}
function renderCoBaoHiem(CoBaoHiem) {
    var value = ""
    
    if (CoBaoHiem == true)
        value = "<td class='text-left'> <div class='orange bolder' title='Không có bảo hiểm' ><i class=\"ace-icon fa fa-ban bigger-130\"></i></div> </td>";
    else
        value = "<td class='text-left'> <div class='green bolder'title = 'Có bảo hiểm' > <i class=\"ace-icon glyphicon glyphicon-ok bigger-130\"></i></div> </td>";
    return value;
}
function renderGoNextPage(page) {
    if (page < totalPage) {
        newCurrentPage = page + 1
        return "<li class='paginate_button next' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + newCurrentPage + ")' >"
            + "<a href='javascript:;'>Sau</a>"
            + "</li>"

    } else {
        return ""
    }
}
function renderGoLastPage(page) {

    if (totalPage > page) {
        return "<li class='paginate_button next' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + totalPage + ")' >"
            + "<a href='javascript:;'>Trang cuối</a>"
            + "</li>"
    } else {
        return ""
    }
}
function renderGoFirstPage(page) {
    if (totalPage > 1 && page > 1) {
        return "<li class='paginate_button previous' onclick='onClickPage(" + 1 + ")' >"
            + "<a href='javascript:;'>Trang đầu</a>"
            + "</li>"
    } else {
        return ""
    }
}
function FormatDateTimeDMY(datetime, outputFormat = 'DD-MM-YYYY') {
    
    let date = moment(datetime, 'YYYY-MM-DD')
    return date.format(outputFormat)
    //try {
    //    debugger
    //    var valueDate = parseInt(datetime.substr(6));
    //    if (valueDate < 0)
    //        return "";
    //    else {
    //        var dateObj = new Date(valueDate);
    //        var dateStr = ('00' + dateObj.getDate()).slice(-2) + "/" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "/" + dateObj.getFullYear();
    //        return dateStr;
    //    }
    //    return dateStr;
    //} catch (e) {
    //    return "";
    //}
}
function setCheckedValueOfRadioButtonGroup(name, boolValue) {
    var radios = document.getElementsByName(name);

    for (var j = 0; j < radios.length; j++) {
        if (radios[j].value === boolValue) {
            radios[j].checked = true;
            break;
        }
    }
}
function renderTextLink(textValue, href, type, className = '') {
    let display = getValueDisplay(textValue, type);
    return "<td class='text-left " + className + "'><a href='" + href + "' >" + display + "</a></td>";
}
function setValueForDateInput(controlId, value) {
    if (value === null)
        value = new Date().getDay + 1;
    controlId.val(value);
}
function getDateSpecific( backtoBefore = 0) {
    let toDay = new Date();
    let last = new Date((toDay).getTime() - (backtoBefore * 24 * 60 * 60 * 1000));
    return FormatDateTimeDMY(last, 'YYYY-MM-DD')
}
function getQueryStatus(controlId = null) {
    if (controlId == isNullOrWhiteSpace)
        controlId = document.getElementById("ddlStatus");
    let status = controlId.val()
    if (!isNullOrWhiteSpace(status))
        return status.toString();
    return '';
}
function removeDuplicate(value = null) {
    if (isNullOrWhiteSpace(value))
        return '';
    let arr = value.split(',');
    if (arr == null)
        return [];
    let uniqueSet = new Set(arr);
    let backToArray = [...uniqueSet]
    return backToArray.toString();
}
function getNewGuid() {
    const s4 = () => {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    };
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}