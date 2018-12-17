using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService.Models
{
    [DataContract]
    public class OrderResponse
    {
        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public OrderRequest OrderData { get; set; }
    }
}