namespace WareHouse22.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblNotification
    {
        [Key]
        public int Snumber { get; set; }

        [StringLength(200)]
        public string Body { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public DateTime Date { get; set; }
    }
}
