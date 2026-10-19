namespace SQLstudio.Models;

public sealed record NewConnectionDatabase(
    string DatabaseId,
    string Host,
    int? Port,
    string DatabaseName,
    string Username,
    string Password,
    string FilePath,
    string ServiceName,
    string AuthencationType,
    bool UseSsl
    );