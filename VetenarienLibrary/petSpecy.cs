//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VetenarienLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class petSpecy
    {
        public petSpecy()
        {
            this.pets = new HashSet<pet>();
        }
    
        public int petSpecies_ID { get; set; }
        public string name { get; set; }
        public Nullable<int> checkupTime { get; set; }
    
        public virtual ICollection<pet> pets { get; set; }
    }
}
