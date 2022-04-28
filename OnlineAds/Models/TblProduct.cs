using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace OnlineAds.Models
{
    public partial class TblProduct
    {
        [Required]
        public int ProId { get; set; }
        [Required]
        public string ProName { get; set; }
        [Required]
        public string ProImage { get; set; }
        [Required]
        public string ProDes { get; set; }
        [Required]
        public int? ProPrice { get; set; }
        public int? ProFkCat { get; set; }
        public int? ProFkUser { get; set; }

        public virtual TblCategory ProFkCatNavigation { get; set; }
        public virtual TblUser ProFkUserNavigation { get; set; }
    }
}
