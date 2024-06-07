namespace WareHouse22.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblUsers
    {
        [Key]
        [StringLength(30)]
        public string Email { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(2)]
        public string Role { get; set; }
        
    }
}
