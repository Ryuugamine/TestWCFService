using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TestService
{
    [DataContract]
    [SwaggerWcfDefinition(ExternalDocsUrl = "http://en.wikipedia.org/wiki/Book", ExternalDocsDescription = "Description of a book")]
    public class Book
    {
        [DataMember]
        [Description("Book ID")]
        public int Id { get; set; }

        [DataMember]
        [Description("Book Name")]
        public string Name { get; set; }

        [DataMember]
        [Description("Book Cost")]
        public int Cost { get; set; }

        [DataMember]
        [Description("Book Description")]
        public string Description { get; set; }

        [DataMember]
        public bool Deleted { get; set; }
    }
}