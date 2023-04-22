using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLTourDuLichAPI.Models;

public partial class QltourdlApiContext : DbContext
{
    public QltourdlApiContext()
    {
    }

    public QltourdlApiContext(DbContextOptions<QltourdlApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnhTin> AnhTins { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<DiaDiemTour> DiaDiemTours { get; set; }

    public virtual DbSet<DiemThamQuan> DiemThamQuans { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhachSan> KhachSans { get; set; }

    public virtual DbSet<KhachSanTour> KhachSanTours { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TinTuc> TinTucs { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-J7EGSMK;Initial Catalog=QLTOURDL_API;Integrated Security=True;TrustServerCertificate=True");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnhTin>(entity =>
        {
            entity.HasKey(e => e.TenFileAnh).HasName("PK__AnhTin__8E7F36218094488E");

            entity.ToTable("AnhTin");

            entity.Property(e => e.TenFileAnh).HasMaxLength(50);
            entity.Property(e => e.MaTin).HasMaxLength(50);
            entity.Property(e => e.ViTri).HasMaxLength(50);

            entity.HasOne(d => d.MaTinNavigation).WithMany(p => p.AnhTins)
                .HasForeignKey(d => d.MaTin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AnhTin__MaTin__412EB0B6");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.MaBk).HasName("PK__Booking__272475AE08C3E86F");

            entity.ToTable("Booking");

            entity.Property(e => e.MaBk)
                .HasMaxLength(50)
                .HasColumnName("MaBK");
            entity.Property(e => e.MaKh)
                .HasMaxLength(50)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.MaTour).HasMaxLength(50);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__MaKH__3D5E1FD2");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__MaNV__3E52440B");

            entity.HasOne(d => d.MaTourNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.MaTour)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__MaTour__3C69FB99");
        });

        modelBuilder.Entity<DiaDiemTour>(entity =>
        {
            entity.HasKey(e => new { e.MaDd, e.MaTour }).HasName("PK__DiaDiemT__D3C0D3187FBD39CA");

            entity.ToTable("DiaDiemTour");

            entity.Property(e => e.MaDd)
                .HasMaxLength(50)
                .HasColumnName("MaDD");
            entity.Property(e => e.MaTour).HasMaxLength(50);
            entity.Property(e => e.ViTriAnh).HasMaxLength(50);

            entity.HasOne(d => d.MaDdNavigation).WithMany(p => p.DiaDiemTours)
                .HasForeignKey(d => d.MaDd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiaDiemTou__MaDD__31EC6D26");

            entity.HasOne(d => d.MaTourNavigation).WithMany(p => p.DiaDiemTours)
                .HasForeignKey(d => d.MaTour)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiaDiemTo__MaTou__32E0915F");
        });

        modelBuilder.Entity<DiemThamQuan>(entity =>
        {
            entity.HasKey(e => e.MaDd).HasName("PK__DiemTham__27258665AE9F24E4");

            entity.ToTable("DiemThamQuan");

            entity.Property(e => e.MaDd)
                .HasMaxLength(50)
                .HasColumnName("MaDD");
            entity.Property(e => e.Mien).HasMaxLength(50);
            entity.Property(e => e.MoTa).HasMaxLength(4000);
            entity.Property(e => e.TenFileAnh).HasMaxLength(50);
            entity.Property(e => e.Tendiadiem).HasMaxLength(50);
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1EE15875AC");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(50)
                .HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .HasColumnName("SDT");
            entity.Property(e => e.TenKh)
                .HasMaxLength(50)
                .HasColumnName("TenKH");
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.UserNameNavigation).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.UserName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachHang__UserN__2C3393D0");
        });

        modelBuilder.Entity<KhachSan>(entity =>
        {
            entity.HasKey(e => e.MaKs).HasName("PK__KhachSan__2725CF13FA8E647C");

            entity.ToTable("KhachSan");

            entity.Property(e => e.MaKs)
                .HasMaxLength(50)
                .HasColumnName("MaKS");
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .HasColumnName("SDT");
            entity.Property(e => e.TenFileAnh).HasMaxLength(50);
            entity.Property(e => e.TenKs)
                .HasMaxLength(50)
                .HasColumnName("TenKS");
            entity.Property(e => e.XepHangKs).HasColumnName("XepHang_KS");
        });

        modelBuilder.Entity<KhachSanTour>(entity =>
        {
            entity.HasKey(e => new { e.MaKs, e.MaTour }).HasName("PK__KhachSan__D3C09A6E47B9F213");

            entity.ToTable("KhachSanTour");

            entity.Property(e => e.MaKs)
                .HasMaxLength(50)
                .HasColumnName("MaKS");
            entity.Property(e => e.MaTour).HasMaxLength(50);
            entity.Property(e => e.ViTriAnh).HasMaxLength(50);

            entity.HasOne(d => d.MaKsNavigation).WithMany(p => p.KhachSanTours)
                .HasForeignKey(d => d.MaKs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachSanTo__MaKS__35BCFE0A");

            entity.HasOne(d => d.MaTourNavigation).WithMany(p => p.KhachSanTours)
                .HasForeignKey(d => d.MaTour)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachSanT__MaTou__36B12243");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70AD89299C6");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai).HasMaxLength(50);
            entity.Property(e => e.TenNv)
                .HasMaxLength(50)
                .HasColumnName("TenNV");
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.UserNameNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.UserName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanVien__UserNa__2F10007B");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.UserName).HasName("PK__TaiKhoan__C9F28457991D90E3");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.ConfirmPassword).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<TinTuc>(entity =>
        {
            entity.HasKey(e => e.MaTin).HasName("PK__TinTuc__3149033588E753C6");

            entity.ToTable("TinTuc");

            entity.Property(e => e.MaTin).HasMaxLength(50);
            entity.Property(e => e.Anh).HasMaxLength(50);
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.MoTa)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.NoiDung)
                .HasMaxLength(4000)
                .IsUnicode(false);

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.TinTucs)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TinTuc__MaNV__398D8EEE");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.MaTour).HasName("PK__Tour__4E5557DE6F765444");

            entity.ToTable("Tour");

            entity.Property(e => e.MaTour).HasMaxLength(50);
            entity.Property(e => e.Anh).HasMaxLength(50);
            entity.Property(e => e.DiemXuatPhat).HasMaxLength(50);
            entity.Property(e => e.SoNgayDl).HasColumnName("SoNgayDL");
            entity.Property(e => e.TenTour).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
