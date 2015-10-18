"use strict";

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

$(document).ready(function () {
    addConfirmationToElements();
});

function addConfirmationToElements() {
    console.log("Called addConfirmationToElements()");
    $(".add-confirm").dnnConfirm({
        text: "Are you sure you want to delete this?",
        yesText: "Yes",
        noText: "No",
        title: "Delete Confirmation"
    });
}