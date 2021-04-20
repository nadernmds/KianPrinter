using Shop.Models;
using Shop.Models.ApiEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shop.Controllers.Api
{
    [RoutePrefix("api")]
    public class CategoryController : ApiController
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [Route("Category/CategoryPage")]
        public IHttpActionResult GetCategoryPage()
        {
            List<ProductCategoryEntity> c =
               db.ProductCategories.Where(a => a.parentCategory == null).Select(a =>
               new ProductCategoryEntity()
               {
                   categoryID = a.categoryID,
                   categoryName = a.categoryName,
                   parentCategory =
                   new ProductSubCategoryEntity()
                   {
                       categoryID = a.ProductCategory1.categoryID,
                       categoryName = a.ProductCategory1.categoryName,
                       parentCategory = a.ProductCategory1.parentCategory
                   },
                   subCategories = a.ProductCategories1.Select(b => new ProductSubCategoryEntity()
                   {
                       categoryID = b.categoryID,
                       categoryName = b.categoryName,
                       parentCategory = b.parentCategory // new ProductSubCategory from b.parent
                   }).ToList()
               }).ToList();
            return Ok(c);
        }

        [Route("Category/SubCategoryPage/{id}")]
        public IHttpActionResult GetSubCategoryPage(int id)
        {
            List<ProductCategoryEntity> categories = db.ProductCategories.Where(c => c.parentCategory == id)
                .Select(c => new ProductCategoryEntity
                {
                    categoryID = c.categoryID,
                    categoryName = c.categoryName,
                    parentCategory = new ProductSubCategoryEntity()
                    {
                        categoryID = c.ProductCategory1.categoryID,
                        categoryName = c.ProductCategory1.categoryName,
                        parentCategory = c.ProductCategory1.parentCategory
                    }
                }).ToList();

            List<int> subCategories = categories.Select(a => a.categoryID).ToList();
            subCategories.Add(id);
            List<ProductEntity> recommendedProducts = db.Products.Where(a => subCategories.Contains(a.categoryID.Value))
                .Select(a => new ProductEntity()
                {
                    productID = a.productID,
                    name = a.name,
                    price = new Models.ApiEntity.PriceEntity()
                    { price = a.price, offpercent = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                    shortDescription = a.shortDescription,
                    description = a.description,
                    brand = new Models.ApiEntity.BrandCategory()
                    { brandName = a.Brand.brandName },
                    state = new Models.ApiEntity.StateCategory()
                    { state = a.ProductState.state },
                    category = new Models.ApiEntity.ProductCategoryEntity()
                    { categoryName = a.ProductCategory.categoryName },
                    existingCount = 0
                }).ToList();

            var bestSellerProducts = db.Products.Where(a => subCategories.Contains(a.categoryID.Value))
                .Select(a => new ProductEntity()
                {
                    productID = a.productID,
                    name = a.name,
                    price = new Models.ApiEntity.PriceEntity()
                    { price = a.price, offpercent = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                    shortDescription = a.shortDescription,
                    description = a.description,
                    brand = new Models.ApiEntity.BrandCategory()
                    { brandName = a.Brand.brandName },
                    state = new Models.ApiEntity.StateCategory()
                    { state = a.ProductState.state },
                    category = new Models.ApiEntity.ProductCategoryEntity()
                    { categoryName = a.ProductCategory.categoryName },
                    existingCount = 0
                }).ToList();

            List<ProductEntity> newProduct = db.Products.Where(a => subCategories.Contains(a.categoryID.Value))
                .OrderByDescending(a => a.categoryID)
                .Select(a => new ProductEntity()
                {
                    productID = a.productID,
                    name = a.name,
                    price = new Models.ApiEntity.PriceEntity()
                    { price = a.price, offpercent = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                    shortDescription = a.shortDescription,
                    description = a.description,
                    brand = new Models.ApiEntity.BrandCategory()
                    { brandName = a.Brand.brandName },
                    state = new Models.ApiEntity.StateCategory()
                    { state = a.ProductState.state },
                    category = new Models.ApiEntity.ProductCategoryEntity()
                    { categoryName = a.ProductCategory.categoryName },
                    existingCount = 0
                }).ToList();
            var result = new { subCategories = categories, recommendedProducts = recommendedProducts, bestSellerProducts = bestSellerProducts, newProduct = newProduct };
            return Ok(result);
        }

        [Route("Category/ProductCategory")]
        public IHttpActionResult GetProductCategory()
        {
            List<ProductCategoryEntity> productcategory = db.ProductCategories
                .Select(c => new ProductCategoryEntity {
                    categoryID=c.categoryID,
                    categoryName=c.categoryName,
                    parentCategory=new Models.ApiEntity.ProductSubCategoryEntity
                    {
                        parentCategory=c.parentCategory
                    }
                }).ToList();
            return Ok(productcategory);
        }
    }
}
