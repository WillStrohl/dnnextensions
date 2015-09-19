
using System.Collections.Generic;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public interface IServiceResponse
    {
        List<ServiceError> Errors { get; set; }
    }
}