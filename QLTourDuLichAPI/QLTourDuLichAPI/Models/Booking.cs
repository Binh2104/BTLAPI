using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class Booking
{
    public string MaBk { get; set; } = null!;

    public DateOnly NgayDi { get; set; }

    public int SoChoBooking { get; set; }

    public DateOnly NgayDat { get; set; }

    public string MaTour { get; set; } = null!;

    public string MaKh { get; set; } = null!;

    public string MaNv { get; set; } = null!;

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual NhanVien MaNvNavigation { get; set; } = null!;

    public virtual Tour MaTourNavigation { get; set; } = null!;
}
