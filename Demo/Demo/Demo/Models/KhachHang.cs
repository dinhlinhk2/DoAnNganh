using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        public int MaDg { get; set; }
        public string? TenDg { get; set; }
        public string? DiaChi { get; set; }
        public int? Sdt { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? TaiKhoan { get; set; }
        public string? MatKhau { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? Salt { get; set; }
        public bool Active { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
