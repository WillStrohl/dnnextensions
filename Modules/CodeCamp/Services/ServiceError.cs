
using System;
using System.Runtime.Serialization;

namespace WillStrohl.Modules.CodeCamp.Services
{
    [DataContract]
    [Serializable]
    public class ServiceError
    {
        [DataMember]
        public string Code { get; set; }
        
        [DataMember]
        public string Description { get; set; }

        public ServiceError()
        {
        }

        public ServiceError(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}