using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Demo.Models
{
    public partial class WebBanSachOnlContext : DbContext
    {
        public WebBanSachOnlContext()
        {
        }

        public WebBanSachOnlContext(DbContextOptions<WebBanSachOnlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; } = null!;
        public virtual DbSet<HoaDon> HoaDons { get; set; } = null!;
        public virtual DbSet<KhachHang> KhachHangs { get; set; } = null!;
        public virtual DbSet<Kho> Khos { get; set; } = null!;
        public virtual DbSet<Nsx> Nsxes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Sach> Saches { get; set; } = null!;
        public virtual DbSet<TheLoai> TheLoais { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-2CMQROFJ\\SQL;Database=WebBanSachOnl;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccountID");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.MatKhau).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Salt)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Accounts_Roles");
            });

            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasKey(e => e.MaCthd);

                entity.ToTable("ChiTietHoaDon");

                entity.Property(e => e.MaCthd).HasColumnName("MaCTHD");

                entity.Property(e => e.MaHd).HasColumnName("MaHD");

                entity.Property(e => e.MaTs).HasColumnName("MaTS");

                entity.HasOne(d => d.MaHdNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaHd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietHoaDon_HoaDon");

                entity.HasOne(d => d.MaTsNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaTs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietHoa__MaTS__38996AB5");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHd)
                    .HasName("PK__HoaDon__2725A6E05D495D16");

                entity.ToTable("HoaDon");

                entity.Property(e => e.MaHd).HasColumnName("MaHD");

                entity.Property(e => e.MaDg).HasColumnName("MaDG");

                entity.Property(e => e.NgayLapHd)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayLap_HD");

                entity.HasOne(d => d.MaDgNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaDg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoaDon__MaDG__35BCFE0A");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaDg)
                    .HasName("PK__KhachHan__2725866083EF03D7");

                entity.ToTable("KhachHang");

                entity.Property(e => e.MaDg).HasColumnName("MaDG");

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.GioiTinh)
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.MatKhau).HasMaxLength(50);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.NgayTao).HasColumnType("date");

                entity.Property(e => e.Salt)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Sdt).HasColumnName("SDT");

                entity.Property(e => e.TaiKhoan).HasMaxLength(50);

                entity.Property(e => e.TenDg)
                    .HasMaxLength(80)
                    .HasColumnName("TenDG");
            });

            modelBuilder.Entity<Kho>(entity =>
            {
                entity.HasKey(e => e.MaKho)
                    .HasName("PK__Kho__3BDA9350C7D609B6");

                entity.ToTable("Kho");

                entity.Property(e => e.MaKho).ValueGeneratedNever();

                entity.Property(e => e.TenSach).HasMaxLength(50);
            });

            modelBuilder.Entity<Nsx>(entity =>
            {
                entity.HasKey(e => e.MaNsx)
                    .HasName("PK__NSX__3A1BDBD2E726245F");

                entity.ToTable("NSX");

                entity.Property(e => e.MaNsx)
                    .ValueGeneratedNever()
                    .HasColumnName("MaNSX");

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Sdt).HasColumnName("SDT");

                entity.Property(e => e.TenNsx)
                    .HasMaxLength(100)
                    .HasColumnName("TenNSX");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleID");

                entity.Property(e => e.Description).HasMaxLength(20);

                entity.Property(e => e.RoleName)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Sach>(entity =>
            {
                entity.HasKey(e => e.MaTs)
                    .HasName("PK__Sach__272500785894B950");

                entity.ToTable("Sach");

                entity.Property(e => e.MaTs)
                    .ValueGeneratedNever()
                    .HasColumnName("MaTS");

                entity.Property(e => e.MaNsx).HasColumnName("MaNSX");

                entity.Property(e => e.MaTl).HasColumnName("MaTL");

                entity.Property(e => e.NamXb)
                    .HasColumnType("date")
                    .HasColumnName("NamXB");

                entity.Property(e => e.TenTs)
                    .HasMaxLength(100)
                    .HasColumnName("TenTS");

                entity.HasOne(d => d.MaKhoNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.MaKho)
                    .HasConstraintName("FK_Sach_Kho");

                entity.HasOne(d => d.MaNsxNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.MaNsx)
                    .HasConstraintName("FK_Sach_NSX");

                entity.HasOne(d => d.MaTlNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.MaTl)
                    .HasConstraintName("FK_Sach_TheLoai");
            });

            modelBuilder.Entity<TheLoai>(entity =>
            {
                entity.HasKey(e => e.MaTl)
                    .HasName("PK__TheLoai__27250071E41F4924");

                entity.ToTable("TheLoai");

                entity.Property(e => e.MaTl)
                    .ValueGeneratedNever()
                    .HasColumnName("MaTL");

                entity.Property(e => e.TenTl)
                    .HasMaxLength(50)
                    .HasColumnName("TenTL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
