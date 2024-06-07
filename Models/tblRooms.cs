namespace WareHouse22.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRooms
    {
        [StringLength(30)]
        public string UserEmail { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(60)]
        public string ItemName { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime Date { get; set; }
    }
}
