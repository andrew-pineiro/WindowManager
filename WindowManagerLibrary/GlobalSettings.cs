namespace WindowManagerLibrary;

public static class GlobalSettings {
    public static string LogFilePath { get; private set; } = $"C:\\Users\\Chill\\Repositories\\WindowManager\\.Logs";
    public static int PrimaryDisplay { get; set; } = 1;
    public static string BuildNumber { get; set; } = "1.0.4";
    public static bool DebugMode { get; set; } = false;
}