//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EcommerceApi.Contaxt
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductAttribute
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeValue { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual ProductAttributeLookup ProductAttributeLookup { get; set; }
    }
}