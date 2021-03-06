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
    
    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            this.Ordes = new HashSet<Orde>();
        }
    
        public int addressID { get; set; }
        public string addressDetail { get; set; }
        public string postalCode { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> cityID { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string location { get; set; }
        public Nullable<bool> active { get; set; }
    
        public virtual City City { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orde> Ordes { get; set; }
    }
}
