using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ShopUI.Models
{
    
    [DataContract]
    public class OrderResponse : BaseResponse
    {
        [DataMember(Name = "orderData")]
        public OrderData OrderData { get; set; }
    }
    
}