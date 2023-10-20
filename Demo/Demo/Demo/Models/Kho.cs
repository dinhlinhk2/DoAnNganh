using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class Kho
    {
        public Kho()
        {
            Saches = new HashSet<Sach>();
        }

        public int MaKho { get; set; }
        public string TenSach { get; set; } = null!;
        public int Soluong { get; set; }

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
