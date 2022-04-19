using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineAds.Models
{
    public partial class TblProduct
    {
        public int ProId { get; set; }
        public string ProName { get; set; }
        public string ProImage { get; set; }
        public string ProDes { get; set; }
        public int? ProPrice { get; set; }
        public int? ProFkCat { get; set; }
        public int? ProFkUser { get; set; }

        public virtual TblCategory ProFkCatNavigation { get; set; }
        public virtual TblUser ProFkUserNavigation { get; set; }
    }
}
