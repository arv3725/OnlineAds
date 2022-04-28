using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace OnlineAds.Models
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblProducts = new HashSet<TblProduct>();
        }

        public int CatId { get; set; }
        [Required(ErrorMessage = "Category Name required")]
        public string CatName { get; set; }
        [Required(ErrorMessage = "Image required")]
        public string CatImage { get; set; }
        public int? CatFkAd { get; set; }
        public int? CatStatus { get; set; }

        public virtual TblAdmin CatFkAdNavigation { get; set; }
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
