using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ShopUI.Models
{
    [DataContract]
    public class OrdersByUser : BaseResponse
    {
        [DataMember(Name = "orderData")]
        public List<OrderData> OrderData { get; set; }
    }
}