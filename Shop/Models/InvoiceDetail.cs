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
    
    public partial class InvoiceDetail
    {
        public int invoiceDetailID { get; set; }
        public Nullable<int> count { get; set; }
        public Nullable<int> invoiceID { get; set; }
        public Nullable<int> productColorID { get; set; }
        public Nullable<int> productID { get; set; }
        public Nullable<int> increaseID { get; set; }
        public Nullable<int> decreaseID { get; set; }
        public Nullable<int> orderID { get; set; }
    
        public virtual Invoice Invoice { get; set; }
        public virtual Orde Orde { get; set; }
        public virtual ProductColor ProductColor { get; set; }
        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
        public virtual Store Store1 { get; set; }
    }
}