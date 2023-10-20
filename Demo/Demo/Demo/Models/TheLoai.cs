using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class TheLoai
    {
        public TheLoai()
        {
            Saches = new HashSet<Sach>();
        }

        public int MaTl { get; set; }
        public string TenTl { get; set; } = null!;

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
