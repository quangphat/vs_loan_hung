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