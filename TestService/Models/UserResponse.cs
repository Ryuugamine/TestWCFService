using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService.Models
{
    [DataContract]
    public class UserResponse
    {
        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public User User { get; set; }
    }
}