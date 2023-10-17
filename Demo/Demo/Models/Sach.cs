using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class Sach
    {
        public Sach()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        public int MaTs { get; set; }
        public string TenTs { get; set; } = null!;
        public int? MaTl { get; set; }
        public int? Gia { get; set; }
        public int? SoLuong { get; set; }
        public DateTime? NamXb { get; set; }
        public int? MaNsx { get; set; }
        public int? MaKho { get; set; }
        public string? Anh { get; set; }

        public virtual Kho? MaKhoNavigation { get; set; }
        public virtual Nsx? MaNsxNavigation { get; set; }
        public virtual TheLoai? MaTlNavigation { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
