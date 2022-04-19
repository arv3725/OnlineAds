using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineAds.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblProducts = new HashSet<TblProduct>();
        }

        public int UId { get; set; }
        public string UName { get; set; }
        public DateTime UDob { get; set; }
        public string UGender { get; set; }
        public string UCity { get; set; }
        public string UState { get; set; }
        public string UEmail { get; set; }
        public string UPassword { get; set; }
        public string UImage { get; set; }
        public string UContact { get; set; }

        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
