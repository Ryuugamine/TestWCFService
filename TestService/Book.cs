using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService
{
    [DataContract]
    public class Book
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int cost { get; set; }

        [DataMember]
        public string description { get; set; }
    }
}