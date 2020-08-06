using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceApi.Models
{
    public class ProductAttributeModel
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}