using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class TrangThaiDon
    {
        public TrangThaiDon()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        public int TrangThaiId { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
