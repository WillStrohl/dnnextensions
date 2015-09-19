
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public partial class CodeCampController : DnnApiController
    {
        #region Testing

        /// <summary>
        /// Use to test a successful response
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/CodeCamp/Ping
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Ping()
        {
            var response = new ServiceResponse<string>() {Content = "Success"};

            return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
        }

        /// <summary>
        /// Use to test a failed response
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/CodeCamp/PingError
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage PingError()
        {
            var errors = new List<ServiceError>();

            errors.Add(new ServiceError()
            {
                Code = "NULL",
                Description = "Some value was null"
            });

            var response = new ServiceResponse<string>() { 
                Errors = errors
            };

            return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
        }

        #endregion
    }
}