//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LoginRegistrationInMVCwithDatabase.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LoginRegistrationInMVCEntities1 : DbContext
    {
        public LoginRegistrationInMVCEntities1()
            : base("name=LoginRegistrationInMVCEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product_> Product_ { get; set; }
        public virtual DbSet<RegisterUser> RegisterUsers { get; set; }
        public virtual DbSet<Sub_Category> Sub_Category { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
