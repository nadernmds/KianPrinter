using Shop.Models;
using Shop.Models.ApiEntity;
using Shop.Models.input;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Shop.Controllers.Api
{
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [Route("User/UserOrderPage")]
        public async Task<IHttpActionResult> GetUserOrderPage()
        {
            int id = 5;
            var c =
           await     db.Ordes.Where(a => a.userID == id)
                .Select(a => new OrderEntity()
                {
                    orderID = a.orderID,
                    orderOff = a.orderOff,
                    address = new Models.ApiEntity.UserAddressEntity()
                    {
                        addressDetail = a.Address.addressDetail,
                        postalCode = a.Address.postalCode,
                        city = new Models.ApiEntity.CityEntity()
                        {
                            cityName = a.Address.City.cityName
                        }
                    },
                    orderState = new Models.ApiEntity.OrderStateEntity()
                    {
                        state = a.OrderState.state
                    },
                    date=a.date.toPersianDate(),
                    orderProducts = a.OrderProducts.Select(b => new OrderProductEntity()
                    {
                        orderProductID = b.orderProductID,
                        count = b.count,
                        orderID = b.orderID,
                        product = new Models.ApiEntity.ProductEntity()
                        {
                            productID = b.Product.productID,
                            name = b.Product.name,
                            price = new Models.ApiEntity.PriceEntity()
                            {
                                price = b.Product.price,
                                offpercent = b.Product.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().offPercent,
                                offeredPrice = b.Product.Offers.OrderByDescending(w => w.offerID).FirstOrDefault().price
                            },
                            shortDescription = b.Product.shortDescription,
                            //description = b.Product.description,
                            category = new Models.ApiEntity.ProductCategoryEntity()
                            {
                                categoryName = b.Product.ProductCategory.categoryName
                            },
                            brand = new Models.ApiEntity.BrandCategory()
                            {
                                brandName = b.Product.Brand.brandName
                            },
                            state = new Models.ApiEntity.StateCategory()
                            {
                                state = b.Product.ProductState.state
                            }
                        }
                    }).ToList()
                }).ToListAsync();
            return Ok(c);
        }

        [Route("User/UserProfilePage")]
        public IHttpActionResult GetUserProfilePage()
        {
            int id = 5;
            UserEntity userinfo = db.Users.Where(a => a.userID == id)
                .Select(a => new UserEntity
                {
                    userinfo = new Models.ApiEntity.UserAddressEntity()
                    {
                        city = new Models.ApiEntity.CityEntity()
                        {
                            cityID = a.Addresses.OrderByDescending(v => v.addressID).FirstOrDefault().cityID,
                            stateID = a.Addresses.OrderByDescending(v => v.addressID).FirstOrDefault().City.stateID
                        }
                    },
                    sex = a.sex,
                    birthday = a.birthday.ToString(),
                    name = a.name,
                    mobile = a.mobile,
                    email = a.email,
                }).FirstOrDefault();
            // convert datetime to PC
            userinfo.birthday = "1372/01/28";
            return Ok(userinfo);
        }

        [Route("User/UserFavoritePage")]
        public IHttpActionResult GetUserFavoritePage()
        {
            int id = 5;
            List<ProductEntity> favorite = db.Favorites.Where(a => a.userID == id)
                .Select(a => new ProductEntity
                {
                        productID = a.productID.Value,
                        isFavorite = true,
                        name = a.Product.name,
                    price = new Models.ApiEntity.PriceEntity()
                    {
                        price = a.Product.price,
                    },
                        shortDescription = a.Product.shortDescription,
                        category = new Models.ApiEntity.ProductCategoryEntity()
                        {
                            categoryName = a.Product.ProductCategory.categoryName
                        },
                        brand = new Models.ApiEntity.BrandCategory()
                        {
                            brandName = a.Product.Brand.brandName
                        },
                        state = new Models.ApiEntity.StateCategory()
                        {
                            state = a.Product.ProductState.state
                        }
                    
                }).ToList();
            return Ok(favorite);
        }

        [Route("User/UserAddressPage")]
        public IHttpActionResult GetUserAddressPage()
        {
            int id = 5;
            List<UserAddressEntity> c = 
             db.Addresses.Where(a => a.userID == id).Select(a => new UserAddressEntity()
            {
                addressID = a.addressID,
                addressDetail = a.addressDetail,
                postalCode = a.postalCode,
                city = new Models.ApiEntity.CityEntity()
                {
                    cityName = a.City.cityName
                }

            }).ToList();
            return Ok(c);
        }

        [Route("User/ContactInfo")]
        public IHttpActionResult GetContactInfo()
        {
            var content = db.Contents.Select(a => new ContentEntity()
            {
                adress = a.adress,
                phones = a.phones,
                social = a.Socials.Select(b => new SocialEntity() { type = b.type, url = b.url }).ToList(),
                about = a.aboutUs
            }).FirstOrDefault();
            return Ok(content);
        }

        [Route("User/FaqInfo")]
        public IHttpActionResult GetFaqInfo()
        {
            var faq = db.Faqs.Select(a => new FaqEntity()
            {
                question=a.question,
                answer=a.answer
            }).ToList();
            return Ok(faq);
        }

        [Route("User/AddUserAddressPage/{id}")]
        public IHttpActionResult Post([FromBody] AddressInput addressInput)
        {
            db.Addresses.Add(new Address
            {
                addressDetail = addressInput.addressDetail,
                postalCode = addressInput.postalCode,
                userID = addressInput.userID,
                cityID = addressInput.cityID
            });
            return Ok(true);
        }

        [Route("User/UpdateUserAddressPage/{id}")]
        [HttpPut]
        public IHttpActionResult Put([FromBody] AddressInput addressInput)
        {
            Models.Address address = db.Addresses.Find(addressInput.addressID);
            address.addressDetail = addressInput.addressDetail;
            address.postalCode = addressInput.postalCode;
            address.userID = addressInput.userID;
            address.cityID = addressInput.cityID;
            db.Entry(address).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }

        [Route("User/DeleteUserAddressPage/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
            db.SaveChanges();
            return Ok(true);
        }

        [Route("User/UpdatePasswordPage/{id}")]
        public IHttpActionResult Post([FromBody] PasswordInput input)
        {
            int id = 5;
            Models.User CurrentUser = db.Users.Find(id);
            CurrentUser.password = input.newPassword;
            db.Entry(CurrentUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }

        [HttpPost, Route("User/EditUserProfilePage")]
        public IHttpActionResult Post([FromBody] UserInput input)
        {
            int id = 5;
            Models.User CurrentUser = db.Users.Find(id);
            CurrentUser.username = input.username;
            CurrentUser.password = input.password;
            CurrentUser.name = input.name;
            CurrentUser.mobile = input.mobile;
            CurrentUser.email = input.email;
            CurrentUser.sex = input.sex;
            db.Entry(CurrentUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok(true);
        }
        [Route("User/AddUserPage/{id}")]
        public IHttpActionResult PostAddUserPage([FromBody] UserInput userInput)
        {
            db.Users.Add(new User
            {
                username = userInput.username,
                password = userInput.password,
                name = userInput.name,
                mobile = userInput.mobile,
                usergroupID = userInput.usergroupID,
                email = userInput.email
            });
            return Ok(true);
        }


    }
}
