using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class HoaDon
    {
        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        public int MaHd { get; set; }
        public DateTime NgayLapHd { get; set; }
        public int TongGiaTri { get; set; }
        public int MaDg { get; set; }

        public virtual KhachHang MaDgNavigation { get; set; } = null!;
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
