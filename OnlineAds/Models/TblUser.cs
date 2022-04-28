using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace OnlineAds.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblProducts = new HashSet<TblProduct>();
        }
        [Required]
        public int UId { get; set; }
        [Required]
        public string UName { get; set; }
        [Required]
        public DateTime UDob { get; set; }
        [Required]
        public string UGender { get; set; }
        [Required]
        public string UCity { get; set; }
        [Required]
        public string UState { get; set; }
        [Required]
        public string UEmail { get; set; }
        [Required]
        public string UPassword { get; set; }
        public string UImage { get; set; }
        [Required]
        public string UContact { get; set; }

        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
