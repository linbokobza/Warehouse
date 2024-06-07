using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WareHouse22.Models
{
    public partial class UsersDAL : DbContext
    {
        public UsersDAL()
            : base("name=Database")
        {
        }

        public virtual DbSet<tblUsers> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
