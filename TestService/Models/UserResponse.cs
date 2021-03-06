﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService.Models
{
    
    [DataContract]
    public class UserResponse : BaseResponse
    {
        [DataMember(Name = "user")]
        public User User { get; set; }
    }
    
}