using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class KhachHang
{
    public DateOnly NgaySinh { get; set; }

    public string Sdt { get; set; } = null!;

    public string MaKh { get; set; } = null!;

    public string TenKh { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public string GioiTinh { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual TaiKhoan UserNameNavigation { get; set; } = null!;
}
