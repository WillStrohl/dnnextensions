"use strict";

var $momentFullDateFormat = "MM/DD/YYYY hh:mm A";
var $momentDateFormat = "MM/DD/YYYY";
var $momentTimeFormat = "hh:mm A";

function LogErrors(errors) {
    if (errors) {
        $.each(errors, function (i, error) {
            console.log("EVENT API RESPONSE ERROR - " + error.Code + ": " + error.Description);
        });
    }
}

function ParseDate(dateToFormat) {
    return new Date(parseInt(dateToFormat.substr(6)));
}

function ParseTime(dateToFormat) {
    var dateTime = new Date(parseInt(dateToFormat.substr(6)));
    var hours = dateTime.getHours();
    var minutes = dateTime.getMinutes();

    var time = ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2);

    return time;
}

function GetSlugFromValue(value) {
    /* Remove unwanted characters, only accept alphanumeric and space */
    var slug = value.replace(/[^A-Za-z0-9 ]/g, "");

    /* Replace multi spaces with a single space */
    slug = slug.replace(/\s{2,}/g, " ");

    /* Replace space with a '-' symbol */
    slug = slug.replace(/\s/g, "-");

    var cleanSlug = slug.toLowerCase();

    return cleanSlug;
}

function addMinutes(date, minutes) {
    return new Date(date.getTime() + minutes * 60000);
}

function cloneObject(obj) {
    if (obj === null || typeof obj !== "object") {
        return obj;
    }

    var temp = obj.constructor(); // give temp the original obj's constructor
    for (var key in obj) {
        temp[key] = cloneObject(obj[key]);
    }

    return temp;
}