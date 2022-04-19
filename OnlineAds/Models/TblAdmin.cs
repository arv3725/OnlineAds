using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineAds.Models
{
    public partial class TblAdmin
    {
        public TblAdmin()
        {
            TblCategories = new HashSet<TblCategory>();
        }

        public int AdId { get; set; }
        public string AdUsername { get; set; }
        public string AdPassword { get; set; }

        public virtual ICollection<TblCategory> TblCategories { get; set; }
    }
}
