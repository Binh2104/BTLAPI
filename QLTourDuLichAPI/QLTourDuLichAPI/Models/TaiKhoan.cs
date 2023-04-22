using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class TaiKhoan
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Loai { get; set; }

    public string ConfirmPassword { get; set; } = null!;

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
