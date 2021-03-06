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
    
    public partial class Orde
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orde()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
            this.OrderProducts = new HashSet<OrderProduct>();
        }
    
        public int orderID { get; set; }
        public Nullable<decimal> orderOff { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> addressID { get; set; }
        public Nullable<int> orderStateID { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<byte> paymentTypeID { get; set; }
        public string description { get; set; }
        public Nullable<int> shippingCost { get; set; }
        public string paymentCode { get; set; }
        public string paymentId { get; set; }
        public Nullable<System.DateTime> paymentDate { get; set; }
    
        public virtual Address Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual OrderState OrderState { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual User User { get; set; }
    }
}
