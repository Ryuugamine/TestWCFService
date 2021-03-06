﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService.Models
{

    [DataContract]
    public class BookResponse : BaseResponse
    {
        [DataMember(Name = "book")]
        public Book Book { get; set; }
    }
    
}