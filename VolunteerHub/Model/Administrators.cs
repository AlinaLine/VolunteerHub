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
    
    public partial class Administrators
    {
        public int AdminID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string SpecialPermissions { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
