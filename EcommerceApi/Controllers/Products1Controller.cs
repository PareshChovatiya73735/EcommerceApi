using EcommerceApi.Contaxt;
using EcommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EcommerceApi.Controllers
{
    public class Products1Controller : ApiController
    {
        private ECommerceDemoEntities db = new ECommerceDemoEntities();

        public IQueryable<ProductModel> GetProducts()
        {
            List<ProductModel> products = (from p in db.Products
                                           join
                     c in db.ProductCategories on p.ProdCatId equals c.ProdCatId
                                           select new ProductModel
                                           {
                                               ProductId = p.ProductId,
                                               ProdCatId = c.ProdCatId,
                                               CategoryName = c.CategoryName,
                                               ProdName = p.ProdName,
                                               ProdDescription = p.ProdDescription
                                           }).ToList();
            return (IQueryable<ProductModel>)products;
        }
    }
}
