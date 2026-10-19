using SQLstudio.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SQLstudio.ViewModels
{
    public partial class NewConnectionViewModel : ViewModelBase
    {
        public ObservableCollection<DatabaseOptions> Databases { get; } =
            [
                new("postgresql", "PostgreSQL", LoadIcon("barca.png")),
                new("mysql", "MySQL", LoadIcon("barca.png")),
                new("sqlite", "SQLite", LoadIcon("barca.png")),
                new("mariadb", "MariaDB", LoadIcon("barca.png")),
                new("oracle", "Oracle", LoadIcon("barca.png")),
                new("sqlserver", "SQL Server", LoadIcon("barca.png"))
            ];

        public ObservableCollection<string> SqlServerAuthenticationOptions { get; } =
            [
                "SQL Server Authentication",
                "Windows Authentication"
            ];

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextCommand))]
        [NotifyPropertyChangedFor(nameof(ConnectionTitle))]
        [NotifyPropertyChangedFor(nameof(IsSqlite))]
        [NotifyPropertyChangedFor(nameof(IsOracle))]
        [NotifyPropertyChangedFor(nameof(IsSqlServer))]
        [NotifyPropertyChangedFor(nameof(UsesServer))]
        [NotifyPropertyChangedFor(nameof(UsesDatabaseName))]
        [NotifyPropertyChangedFor(nameof(UsesCredentials))]
        [NotifyPropertyChangedFor(nameof(SupportSsl))]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        [NotifyPropertyChangedFor(nameof(IconDb))]
        public partial DatabaseOptions? SelectedDatabase { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSelectingDatabase))]
        [NotifyPropertyChangedFor(nameof(IsConfiguringConnection))]
        private int currentStep;

        public enum ConnectionMode
        {
            Host,
            Url
        };

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsUrlConnection))]
        [NotifyPropertyChangedFor(nameof(IsHostConnection))]
        [NotifyPropertyChangedFor(nameof(ConnectByHost))]
        [NotifyPropertyChangedFor(nameof(ConnectByUrl))]
        [NotifyPropertyChangedFor(nameof(UsesDatabaseName))]
        private ConnectionMode connectBy = ConnectionMode.Host;

        public bool IsHostConnection => ConnectBy == ConnectionMode.Host;
        public bool IsUrlConnection => ConnectBy == ConnectionMode.Url;

        public bool ConnectByHost
        {
            get => ConnectBy == ConnectionMode.Host;
            set
            {
                if (value)
                    ConnectBy = ConnectionMode.Host;
            }
        }

        public bool ConnectByUrl
        {
            get => ConnectBy == ConnectionMode.Url;
            set
            {
                if (value)
                    ConnectBy = ConnectionMode.Url;
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        private string host = "localhost";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        private string port = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        private string databaseName = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        private string username = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        private string password = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        private string filepath = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        private string serviceName = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        [NotifyPropertyChangedFor(nameof(UsesCredentials))]
        private string sqlServerAuth = "SQL Server Authentication";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanConnect))]
        private string url = "";

        [ObservableProperty]
        private bool useSsl;

        [ObservableProperty]
        private string errorMessage = "";

        public bool IsSqlite => SelectedDatabase?.Id == "sqlite";
        public bool IsOracle => SelectedDatabase?.Id == "oracle";
        public bool IsSqlServer => SelectedDatabase?.Id == "sqlserver";
        public bool UsesServer => SelectedDatabase is not null && !IsSqlite;

        public bool UsesDatabaseName => UsesServer && !IsOracle && ConnectBy == ConnectionMode.Host;

        public bool UsesCredentials => UsesServer && (!IsSqlServer ||
            SqlServerAuth == "SQL Server Authentication");

        public bool SupportSsl => SelectedDatabase?.Id is "postgresql" or "mysql" or "mariadb";

        public string ConnectionTitle => SelectedDatabase is null ? "Connection"
            : $"{SelectedDatabase.DisplayName} connection";

        public Bitmap? IconDb => SelectedDatabase?.IconPath;

        public bool CanConnect
        {
            get
            {
                if (SelectedDatabase is null) return false;

                if (IsSqlite) return !string.IsNullOrWhiteSpace(Filepath);

                if (string.IsNullOrWhiteSpace(Host)) return false;

                if (!int.TryParse(Port, out int parsedPort)) return false;

                if (parsedPort is < 1 or > 65535) return false;

                if (IsOracle)
                {
                    if (string.IsNullOrWhiteSpace(ServiceName)) return false;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(DatabaseName)) return false;
                }

                if (UsesCredentials && string.IsNullOrWhiteSpace(Username)) return false;

                if (ConnectByUrl && string.IsNullOrWhiteSpace(Url)) return false;

                return true;
            }
        }

        public bool IsSelectingDatabase => CurrentStep == 0;
        public bool IsConfiguringConnection => CurrentStep == 1;

        public NewConnectionViewModel()
        {
            CurrentStep = 0;
        }

        partial void OnSelectedDatabaseChanged(DatabaseOptions? value)
        {
            if (value is null)
                return;

            Host = value.Id == "sqlite" ? "" : "localhost";

            Port = value.Id switch
            {
                "postgresql" => "5432",
                "mysql" => "3306",
                "mariadb" => "3306",
                "oracle" => "1521",
                "sqlserver" => "1433",
                _ => ""
            };

            DatabaseName = "";
            Username = "";
            Password = "";
            Filepath = "";
            ServiceName = "";
            SqlServerAuth = "SQL Server Authentication";
            UseSsl = false;
            ErrorMessage = "";
        }

        [RelayCommand(CanExecute = nameof(CanGoNext))]
        private void Next()
        {
            ErrorMessage = "";
            CurrentStep = 1;
        }

        private bool CanGoNext()
        {
            return SelectedDatabase is not null;
        }

        [RelayCommand]
        private void Back()
        {
            ErrorMessage = "";
            CurrentStep = 0;
        }

        private static Bitmap LoadIcon(string filename)
        {
            var uri = new Uri($"avares://SQLstudio/Assets/Icons/{filename}");

            using Stream stream = AssetLoader.Open(uri);
            return new Bitmap(stream);
        }
    }
}
