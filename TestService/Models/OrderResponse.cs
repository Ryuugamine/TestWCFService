using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService.Models
{
    
    [DataContract]
    public class OrderResponse : BaseResponse
    {
        [DataMember]
        public OrderRequest OrderData { get; set; }
    }
    
}