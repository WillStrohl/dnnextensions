"use strict";

function LogErrors(errors) {
    if (errors) {
        $.each(errors, function (i, error) {
            console.log("EVENT API RESPONSE ERROR - CODE: " + error.Code + " DESCRIPTION: " + error.Description);
        });
    }
}