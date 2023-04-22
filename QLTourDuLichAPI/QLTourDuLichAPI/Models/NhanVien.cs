using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class NhanVien
{
    public string GioiTinh { get; set; } = null!;

    public string MaNv { get; set; } = null!;

    public string TenNv { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<TinTuc> TinTucs { get; set; } = new List<TinTuc>();

    public virtual TaiKhoan UserNameNavigation { get; set; } = null!;
}
