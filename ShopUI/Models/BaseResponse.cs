using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ShopUI.Models
{
    
    [DataContract]
    public class BaseResponse
    {
        [DataMember (Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
    
}