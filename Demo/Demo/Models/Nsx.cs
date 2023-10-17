using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class Nsx
    {
        public Nsx()
        {
            Saches = new HashSet<Sach>();
        }

        public int MaNsx { get; set; }
        public string TenNsx { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public int Sdt { get; set; }
        public string Email { get; set; } = null!;

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
