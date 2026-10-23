using CommunityToolkit.Mvvm.ComponentModel;
using System.Security;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace SQLstudio.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    [ObservableProperty]
    public partial bool IsAutoCommitEnabled { get; set; } = true;

    public ObservableCollection<string> DatabasesNames { get; } = new();

    public void LoadDatabases(IEnumerable<string> names)
    {
        DatabasesNames.Clear();

        foreach (var name in names)
        {
            DatabasesNames.Add(name);
        }

    }
}
