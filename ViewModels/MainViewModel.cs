using CommunityToolkit.Mvvm.ComponentModel;
using System.Security;

namespace SQLstudio.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";

    [ObservableProperty]
    public partial bool IsAutoCommitEnabled { get; set; } = true;
}
