﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class VetenarianDBEntities1 : DbContext
    {
        public VetenarianDBEntities1()
            : base("name=VetenarianDBEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<consultation> consultations { get; set; }
        public virtual DbSet<job> jobs { get; set; }
        public virtual DbSet<pet> pets { get; set; }
        public virtual DbSet<petOwner> petOwners { get; set; }
        public virtual DbSet<petSpecy> petSpecies { get; set; }
    }
}
