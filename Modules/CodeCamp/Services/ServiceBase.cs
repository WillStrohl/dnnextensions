
using DotNetNuke.Web.Api;
using WillStrohl.Modules.CodeCamp.Entities;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public class ServiceBase : DnnApiController
    {
        protected string SUCCESS_MESSAGE = "SUCCESS";
        protected string ERROR_MESSAGE = "An error occurred. Please check the event log or contact your site administrator for more information";

        protected CodeCampInfoController CodeCampDataAccess { get; set; }
        protected RegistrationInfoController RegistrationDataAccess { get; set; }
        protected RoomInfoController RoomDataAccess { get; set; }
        protected SessionInfoController SessionDataAccess { get; set; }
        protected SessionRegistrationInfoController SessionRegistrationDataAccess { get; set; }
        protected SessionSpeakerInfoController SessionSpeakerDataAccess { get; set; }
        protected SpeakerInfoController SpeakerDataAccess { get; set; }

        public ServiceBase()
        {
            CodeCampDataAccess = new CodeCampInfoController();
            RegistrationDataAccess = new RegistrationInfoController();
            RoomDataAccess = new RoomInfoController();
            SessionDataAccess = new SessionInfoController();
            SessionRegistrationDataAccess = new SessionRegistrationInfoController();
            SessionSpeakerDataAccess = new SessionSpeakerInfoController();
            SpeakerDataAccess = new SpeakerInfoController();
        }
    }
}