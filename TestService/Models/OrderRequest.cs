﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService.Models
{
    [DataContract]
    public class OrderRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int TotalPayment { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public List<int> Books { get; set; }
    }
}