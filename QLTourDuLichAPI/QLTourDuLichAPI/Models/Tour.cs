using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class Tour
{
    public string MaTour { get; set; } = null!;

    public string TenTour { get; set; } = null!;

    public int GiaCho { get; set; }

    public string Anh { get; set; } = null!;

    public string DiemXuatPhat { get; set; } = null!;

    public int XepHangTour { get; set; }

    public int SoNgayDl { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<DiaDiemTour> DiaDiemTours { get; set; } = new List<DiaDiemTour>();

    public virtual ICollection<KhachSanTour> KhachSanTours { get; set; } = new List<KhachSanTour>();
}
