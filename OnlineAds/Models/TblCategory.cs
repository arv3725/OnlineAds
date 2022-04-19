using System;
using System.Collections.Generic;

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
        public string CatName { get; set; }
        public string CatImage { get; set; }
        public int? CatFkAd { get; set; }
        public int? CatStatus { get; set; }

        public virtual TblAdmin CatFkAdNavigation { get; set; }
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
