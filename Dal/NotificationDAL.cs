using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace WareHouse22.Models
{
    public partial class NotificationDAL : DbContext
    {
        public NotificationDAL()
            : base("name=Database")
        {
        }

        public virtual DbSet<tblNotification> Notification { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
