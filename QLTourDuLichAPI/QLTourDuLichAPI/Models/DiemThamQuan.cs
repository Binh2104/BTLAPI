using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class DiemThamQuan
{
    public string Tendiadiem { get; set; } = null!;

    public string MaDd { get; set; } = null!;

    public string Mien { get; set; } = null!;

    public string TenFileAnh { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<DiaDiemTour> DiaDiemTours { get; set; } = new List<DiaDiemTour>();
}
