﻿function getURLParameter(name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null
}

function deleteCookies() {
    document.cookie = "MyTestCookie=; expires=Thu, 01 Jan 1970 00:00:00 UTC; Path=/";
    document.cookie = "ID=; expires=Thu, 01 Jan 1970 00:00:00 UTC; Path=/";
    document.cookie = "CanCreate=; expires=Thu, 01 Jan 1970 00:00:00 UTC; Path=/";

    window.location.href = '/Login.aspx';
}

function GetUserName() {
    var username = getCookie("MyTestCookie");
    
    if (username === "")
        window.location.href = '/Login.aspx';

    return username;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function eraseSession ()
{
    sessionStorage.clear();
}

function setUpParents(data) {
    for (i = 0 ; i < data.length ; i++)
        $("#parents").append("<option value=\"" + data[i].ID + "\">" + data[i].ID + "</option>");
}

function setUpParents(data, ID) {
    for (i = 0 ; i < data.length ; i++)
        if (data[i].ID !== ID)
            $("#parents").append("<option value=\"" + data[i].ID + "\">" + data[i].ID + "</option>");
}

function setUpDropDown(data) {
    for (i = 0 ; i < data.length ; i++)
        $("#roles").append("<option value=\"" + data[i].ID + "\">" + data[i].Type + "</option>")
}