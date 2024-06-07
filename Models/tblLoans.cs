namespace WareHouse22.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblLoans
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string UserEmail { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ECode { get; set; }

        [StringLength(30)]
        public string ItemName { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime? StartDate { get; set; }
        [Key]
        [Column(Order = 4)]
        public DateTime? FinalDate { get; set; }

        [StringLength(2)]
        public string Returned { get; set; }
    }
}
