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
