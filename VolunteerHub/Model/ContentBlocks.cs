//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VolunteerHub.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ContentBlocks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContentBlocks()
        {
            this.NewsImages = new HashSet<NewsImages>();
        }
    
        public int ContentBlockID { get; set; }
        public Nullable<int> NewsID { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        public Nullable<int> OrderIndex { get; set; }
    
        public virtual News News { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsImages> NewsImages { get; set; }
    }
}
