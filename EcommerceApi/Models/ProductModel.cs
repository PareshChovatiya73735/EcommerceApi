using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceApi.Models
{
    public class ProductModel
    {
        public long ProductId { get; set; }
        public int ProdCatId { get; set; }
        public string CategoryName { get; set; }
        public string ProdName { get; set; }
        public string ProdDescription { get; set; }
        public List<ProductCategoryModel> CategoryList { get; set; }
        public List<ProductAttributeModel> AttributeList { get; set; }

        public ProductModel()
        {
            CategoryList = new List<ProductCategoryModel>();
            AttributeList = new List<ProductAttributeModel>();
        }
    }
}