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
    
    public partial class ProductAttributeTemplateGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductAttributeTemplateGroup()
        {
            this.ProductAttributeTemplates = new HashSet<ProductAttributeTemplate>();
        }
    
        public int ProductAttributeTemplateGroupID { get; set; }
        public string name { get; set; }
        public Nullable<int> categoryID { get; set; }
    
        public virtual ProductCategory ProductCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductAttributeTemplate> ProductAttributeTemplates { get; set; }
    }
}
