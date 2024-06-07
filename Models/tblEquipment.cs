namespace WareHouse22.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblEquipment")]
    public partial class tblEquipment
    {
        [Key]
        public string Code { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(20)]
        public string State { get; set; }

        [StringLength(30)]
        public string Category { get; set; }

        public int? MaxLoanTime { get; set; }

        [StringLength(1000)]
        public string Instruction { get; set; }
    }
}
