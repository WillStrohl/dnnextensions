"use strict";

// I control the Alert modal window.
// --
// NOTE: This controller gets "modals" injected; but, it is in no way 
// different than any other Controller in your entire AngularJS application. 
// It takes services, manages the view-model, and knows NOTHING about the DOM.
codeCampApp.controller(
    "AlertModalController",
    function ($scope, modals) {

        // Setup default values using modal params.
        $scope.message = (modals.params().message || "Whoa!");


        // ---
        // PUBLIC METHODS.
        // ---


        // Wire the modal buttons into modal resolution actions.
        $scope.close = modals.resolve;


        // I jump from the current alert-modal to the confirm-modal.
        $scope.jumpToConfirm = function () {

            // We could have used the .open() method to jump from one modal 
            // to the next; however, that would have implicitly "rejected" the
            // current modal. By using .proceedTo(), we open the next window, but
            // defer the resolution of the current modal until the subsequent 
            // modal is resolved or rejected.
            modals.proceedTo(
                "confirm",
                {
                    message: "I just came from Alert - doesn't that blow your mind?",
                    confirmButton: "Eh, maybe a little",
                    denyButton: "Oh please"
                }
            )
            .then(
                function handleResolve() {

                    console.log("Piped confirm resolved.");

                },
                function handleReject() {

                    console.warn("Piped confirm rejected.");

                }
            );

        };

    }
);


// -------------------------------------------------- //
// -------------------------------------------------- //


// I control the Confirm modal window.
// --
// NOTE: This controller gets "modals" injected; but, it is in no way 
// different than any other Controller in your entire AngularJS application. 
// It takes services, manages the view-model, and knows NOTHING about the DOM.
codeCampApp.controller(
    "ConfirmModalController",
    function ($scope, modals) {

        var params = modals.params();

        // Setup defaults using the modal params.
        $scope.message = (params.message || "Are you sure?");
        $scope.confirmButton = (params.confirmButton || "Yes!");
        $scope.denyButton = (params.denyButton || "Oh, hell no!");


        // ---
        // PUBLIC METHODS.
        // ---


        // Wire the modal buttons into modal resolution actions.
        $scope.confirm = modals.resolve;
        $scope.deny = modals.reject;

    }
);


// -------------------------------------------------- //
// -------------------------------------------------- //


// I control the Prompt modal window.
// --
// NOTE: This controller gets "modals" injected; but, it is in no way 
// different than any other Controller in your entire AngularJS application. 
// It takes services, manages the view-model, and knows NOTHING about the DOM.
codeCampApp.controller(
    "PromptModalController",
    function ($scope, modals) {

        // Setup defaults using the modal params.
        $scope.message = (modals.params().message || "Give me.");

        // Setup the form inputs (using modal params).
        $scope.form = {
            input: (modals.params().placeholder || "")
        };

        $scope.errorMessage = null;


        // ---
        // PUBLIC METHODS.
        // ---


        // Wire the modal buttons into modal resolution actions.
        $scope.cancel = modals.reject;


        // I process the form submission.
        $scope.submit = function () {

            // If no input was provided, show the user an error message.
            if (!$scope.form.input) {

                return ($scope.errorMessage = "Please provide something!");

            }

            modals.resolve($scope.form.input);

        };

    }
);