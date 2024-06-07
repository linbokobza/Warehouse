using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WareHouse22.Models
{
    public partial class LoansDAL : DbContext
    {
        public LoansDAL()
            : base("name=Database")
        {
        }

        public virtual DbSet<tblLoans> Loans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
