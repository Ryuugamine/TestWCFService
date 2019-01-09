using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ShopUI.Models
{
    
    [DataContract]
    public class BooksInOrder
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "orderId")]
        public int OrderId { get; set; }

        [DataMember(Name = "bookId")]
        public int BookId { get; set; }
    }
    
}