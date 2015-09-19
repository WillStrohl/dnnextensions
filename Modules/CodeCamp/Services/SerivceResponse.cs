
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WillStrohl.Modules.CodeCamp.Services
{
    [DataContract]
    [Serializable]
    public class ServiceResponse<T> : IServiceResponse
    {
        [DataMember]
        public List<ServiceError> Errors { get; set; }

        [DataMember]
        public T Content { get; set; }

        public ServiceResponse()
        {
            Errors = new List<ServiceError>();
        }
    }
}