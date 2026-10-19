using Avalonia.Media.Imaging;

namespace SQLstudio.Models
{
    public sealed record DatabaseOptions(
        string Id,
        string DisplayName,
        Bitmap IconPath
        );
}
