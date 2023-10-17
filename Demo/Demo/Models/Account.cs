using System;
using System.Collections.Generic;

namespace Demo.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string Phone { get; set; } = null!;
        public string? MatKhau { get; set; } = null!;
        public int? RoleId { get; set; }
        public string? Salt { get; set; }
        public string? Email { get; set; }

        public virtual Role? Role { get; set; }
    }
}
