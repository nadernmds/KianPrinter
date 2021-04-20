using Shop.Models;
using Shop.Models.ApiEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;

namespace Shop.Controllers.Api
{
    [RoutePrefix("api")]
    public class ProductController : ApiController
    {
        Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        private int userID {
        get { return 5; }
        }
        public ProductController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

       public IHttpActionResult Get()
        {
            List<CommentEntity> comment = db.Comments.Select
                (c => new CommentEntity{ text = c.commentText,
                    commentID =c.commentID,
                    userID =c.userID})
                    .ToList();
            return Ok(comment);
        }

        [Route("Product/AddProductComment/{id}")]
        public IHttpActionResult Post([FromBody]CommentEntity comment)
        {
            int uid = 5;
            db.Comments.Add(new Comment
            {
                productID=comment.productID,
                rate= comment.rate,
                userID=uid,
                commentText=comment.text
            });
            db.SaveChanges();
            return Ok(true);
        }

        [Route("product/productDetailPage/{id}")]
        public IHttpActionResult GetProductDetailPage(int id)
        {
            int userID = 5; // get from User.Identity
            Product currentProduct = db.Products.Find(id);

            PriceEntity _price = new PriceEntity()
            {
                price = currentProduct.price,
                offeredPrice = currentProduct.Offers.LastOrDefault()?.price,
                offpercent = currentProduct.Offers.LastOrDefault()?.offPercent
            };

            List<CommentEntity> productComments = db.Comments.Where(a => a.productID == id).Select(c => new CommentEntity
            {
                productID=c.productID,
                commentID = c.commentID,
                text = c.commentText,
                name = c.User.name,
                positive = c.positive,
                negative = c.negative,
                rate = c.rate
            }).ToList();

            List<ProductImageEntity> productImage = db.ProductImages.Where(a => a.productID == id)
                .Select(c => new ProductImageEntity()
                {
                    productImageID = c.productImageID
                }).ToList();

            var productattributes = productattr();
            ProductEntity productdetails = db.Products.Where(c => c.productID == id)
                .Select(c => new ProductEntity
                {
                    isFavorite = c.Favorites.Any(r => r.userID == userID),
                    productID = c.productID,
                    name = c.name,
                    shortDescription = c.shortDescription,
                    description = c.description,
                    category = new Models.ApiEntity.ProductCategoryEntity()
                    { categoryName = c.ProductCategory.categoryName },
                    brand = new Models.ApiEntity.BrandCategory()
                    { brandName = c.Brand.brandName },
                    state = new Models.ApiEntity.StateCategory()
                    { state = c.ProductState.state }
                }).FirstOrDefault();

            productdetails.images = productImage;
            productdetails.comments = productComments;
            productdetails.attributes = productattributes;
            productdetails.price = _price;

            List<ProductEntity> sameProducts = db.Products.Where(a => a.categoryID == id)
                .Select(c => new ProductEntity
                {
                    productID = c.productID,
                    name = c.name,
                    price = new Models.ApiEntity.PriceEntity()
                    { price = c.price, offpercent = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                    shortDescription = c.shortDescription,
                    description = c.description,
                    category = new Models.ApiEntity.ProductCategoryEntity()
                    { categoryName = c.ProductCategory.categoryName },
                    brand = new Models.ApiEntity.BrandCategory()
                    { brandName = c.Brand.brandName },
                    state = new Models.ApiEntity.StateCategory()
                    { state = c.ProductState.state },
                    existingCount = 0
                }).ToList();

            sameProducts.ForEach(a => a.price = new PriceEntity() { });

            var result = new { productdetail = productdetails, sameProduct = sameProducts };
            return Ok(result);
        }


        private List<ProductAttributeEntity> productattr()
        {
            List<ProductAttributeEntity> productAttribute = new List<ProductAttributeEntity>();
            productAttribute.Add(new ProductAttributeEntity()
            {
                name = "اندازه",
                value = "بزرگ"

            });

            productAttribute.Add(new ProductAttributeEntity()
            {

                name = "رنگ",
                value = "آبی"
            });
            productAttribute.Add(new ProductAttributeEntity()
            {
                name = "وزن",
                value = "400 گرم"
            });
            return productAttribute;
        }

        [Route("Product/ProductInformationPage/{id}")]
        public IHttpActionResult GetProductInformationPage(List<int> id)
        {
            List<ProductEntity> product = db.Products.Where(a => id.Contains(a.productID))
                .Select(a => new ProductEntity
                {
                    productID = a.productID,
                    name = a.name,
                    price = new Models.ApiEntity.PriceEntity()
                    { price = a.price, offpercent = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                    shortDescription = a.shortDescription,
                    description = a.description,
                    category = new Models.ApiEntity.ProductCategoryEntity
                    {
                        categoryName = a.ProductCategory.categoryName
                    },
                    brand = new Models.ApiEntity.BrandCategory
                    {
                        brandName = a.Brand.brandName
                    },
                    state = new Models.ApiEntity.StateCategory
                    {
                        state = a.ProductState.state
                    }
                }).ToList();
            return Ok(product);
        }

        [Route("Product/ProductOfCategoryPage")]
        public IHttpActionResult Post([FromBody]ProductOfCategoryInput input)
        {
            List<ProductEntity> product = db.Products.Where(a => a.categoryID == input.catID).Take(20)
                  .Select(a => new ProductEntity
                  {
                      productID = a.productID,
                      visit = a.visit,
                      name = a.name,
                      price = new Models.ApiEntity.PriceEntity()
                      { price = a.price, offpercent = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                      shortDescription = a.shortDescription,
                      description = a.description,
                      brand = new Models.ApiEntity.BrandCategory()
                      {
                          brandName = a.Brand.brandName
                      },
                      state = new Models.ApiEntity.StateCategory()
                      {
                          state = a.ProductState.state
                      },
                      category = new Models.ApiEntity.ProductCategoryEntity()
                      {
                          categoryName = a.ProductCategory.categoryName
                      },
                      existingCount = 0

                  }).ToList();

            input.sort = input.sort.ToLower().Replace("sorttype.", string.Empty);
            if (input.sort == "newest")
            {
                product = product.OrderByDescending(a => a.productID).ToList();
            }
            else if (input.sort == "priceup2down")
            {
                product = product.OrderByDescending(a => a.price.price).ToList();
            }
            else if (input.sort == "pricedown2up")
            {
                product = product.OrderBy(a => a.price.price).ToList();
            }
            else if (input.sort == "moresell")
            {
                //product = product.OrderBy(a => a).ToList();
            }
            else if (input.sort == "byview")
            {
                product = product.OrderByDescending(a => a.visit).ToList();
            }
            return Ok(product);
        }

        [Route("Product/ProductSearchPage/{id}")]
        public IHttpActionResult GetProductSearchPage(string id)
        {
            List<ProductEntity> product = db.Products.Where(a => a.name.Contains(id))
                .Select(a => new ProductEntity
                {
                    productID = a.productID,
                    name = a.name,
                    price = new Models.ApiEntity.PriceEntity()
                    {
                        price = a.price,
                        offpercent = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent,
                        offeredPrice = a.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price
                    },
                    shortDescription = a.shortDescription,
                    description = a.description,
                    category = new Models.ApiEntity.ProductCategoryEntity
                    {
                        categoryName = a.ProductCategory.categoryName
                    },
                    brand = new Models.ApiEntity.BrandCategory
                    {
                        brandName = a.Brand.brandName
                    },
                    state = new Models.ApiEntity.StateCategory
                    {
                        state = a.ProductState.state
                    }
                }).ToList();
            return Ok(product);
        }
        [HttpPost,Route("Product/AddFavorite")]
        public IHttpActionResult Post(int pid)
        {
            // 0 : remove from fav 
            // 1 : add    to   fav
            bool res = false;
            int id = 5;
            var ufavorite = db.Favorites.Any(c => c.productID == pid && c.userID==id);
            if(ufavorite)
            {
                var favorite = db.Favorites.Where(c => c.productID == pid && c.userID == id).FirstOrDefault();
                db.Favorites.Remove(favorite);
                db.SaveChanges();
            }
            else if(!ufavorite)
            {
                db.Favorites.Add(new Favorite
                {
                    productID = pid,
                    userID=id
                });
                db.SaveChanges();
                res = true;
            }
            return Ok(res);
        }
        [Route("Product/MainPage")]
        public IHttpActionResult GetMainPage()
        {
            List<ProductEntity> json = db.Products.Take(10).Select(c => new ProductEntity
            {
                productID = c.productID,
                name = c.name,
                price = new Models.ApiEntity.PriceEntity()
                { price = c.price, offpercent = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                shortDescription = c.shortDescription,
                description = c.description
                ,
                category = new Models.ApiEntity.ProductCategoryEntity()
                {
                    categoryName = c.ProductCategory.categoryName,
                    parentCategory = new ProductSubCategoryEntity()
                },
                brand = new Models.ApiEntity.BrandCategory() { brandName = c.Brand.brandName },
                state = new Models.ApiEntity.StateCategory() { state = c.ProductState.state }
            }).ToList();

            List<ProductEntity> product = db.Products.Take(10).Select
                (c => new ProductEntity
                {
                    productID = c.productID,
                    name = c.name,
                    price = new Models.ApiEntity.PriceEntity()
                    {
                        price = c.price,
                        offpercent = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent,
                        offeredPrice = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price
                    },
                    shortDescription = c.shortDescription,
                    description = c.description,
                    category = new Models.ApiEntity.ProductCategoryEntity()
                    {
                        categoryName = c.ProductCategory.categoryName,
                        parentCategory = new ProductSubCategoryEntity()
                    },
                    brand = new Models.ApiEntity.BrandCategory() { brandName = c.Brand.brandName },
                    state = new Models.ApiEntity.StateCategory() { state = c.ProductState.state }
                }).ToList();


            List<ProductCategoryEntity> category = db.ProductCategories.Where(c => c.parentCategory == null)
                .Select(c =>
                new ProductCategoryEntity
                {
                    categoryID = c.categoryID,
                    categoryName = c.categoryName,
                    parentCategory = new ProductSubCategoryEntity()
                    {
                        categoryID = c.ProductCategory1.categoryID,
                        categoryName = c.ProductCategory1.categoryName,
                        parentCategory = c.ProductCategory1.parentCategory
                    },
                    subCategories = c.ProductCategories1.Select(n => new ProductSubCategoryEntity() { categoryID = n.categoryID, categoryName = n.categoryName, parentCategory = n.parentCategory }).ToList()
                }).ToList();

            List<ProductEntity> recentproduct1 = db.Products.Take(10).Select(c => new ProductEntity
            {
                productID = c.productID,
                name = c.name,
                price = new Models.ApiEntity.PriceEntity()
                { price = c.price, offpercent = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                shortDescription = c.shortDescription,
                description = c.description,
                category = new Models.ApiEntity.ProductCategoryEntity()
                {
                    categoryName = c.ProductCategory.categoryName,
                    parentCategory = new ProductSubCategoryEntity()
                },
                brand = new Models.ApiEntity.BrandCategory() { brandName = c.Brand.brandName },
                state = new Models.ApiEntity.StateCategory() { state = c.ProductState.state }
            }).ToList();
            List<ProductEntity> recentproduct2 = db.Products.Take(10).Select(c => new ProductEntity
            {
                productID = c.productID,
                name = c.name,
                price = new Models.ApiEntity.PriceEntity()
                { price = c.price, offpercent = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent, offeredPrice = c.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price },
                shortDescription = c.shortDescription,
                description = c.description,
                category = new Models.ApiEntity.ProductCategoryEntity()
                { categoryName = c.ProductCategory.categoryName,
                    parentCategory = new ProductSubCategoryEntity()
                },
                brand = new Models.ApiEntity.BrandCategory() { brandName = c.Brand.brandName },
                state = new Models.ApiEntity.StateCategory() { state = c.ProductState.state }
            }).ToList();
            var Slider = getSliders();
            var result = new { Bestproduct = json, Offerproduct = product, slider = Slider, Category = category, RecentProducts1 = recentproduct1, RecentProducts2 = recentproduct2 };
            return Ok(result);

        }

        private List<SliderEntity> getSliders()
        {
            List<SliderEntity> sliders = db.ShopCarousels.OrderByDescending(a => a.shopCarouselID).Take(4).Select(a => new SliderEntity
            {
                sliderID = a.shopCarouselID,
                title = ""
            }).ToList();
            return sliders;
        }


        #region Cart
        [HttpGet,Route("Product/GetCart")]
        public List<ProductEntity> getCart()
        {
            List<ProductEntity> cartList = db.Carts.Where(a => a.userID == userID)
                .Select(b => new ProductEntity()
                {
                existingCount = b.count,
                productID = b.productID.Value,
                price = new PriceEntity()
                {
                    price = b.Product.price,
                    offeredPrice = b.Product.Offers.OrderByDescending(c => c.offerID).FirstOrDefault().price,
                    offpercent = b.Product.Offers.OrderByDescending(c => c.offerID).FirstOrDefault().offPercent
                },
                name = b.Product.name,
                shortDescription = b.Product.shortDescription
            }).ToList();
            return cartList;
        }

        [HttpPost,Route("Product/DeleteCart")]
        public bool DeleteCart([FromBody] int pid)
        {
            int id = 5;
            var cart = db.Carts.Where(a => a.userID == id && a.productID == pid).FirstOrDefault();
            if (cart != null)
            {
                db.Carts.Remove(cart);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        [HttpPost, Route("Product/AddCart")]
        public bool addToCart([FromBody]CartEntity cartEntity)
        {
            // if product is exist in cart Update count
            Cart item = db.Carts.Where(a => a.userID == userID && a.productID == cartEntity.productID).FirstOrDefault();
            if (item != null)
            {
                item.count = cartEntity.count;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else  // Add new Cart
            {
                db.Carts.Add(new Cart() {
                    count = cartEntity.count,
                    expire = DateTime.Now.AddDays(2),
                    productID = cartEntity.productID,
                    userID = userID
                });
                db.SaveChanges();
                return true;
            }         
        }
        #endregion
    }
}
