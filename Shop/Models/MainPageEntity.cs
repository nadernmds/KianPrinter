using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models.ApiEntity
{
    public class MainPageEntity
    {
        public List<SliderEntity> sliders = new List<SliderEntity>();
        public List<ProductEntity> bestSeller = new List<ProductEntity>();
        public List<CommentEntity> comment = new List<CommentEntity>();
        public List<ProductCategoryEntity> productCategory = new List<ProductCategoryEntity>();
        public List<ProductAttributeEntity> productAttribute = new List<ProductAttributeEntity>();
        public List<ProductImageEntity> productImage = new List<ProductImageEntity>();
    }
    public class SliderEntity
    {
        public int sliderID { get; set; }
        public string title { get; set; } = "";
    }

    public class ProductAttributeEntity
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    public class ContentEntity
    {
        public string adress { get; set; }
        public string phones { get; set; }
        public List<SocialEntity> social { get; set; }
        public string about { get; set; }
    }
    public class SocialEntity
    {
        public string type { get; set; }
        public string url { get; set; }
    }
    public class FaqEntity
    {
        public string question { get; set; }
        public string answer { get; set; }
    }
    public class ProductEntity
    {
        public int productID { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public string description { get; set; }
        public bool isFavorite = false;
        public long? visit { get; set; }
        public PriceEntity price { get; set; }
        public ProductCategoryEntity category { get; set; }
        public BrandCategory brand{ get; set; }
        public StateCategory state { get; set; }
        public int? existingCount = 1;
        public List<CommentEntity> comments = new List<CommentEntity>();
        public List<ProductImageEntity> images = new List<ProductImageEntity>();
        public List<ProductAttributeEntity> attributes = new List<ProductAttributeEntity>();
    }
    public class PriceEntity
    {
        public decimal price { get; set; }
        public decimal? offeredPrice { get; set; }
        public int? offpercent { get; set; }
    }
    public class ProductCategoryEntity
    {
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public ProductSubCategoryEntity parentCategory { get; set; }
        public List<ProductSubCategoryEntity> subCategories = new List<ProductSubCategoryEntity>();
    }
    public class ProductSubCategoryEntity
    {
        public int? categoryID { get; set; }
        public string categoryName { get; set; }
        public int? parentCategory { get; set; }
    }
    public class OrderEntity
    {
        public int orderID { get; set; }
        public decimal? orderOff { get; set; }
        public int? userID { get; set; }
        public UserAddressEntity address { get; set; }
        public OrderStateEntity orderState { get; set; }
        public string date { get; set; }
        public List<OrderProductEntity> orderProducts = new List<OrderProductEntity>();
    }
    public class OrderProductEntity
    {
        public int orderProductID { get; set; }
        public int? count { get; set; } 
        public int? orderID { get; set; }
        public ProductEntity product { get; set; }
    }
    public class OrderStateEntity
    {
        public int orderStateID { get; set; }
        public string state { get; set; }
    }
    public class UserAddressEntity
    {
        public int addressID { get; set; }
        public string addressDetail { get; set; }
        public string postalCode { get; set; }
        public CityEntity city { get; set; }
    }
    public class AddressEntity
    {
        public int addressID { get; set; }
        public string addressDetail { get; set; }
        public string postalCode { get; set; }
        public int? userID { get; set; }
        public CityEntity city { get; set; }
    }
    public class CityEntity
    {
        public int? cityID { get; set; }
        public string cityName { get; set; }
        public int? stateID { get; set; }
    }
    public class StateEntity
    {
        public int stateID { get; set; }
        public string stateName { get; set; }
    }

    public class UserEntity
    {
        public UserAddressEntity userinfo { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
        public bool? sex { get; set; }
    }
    public class BrandCategory
    {
        public int? brandID { get; set; }
        public string brandName { get; set; }
    }

    public class UserDetailEntity
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }

    public class ProductImageEntity
    {
        public int productImageID { get; set; }
        public int? productID { get; set; }
    }
    public class CommentEntity
    {
        public int commentID { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public string positive { get; set; }
        public string negative { get; set; }
        public int? productID { get; set; }
        public int? userID { get; set; }
        public int? rate { get; set; }
    }
    public class StateCategory
    {
        public int productStateID { get; set; }
        public string state { get; set; }
    }
    public class CartEntity
    {
        public int productID { get; set; }
        public int count { get; set; }
    }
}