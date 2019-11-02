//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Atiran.DataLayer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            this.UserFavorites = new HashSet<UserFavorite>();
        }
    
        public int MenuID { get; set; }
        public Nullable<int> SubSystemID { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentMenuID { get; set; }
        public Nullable<int> FormID { get; set; }
        public Nullable<int> order { get; set; }
        public string Shortcut { get; set; }
    
        public virtual Form Form { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFavorite> UserFavorites { get; set; }
    }
}
