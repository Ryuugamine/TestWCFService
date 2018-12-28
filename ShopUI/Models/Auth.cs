using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ShopUI.Models
{
    [DataContract]
    public class Auth
    {
        [DataMember (Name = "email")]
        public string Email { get; set; }

        [DataMember (Name = "password")]
        public string Password { get; set; }
    }
}