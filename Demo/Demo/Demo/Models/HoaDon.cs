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
        public int? TrangThaiId { get; set; }
        public bool Deleted { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaymentDate { get; set; }

        public virtual KhachHang? MaDgNavigation { get; set; }
        public virtual TrangThaiDon? TrangThai { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
