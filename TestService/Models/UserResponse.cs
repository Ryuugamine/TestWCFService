using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService.Models
{
    
    [DataContract]
    public class UserResponse : BaseResponse
    {
        [DataMember]
        public User User { get; set; }
    }
    
}