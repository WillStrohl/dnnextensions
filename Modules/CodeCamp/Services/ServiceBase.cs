
using DotNetNuke.Web.Api;
using WillStrohl.Modules.CodeCamp.Entities;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public class ServiceBase : DnnApiController
    {
        protected string ERROR_MESSAGE = "An error occurred. Please check the event log or contact your site administrator for more information";

        protected CodeCampInfoController CodeCampDataAccess { get; set; }

        public ServiceBase()
        {
            CodeCampDataAccess = new CodeCampInfoController();
        }
    }
}