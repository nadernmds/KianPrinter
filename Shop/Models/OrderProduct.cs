//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderProduct
    {
        public int orderProductID { get; set; }
        public Nullable<int> count { get; set; }
        public Nullable<int> orderID { get; set; }
        public Nullable<int> productID { get; set; }
        public Nullable<int> productColorID { get; set; }
    
        public virtual Orde Orde { get; set; }
        public virtual ProductColor ProductColor { get; set; }
        public virtual Product Product { get; set; }
    }
}