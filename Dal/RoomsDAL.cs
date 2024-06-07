using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WareHouse22.Models
{
    public partial class RoomsDAL : DbContext
    {
        public RoomsDAL()
            : base("name=Database")
        {
        }

        public virtual DbSet<tblRooms> Rooms{ get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
