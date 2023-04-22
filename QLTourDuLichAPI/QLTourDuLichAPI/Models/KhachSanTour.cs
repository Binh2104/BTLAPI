using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class KhachSanTour
{
    public string? ViTriAnh { get; set; }

    public string MaKs { get; set; } = null!;

    public string MaTour { get; set; } = null!;

    public virtual KhachSan MaKsNavigation { get; set; } = null!;

    public virtual Tour MaTourNavigation { get; set; } = null!;
}
