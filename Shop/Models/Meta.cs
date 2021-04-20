using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    //public class Meta
    //{
    //}
    internal class MetaAddress
    {
        public int addressID { get; set; }
        [Display(Name = "آدرس")]
        [DisplayName("آدرس")]
        public string addressDetail { get; set; }
        [Display(Name = "کد پستی")]
        [DisplayName("کد پستی")]
        public string postalCode { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> cityID { get; set; }
        [Display(Name = "شماره ثابت")]
        [DisplayName("شماره ثابت")]
        public string phone { get; set; }
        [Display(Name = "موبایل")]
        [DisplayName("موبایل")]
        public string mobile { get; set; }
        [Display(Name = "مختصات جغرافیایی ")]
        [DisplayName("مختصات جغرافیایی ")]
        public string location { get; set; }
    }
    [MetadataType(typeof(MetaAddress))]
    public partial class Address
    {
    }

    internal class MetaAnswer
    {
        public int answerID { get; set; }
        [Display(Name = "پاسخ")]
        [DisplayName("پاسخ")]
        public string answerText { get; set; }
        [Display(Name = "تاریخ")]
        [DisplayName("تاریخ")]
        public Nullable<System.DateTime> answerDate { get; set; }
        public Nullable<int> questionID { get; set; }
        public Nullable<int> userID { get; set; }
    }
    [MetadataType(typeof(MetaAnswer))]
    public partial class Answer
    {
    }
    internal class MetaBrand
    {

        public int brandID { get; set; }
        [Display(Name = "برند")]
        [DisplayName("برند")]
        public string brandName { get; set; }
        [Display(Name = "نام‌انگلیسی‌برند")]
        [DisplayName("نام‌انگلیسی‌برند")]
        public string enBrandName { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
    [MetadataType(typeof(MetaBrand))]
    public partial class Brand
    {
    }

    internal class MetaCity
    {
        public int cityID { get; set; }
        [Display(Name = "شهر")]
        [DisplayName("شهر")]
        public string cityName { get; set; }
        public Nullable<int> stateID { get; set; }
    }
    [MetadataType(typeof(MetaCity))]
    public partial class City
    {
    }

    internal class MetaComment
    {
        public int commentID { get; set; }
        [Display(Name = "نظر")]
        [DisplayName("نظر")]
        public string commentText { get; set; }
        [Display(Name = "نقاط قوت")]
        [DisplayName("نقاط قوت")]
        public string positive { get; set; }
        [Display(Name = "نقاط ضعف")]
        [DisplayName("نقاط ضعف")]
        public string negative { get; set; }
        public Nullable<int> productID { get; set; }
        public Nullable<int> userID { get; set; }
        [Display(Name = "امتیاز")]
        [DisplayName("امتیاز")]
        public Nullable<int> rate { get; set; }

    }
    [MetadataType(typeof(MetaComment))]
    public partial class Comment
    {
    }

    internal class MetaOffer
    {
        public int offerID { get; set; }
        [Display(Name = "تاریخ شروع")]
        [DisplayName("تاریخ شروع")]
        public Nullable<System.DateTime> startDate { get; set; }
        [Display(Name = "تاریخ پایان")]
        [DisplayName("تاریخ پایان")]
        public Nullable<System.DateTime> endDate { get; set; }
        [Display(Name = "قیمت")]
        [DisplayName("قیمت")]
        public Nullable<decimal> price { get; set; }
        [Display(Name = "درصد")]
        [DisplayName("درصد")]
        public Nullable<int> offPercent { get; set; }
        public Nullable<int> productID { get; set; }

    }
    [MetadataType(typeof(MetaOffer))]
    public partial class Offer
    {

    }
    internal class MetaOrde
    {
        public int orderID { get; set; }
        [Display(Name = "تخفیف")]
        [DisplayName("تخفیف")]
        public Nullable<decimal> orderOff { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> addressID { get; set; }
        public Nullable<int> orderStateID { get; set; }
        [Display(Name = "تاریخ")]
        [DisplayName("تاریخ")]
        public Nullable<System.DateTime> date { get; set; }
    }
    [MetadataType(typeof(MetaOrde))]
    public partial class Orde
    {
    }

    internal class MetaOrderProduct
    {
        public int orderProductID { get; set; }
        [Display(Name = "تعداد")]
        [DisplayName("تعداد")]
        public Nullable<int> count { get; set; }

        public Nullable<int> orderID { get; set; }
        public Nullable<int> productID { get; set; }

    }
    [MetadataType(typeof(MetaOrderProduct))]
    public partial class OrderProduct
    {
    }

    internal class MetaOrderState
    {
        public int orderStateID { get; set; }
        [Display(Name = "وضعیت")]
        [DisplayName("وضعیت")]
        public string state { get; set; }
    }
    [MetadataType(typeof(MetaOrderState))]
    public partial class OrderState
    {
    }

    internal class MetaProduct
    {
        public int productID { get; set; }
        [Display(Name = "محصول")]
        [DisplayName("محصول")]
        public string name { get; set; }
        [Display(Name = "قیمت")]
        [DisplayName("قیمت")]
        public Nullable<decimal> price { get; set; }
        [Display(Name = "توضیح کوتاه")]
        [DisplayName("توضیح کوتاه")]
        public string shortDescription { get; set; }
        [Display(Name = "توضیحات")]
        [DisplayName("توضیحات")]
        public string description { get; set; }
        public Nullable<int> categoryID { get; set; }
        public Nullable<int> brandID { get; set; }
        public Nullable<int> productStateID { get; set; }
        [Display(Name = "تعداد موجودی")]
        [DisplayName("تعداد موجودی")]
        public Nullable<int> existingCount { get; set; }
        public Nullable<long> visit { get; set; }
        [Display(Name = "ویترین")]
        [DisplayName("ویترین")]
        public Nullable<bool> vitrin { get; set; }
    }
    [MetadataType(typeof(MetaProduct))]
    public partial class Product
    {

    }

    internal class MetaProductCategory
    {
        public int categoryID { get; set; }
        [Display(Name = "نام دسته بندی")]
        [DisplayName("نام دسته بندی")]
        [Required(ErrorMessage = "نوشتن این مورد اجباریست")]
        public string categoryName { get; set; }
        [Display(Name = "گروه دسته بندی")]
        [DisplayName("گروه دسته بندی")]
        public Nullable<int> parentCategory { get; set; }
    }
    [MetadataType(typeof(MetaProductCategory))]
    public partial class ProductCategory
    {
    }
    internal class MetaProductState
    {
        public int productStateID { get; set; }
        [Display(Name = "وضعیت")]
        [DisplayName("وضعیت")]
        public string state { get; set; }
    }
    [MetadataType(typeof(MetaProductState))]
    public partial class ProductState
    {
    }
    internal class MetaQuestion
    {
        public int questionID { get; set; }
        [Display(Name = "سوال")]
        [DisplayName("سوال")]
        public string questionText { get; set; }
        [Display(Name = "تاریخ")]
        [DisplayName("تاریخ")]
        public Nullable<System.DateTime> questionDate { get; set; }
        public Nullable<int> productID { get; set; }
        public Nullable<int> userID { get; set; }
    }
    [MetadataType(typeof(MetaQuestion))]
    public partial class Question
    {
    }
    internal class MetaState
    {
        public int stateID { get; set; }
        [Display(Name = "استان")]
        [DisplayName("استان")]
        public string stateName { get; set; }

    }
    [MetadataType(typeof(MetaState))]
    public partial class State
    {
    }

    internal class MetaUser
    {
        public int userID { get; set; }
        [Display(Name = "نام کاربری")]
        [DisplayName("نام کاربری")]
        public string username { get; set; }
        [Display(Name = "رمز عبور")]
        [DisplayName("رمز عبور")]
        public string password { get; set; }
        [Display(Name = "نام")]
        [DisplayName("نام")]
        public string name { get; set; }
        [Display(Name = "موبایل")]
        [DisplayName("موبایل")]
        public string mobile { get; set; }
        public Nullable<int> usergroupID { get; set; }
        [Display(Name = "ایمیل")]
        [DisplayName("ایمیل")]
        public string email { get; set; }
        [Display(Name = "جنسیت")]
        [DisplayName("جنسیت")]
        public Nullable<bool> sex { get; set; }
    }
    [MetadataType(typeof(MetaUser))]
    public partial class User
    {
        public object UserInfo { get; internal set; }
        public string confirmPassword { get; set; }
    }
    internal class MetaUserGroup
    {
        public int usergroupID { get; set; }
        [Display(Name = "گروه")]
        [DisplayName("گروه")]
        public string groupName { get; set; }
    }
    [MetadataType(typeof(MetaUserGroup))]
    public partial class UserGroup
    {
    }

    internal class MetaAttribteValueType
    {
        public byte attributeValueTypeID { get; set; }
        [Display(Name = "نوع مقدار")]
        [DisplayName("نوع مقدار")]
        public string valueType { get; set; }
    }
    [MetadataType(typeof(MetaAttribteValueType))]
    public partial class AttribteValueType
    {
    }

    internal class MetaProductAttribute
    {
        [Display(Name = " مقدار")]
        [DisplayName("مقدار")]
        public string value { get; set; }

    }
    [MetadataType(typeof(MetaProductAttribute))]
    public partial class ProductAttribute
    {
    }

    internal class MetaProductAttributeTemplate
    {
        public long productAtrributeTemplateID { get; set; }
        [Display(Name = "عنوان")]
        [DisplayName("عنوان")]
        public string name { get; set; }
        public Nullable<byte> attributeValueTypeID { get; set; }
        public Nullable<int> unitID { get; set; }
        [Display(Name = "قابل‌جستجو")]
        [DisplayName("قابل‌جستجو")]
        public Nullable<bool> searchable { get; set; }
        public Nullable<int> categoryID { get; set; }

    }
    [MetadataType(typeof(MetaProductAttributeTemplate))]
    public partial class ProductAttributeTemplate
    {
    }

    internal class MetaUnit
    {
        public int unitID { get; set; }
        [Display(Name = "واحد اندازه‌گیری")]
        [DisplayName("واحد اندازه‌گیری")]
        public string unitName { get; set; }
    }
    [MetadataType(typeof(MetaUnit))]
    public partial class Unit
    {
    }

    internal class MetaBanner
    {
        public int bannerID { get; set; }
        [Display(Name = "مسیر")]
        [DisplayName("مسیر")]
        public string url { get; set; }
    }
    [MetadataType(typeof(MetaBanner))]
    public partial class Banner
    {
    }

    internal class MetaShopCarousel
    {
        public int shopCarouselID { get; set; }
        [Display(Name = "نمایش")]
        [DisplayName("نمایش")]
        public Nullable<bool> show { get; set; }
    }
    [MetadataType(typeof(MetaShopCarousel))]
    public partial class ShopCarousel
    {
    }

    internal class MetaContent
    {
        public int contentID { get; set; }
        [Display(Name = "محتوی")]
        [DisplayName("محتوی")]
        public string employeText { get; set; }
        [Display(Name = "درباره ما")]
        [DisplayName("درباره ما")]
        public string aboutUs { get; set; }
        [Display(Name = "عنوان")]
        [DisplayName("عنوان")]
        public string title { get; set; }
        [Display(Name = "چرا ما")]
        [DisplayName("چرا ما")]
        public string whyUs { get; set; }
        [Display(Name = "متن کوتاه")]
        [DisplayName("متن کوتاه")]
        public string shortDescription { get; set; }
        [Display(Name = "سیاست های کاری")]
        [DisplayName("سیاست های کاری")]
        public string policy { get; set; }
        [Display(Name = "قوانین کاری")]
        [DisplayName("قوانین کاری")]
        public string rules { get; set; }
        [Display(Name = "آدرس")]
        [DisplayName("آدرس")]
        public string adress { get; set; }
        [Display(Name = "شماره تماس")]
        [DisplayName("شماره تماس")]
        public string phones { get; set; }
        [Display(Name = "ایمیل")]
        [DisplayName("ایمیل")]
        public string email { get; set; }
        [Display(Name = "تماس با ما")]
        [DisplayName("تماس با ما")]
        public string contactUs { get; set; }
        
    }
    [MetadataType(typeof(MetaContent))]
    public partial class Content
    {

    }

    internal class MetaProductColor
    {
        public int productColorID { get; set; }
        [Display(Name = "کد‌رنگ")]
        [DisplayName("کد‌رنگ")]
        public string colorCode { get; set; }
        [Display(Name = "نام‌رنگ")]
        [DisplayName("نام‌رنگ")]
        public string color { get; set; }
        [Display(Name = "موجود")]
        [DisplayName("موجود")]
        public Nullable<bool> exist { get; set; }
    }
    [MetadataType(typeof(MetaProductColor))]
    public partial class ProductColor
    { }

    internal class MetaProductAttributeTemplateGroup
    {
        public int ProductAttributeTemplateGroupID { get; set; }
        [Display(Name = "نام")]
        [DisplayName("نام")]
        public string name { get; set; }
        public Nullable<int> categoryID { get; set; }
    }
    [MetadataType(typeof(MetaProductAttributeTemplateGroup))]
    public partial class ProductAttributeTemplateGroup
    {}







    internal class MetaSocial
    {
        public int socialID { get; set; }

        [Display(Name = "نام‌")]
        [DisplayName("نام‌")]
        public string type { get; set; }
        [Display(Name = "آیکون شبکه اجتماعی")]
        [DisplayName("آیکون شبکه اجتماعی")]
        public string socialIcon { get; set; }
        [Display(Name = "لینک")]
        [DisplayName("لینک")]
        public string url { get; set; }
        
        public Nullable<int> contentID { get; set; }
        public Nullable<int> employeID { get; set; }


    }
    [MetadataType(typeof(MetaSocial))]
    public partial class Social
    { }
    internal class MetaStore
    {
        public int storeID { get; set; }
        [Display(Name = "نام انبار")]
        [DisplayName("نام انبار")]
        public string name { get; set; }
        [Display(Name = "تلفن")]
        [DisplayName("تلفن")]
        public string phone { get; set; }
        [Display(Name = "موبایل")]
        [DisplayName("موبایل")]
        public string mobile { get; set; }
        [Display(Name = "آدرس")]
        [DisplayName("آدرس")]
        public string address { get; set; }
        [Display(Name = "فعال")]
        [DisplayName("فعال")]
        public Nullable<bool> active { get; set; }
        public Nullable<int> cityID { get; set; }
        public Nullable<int> userID { get; set; }
    }
    [MetadataType(typeof(MetaStore))]
    public partial class Store
    {
    }
    internal class MetaInvoiceType
    {
        public int invoiceTypeID { get; set; }
        [Display(Name = "نام")]
        [DisplayName("نام")]
        public string name { get; set; }
    }
    [MetadataType(typeof(MetaInvoiceType))]
    public partial class InvoiceType
    {
    }
    internal class MetaInvoiceDetail
    {
        public int invoiceDetailID { get; set; }
        [DisplayName("تعداد")]
        [Display(Name ="تعداد")]
        public Nullable<int> count { get; set; }
        public Nullable<int> invoiceID { get; set; }
        public Nullable<int> productColorID { get; set; }
        public Nullable<int> productID { get; set; }
        public Nullable<int> increaseID { get; set; }
        public Nullable<int> decreaseID { get; set; }
        public Nullable<int> orderID { get; set; }
        
    }
    [MetadataType(typeof(MetaInvoiceDetail))]
    public partial class InvoiceDetail
    {
    }
    internal class MetaInvoice
    {

        public int invoiceID { get; set; }
        [DisplayName("توضیحات")]
        [Display(Name = "توضیحات")]
        public string description { get; set; }
        [DisplayName("تاریخ")]
        [Display(Name = "تاریخ")]
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> invoiceTypeID { get; set; }
        public Nullable<int> userID { get; set; }
        [JsonIgnore]
        public virtual InvoiceType InvoiceType { get; set; }


    }
    [MetadataType(typeof(MetaInvoice))]
    public partial class Invoice
    {
    }
    internal class MetaGuaranteeCustomer
    {
        public int guaranteeCustomerID { get; set; }
        [Display(Name = "نام مشتری")]
        [DisplayName("نام مشتری")]
        public string name { get; set; }
        [Display(Name = "کد ملی")]
        [DisplayName("کد ملی")]
        public string nationalCode { get; set; }
        [Display(Name = "شماره های تماس")]
        [DisplayName("شماره های تماس")]
        public string phone { get; set; }
        [Display(Name = "موبایل")]
        [DisplayName("موبایل")]
        public string mobile { get; set; }
        [Display(Name = "نام شرکت")]
        [DisplayName("نام شرکت")]
        public string companyName { get; set; }
    }
    [MetadataType(typeof(MetaGuaranteeCustomer))]
    public partial class GuaranteeCustomer
    {
    }
    internal class MetaGuaranteeProduct
    {
        public int guaranteeProductID { get; set; }
        [Display(Name = " نام کالا")]
        [DisplayName("نام کالا")]
        public string productName { get; set; }
        [Display(Name = "مدل دستگاه")]
        [DisplayName("مدل دستگاه")]
        public string model { get; set; }
        [Display(Name = "سریال")]
        [DisplayName("سریال")]
        public string serialNumber { get; set; }
        public Nullable<int> guaranteeStateID { get; set; }
        [Display(Name = "تاریخ خرید")]
        [DisplayName("تاریخ خرید")]
        public Nullable<System.DateTime> buyTime { get; set; }
        [Display(Name = "تاریخ تحویل به شرکت")]
        [DisplayName("تاریخ تحویل به شرکت")]
        public Nullable<System.DateTime> deliveryDate { get; set; }
        [Display(Name = "تاریخ آماده تحویل ")]
        [DisplayName("تاریخ آماده تحویل")]
        public Nullable<System.DateTime> doneDate { get; set; }
        [Display(Name = "تاریخ تحویل به مشتری ")]
        [DisplayName("تاریخ تحویل به مشتری")]
        public Nullable<System.DateTime> customerDeliveryDate { get; set; }
        [Display(Name = "توضیحات")]
        [DisplayName("توضیحات")]
        public string description { get; set; }
        [Display(Name = "نتیجه")]
        [DisplayName("نتیجه")]
        public string result { get; set; }
        [Display(Name = "تاریخ تایید")]
        [DisplayName("تاریخ تایید")]
        public DateTime? confirmDate { get; set; }
        public Nullable<int> guaranteeServiceCompanyID { get; set; }
        public Nullable<int> guaranteeCustomerID { get; set; }
        public Nullable<int> userID { get; set; }
        [Display(Name = "اظهارات مشتری")]
        [DisplayName("اظهارات مشتری")]
        public string customerOpinion { get; set; }
        [Display(Name = "ایرادات مشاهده شده")]
        [DisplayName("ایرادات مشاهده شده")]
        public string repairManOpinion { get; set; }
        [Display(Name = "متعلقات دستگاه")]
        [DisplayName("متعلقات دستگاه")]
        public string include { get; set; }
        [Display(Name = "ایرادات فیزیکی")]
        [DisplayName("ایرادات فیزیکی")]
        public string physicalProblems { get; set; }
        [Display(Name = "شماره اموال")]
        [DisplayName("شماره اموال")]
        public Nullable<int> amvalNumber { get; set; }
        [Display(Name = "کد رهگیری")]
        [DisplayName("کد رهگیری")]
        public Nullable<int> trackingCode { get; set; }
    }
    [MetadataType(typeof(MetaGuaranteeProduct))]
    public partial class GuaranteeProduct
    {
    }
    internal class MetaGuaranteeServiceCompany
    {
        public int guaranteeServiceCompanyID { get; set; }
        [Display(Name = "نام شرکت")]
        [DisplayName("نام شرکت")]
        public string companyName { get; set; }
    }
    [MetadataType(typeof(MetaGuaranteeServiceCompany))]
    public partial class GuaranteeServiceCompany
    {
    }

    internal class MetaGuaranteeState
    {
        public int guaranteeStateID { get; set; }
        [Display(Name = "وضعیت")]
        [DisplayName("وضعیت")]
        public string stateName { get; set; }
    }
    [MetadataType(typeof(MetaGuaranteeState))]
    public partial class GuaranteeState
    {
    }

    internal class MetaGuaranteeExtraItem
    {
        public int guaranteeExtraItemID { get; set; }
        [Display(Name = "نام وسیله جانبی")]
        [DisplayName("وضعیت")]
        public string itemName { get; set; }
    }
    [MetadataType(typeof(MetaGuaranteeExtraItem))]
    public partial class GuaranteeExtraItem
    {
    }
}