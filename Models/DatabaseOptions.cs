using System;
using System.Collections.Generic;
using System.Text;

namespace SQLstudio.Models
{
    public sealed record DatabaseOptions(
        string Id,
        string DisplayName,
        string IconPath
        );
}
