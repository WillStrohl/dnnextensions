(function () {
    'use strict';

    codeCampApp.factory('codeCampEntityFactory', codeCampEntityFactory);

    codeCampEntityFactory.$inject = [];

    function codeCampEntityFactory() {

        var $self = this;

        //TODO: verify that instantiating new date like this will work as expected

        customProperties = function() {
            return new [
                {
                    Name: "",
                    Value: ""
                }
            ];
        }

        codeCampInfo = function () {
            return new {
                CodeCampId: -1,
                ModuleId: -1,
                Title: "",
                Description: "",
                BeginDate: new Date(),
                EndDate: new Date(),
                IconFile: "",
                AltText: "",
                RoleName: "",
                WaitListRoleName: "",
                MaximumCapacity: -1,
                NumberRegistered: -1,
                EventFull: false,
                RegistrationActive: false,
                ShowShirtSize: false,
                ShowAuthor: false,
                CreatedByUserId: -1,
                CreatedByDate: new Date,
                LastUpdatedByUserId: -1,
                LastUpdatedByDate: new Date(),
                CustomProperties: new $self.customProperties
            };
        }

        registrationInfo = function() {
            return new {
                RegistrationId: -1,
                CodeCampId: -1,
                UserId: -1,
                ShirtSize: "",
                RegistrationDate: new Date(),
                IsRegistered: false,
                CustomProperties: new $self.customProperties
            };
        }

        roomInfo = function() {
            return new {
                RoomId: -1,
                CodeCampId: -1,
                RoomName: "",
                Description: "",
                MaximumCapacity: -1,
                CreatedByUserId: -1,
                CreatedByDate: new Date,
                LastUpdatedByUserId: -1,
                LastUpdatedByDate: new Date(),
                CustomProperties: new $self.customProperties
            };
        }

        sessionInfo = function() {
            return new {
                SessionId: -1,
                Title: "",
                Description: "",
                TrackId: -1,
                TimeSlotId: -1,
                AudienceLevel: -1,
                NumberRegistered: -1,
                CreatedByUserId: -1,
                CreatedByDate: new Date,
                LastUpdatedByUserId: -1,
                LastUpdatedByDate: new Date(),
                IsApproved: false,
                ApprovedByUserId: -1,
                ApprovedByDate: new Date(),
                CustomProperties: new $self.customProperties
            };
        }

        sessionRegistrationInfo = function() {
            return new {
                SessionRegistrationId: -1,
                SessionId: -1,
                RegistrationId: -1
            };
        }

        sessionSpeakerInfo = function () {
            return new {
                SessionSpeakerId: -1,
                SessionId: -1,
                SpeakerId: -1
            };
        }

        speakerInfo = function() {
            return new {
                SpeakerId: -1,
                CodeCampId: -1,
                RegistrationId: -1,
                SpeakerName: "",
                CompanyName: "",
                CompanyTitle: "",
                URL: "",
                Bio: "",
                IconFile: "",
                IsAuthor: false,
                CreatedByUserId: -1,
                CreatedByDate: new Date,
                LastUpdatedByUserId: -1,
                LastUpdatedByDate: new Date(),
                CustomProperties: new $self.customProperties
            };
        }

        timeSlotInfo = function() {
            return new {
                TimeSlotId: -1,
                CodeCampId: -1,
                BeginTime: new Date(),
                EndTime: new Date(),
                AgendaText: "",
                SpanAllTracks: false,
                IncludeInDropDowns: false,
                CreatedByUserId: -1,
                CreatedByDate: new Date,
                LastUpdatedByUserId: -1,
                LastUpdatedByDate: new Date(),
                CustomProperties: new $self.customProperties
            };
        }

        trackInfo = function() {
            return new {
                TrackId: -1,
                CodeCampId: -1,
                RoomId: -1,
                Title: "",
                Description: "",
                CreatedByUserId: -1,
                CreatedByDate: new Date,
                LastUpdatedByUserId: -1,
                LastUpdatedByDate: new Date(),
                CustomProperties: new $self.customProperties
            };
        }

        volunteerInfo = function() {
            return new {
                VolunteerId: -1,
                RegistrationId: -1,
                Notes: "",
                CustomProperties: new $self.customProperties
            };
        }

        volunteerTaskInfo = function() {
            return new {
                VolunteerTaskId: -1,
                CodeCampId: -1,
                VolunteerId: -1,
                Title: "",
                BeginDate: new Date(),
                EndDate: new Date(),
                Completed: false,
                CreatedByUserId: -1,
                CreatedByDate: new Date,
                LastUpdatedByUserId: -1,
                LastUpdatedByDate: new Date(),
                CustomProperties: new $self.customProperties
            };
        }
    }
})();