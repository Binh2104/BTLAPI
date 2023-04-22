using QLTourDuLichAPI.Models;
namespace QLTourDuLichAPI.InputModelsApi
{
    public class TinTucCreateInputMode
    {
        public string MaTin { get; set; } = null!;

        public IFormFile? Anh { get; set; } = null!;

        public string MoTa { get; set; } = null!;

        public string NoiDung { get; set; } = null!;

        public string MaNv { get; set; } = null!;
    }
}
