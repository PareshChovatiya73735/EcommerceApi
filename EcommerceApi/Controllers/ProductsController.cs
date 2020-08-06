using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EcommerceApi.Contaxt;
using EcommerceApi.Models;

namespace EcommerceApi.Controllers
{
    public class ProductsController : ApiController
    {
        private ECommerceDemoEntities db = new ECommerceDemoEntities();

        // GET: api/Products
        public List<ProductModel> GetProducts()
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
            return products;
        }

        [Route("api/Products/GetCategory")]
        public List<ProductCategoryModel> GetCategory()
        {
            List<ProductCategoryModel> category = (from c in db.ProductCategories
                                                   select new ProductCategoryModel
                                                   {
                                                       ProdCatId = c.ProdCatId,
                                                       CategoryName = c.CategoryName,

                                                   }).ToList();
            return category;
        }

        [Route("api/Products/GetAttributeLookups/{id}")]
        public List<ProductAttributeModel> GetAttributeLookups(int id)
        {
            List<ProductAttributeModel> attributeLookups = (from a in db.ProductAttributeLookups
                                                            where a.ProdCatId == id
                                                            select new ProductAttributeModel
                                                            {
                                                                AttributeId = a.AttributeId,
                                                                AttributeName = a.AttributeName,

                                                            }).ToList();
            return attributeLookups;
        }

        // GET: api/Products/5
        public IHttpActionResult GetProduct(long id)
        {
            ProductModel products = (from p in db.Products
                                     join
               c in db.ProductCategories on p.ProdCatId equals c.ProdCatId
                                     where p.ProductId == id
                                     select new ProductModel
                                     {
                                         ProductId = p.ProductId,
                                         ProdCatId = c.ProdCatId,
                                         ProdName = p.ProdName,
                                         ProdDescription = p.ProdDescription
                                     }).FirstOrDefault();

            products.CategoryList = (from c in db.ProductCategories
                                     select new ProductCategoryModel
                                     {
                                         ProdCatId = c.ProdCatId,
                                         CategoryName = c.CategoryName,

                                     }).ToList();

            products.AttributeList = (from aL in db.ProductAttributeLookups
                                      join aV in db.ProductAttributes on aL.AttributeId equals aV.AttributeId
                                      join c in db.ProductCategories on products.ProdCatId equals c.ProdCatId
                                      where aV.ProductId == products.ProductId
                                      select new ProductAttributeModel
                                      {
                                          AttributeId = aL.AttributeId,
                                          AttributeName = aL.AttributeName,
                                          AttributeValue = aV.AttributeValue,

                                      }).ToList();

            return Ok(products);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(long id, ProductModel product)
        {

            try
            {
                Product P = new Product();
                P.ProductId = product.ProductId;
                P.ProdName = product.ProdName;
                P.ProdCatId = product.ProdCatId;
                P.ProdDescription = product.ProdDescription;
                db.Entry(P).State = EntityState.Modified;
                db.SaveChanges();

                List<ProductAttribute> attribute = db.ProductAttributes.Where(x => x.ProductId == P.ProductId).ToList();
                db.ProductAttributes.RemoveRange(attribute);
                db.SaveChanges();

                foreach (var item in product.AttributeList)
                {
                    ProductAttribute att = new ProductAttribute();

                    att.AttributeId = item.AttributeId;
                    att.ProductId = P.ProductId;
                    att.AttributeValue = item.AttributeValue;
                    db.ProductAttributes.Add(att);
                    db.SaveChanges();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        public IHttpActionResult PostProduct(ProductModel product)
        {
            Product P = new Product();
            P.ProdName = product.ProdName;
            P.ProdCatId = product.ProdCatId;
            P.ProdDescription = product.ProdDescription;
            db.Products.Add(P);
            db.SaveChanges();

            foreach (var item in product.AttributeList)
            {
                ProductAttribute att = new ProductAttribute();
                att.AttributeId = item.AttributeId;
                att.ProductId = P.ProductId;
                att.AttributeValue = item.AttributeValue;
                db.ProductAttributes.Add(att);
                db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(long id)
        {
            List<ProductAttribute> attribute = db.ProductAttributes.Where(x => x.ProductId == id).ToList();
            db.ProductAttributes.RemoveRange(attribute);
            db.SaveChanges();

            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(long id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}