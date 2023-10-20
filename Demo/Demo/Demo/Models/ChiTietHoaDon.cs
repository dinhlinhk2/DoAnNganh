using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class ChiTietHoaDon
    {
        public int MaTs { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }
        public int MaHd { get; set; }
        public int MaCthd { get; set; }

        public virtual HoaDon MaHdNavigation { get; set; } = null!;
        public virtual Sach MaTsNavigation { get; set; } = null!;
    }
}
