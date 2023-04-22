using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class TinTuc
{
    public string MaTin { get; set; } = null!;

    public string Anh { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public string MaNv { get; set; } = null!;

    public virtual ICollection<AnhTin> AnhTins { get; set; } = new List<AnhTin>();

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
