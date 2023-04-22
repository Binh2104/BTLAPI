using System;
using System.Collections.Generic;

namespace QLTourDuLichAPI.Models;

public partial class DiaDiemTour
{
    public string MaDd { get; set; } = null!;

    public string ViTriAnh { get; set; } = null!;

    public string MaTour { get; set; } = null!;

    public virtual DiemThamQuan MaDdNavigation { get; set; } = null!;

    public virtual Tour MaTourNavigation { get; set; } = null!;
}
