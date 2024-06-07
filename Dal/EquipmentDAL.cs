using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WareHouse22.Models
{
    public partial class EquipmentDAL : DbContext
    {
        public EquipmentDAL()
            : base("name=Database")
        {
        }

        public  DbSet<tblEquipment> Equipment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
