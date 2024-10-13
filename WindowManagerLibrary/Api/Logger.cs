namespace WindowManagerLibrary.Api;


public class Logger {
    //TODO(#1): need to handle all errors in logging
    private readonly string logPath = Path.Join(GlobalSettings.LogFilePath, $"{logFileDate}_wm_{GlobalSettings.BuildNumber}.log");
    private static readonly string logFileDate = DateTime.Now.ToString("MM-yyyy");
    private static string log(string message) {
        return $"[{DateTime.Now}] INFO: {message}\n";
    }
    //TODO(#3): improve exception logging
    private static string logEx(Exception e) {
        return $"[{DateTime.Now}] EXCEPTION: {e.Message}\n";
    } 
    private static string logErr(string message) {
        return $"[{DateTime.Now}] ERROR: {message}\n";
    }
    public static void WriteDebug(string message) {
        Console.WriteLine($"DEBUG: {message}");
    }
    public Logger() {
        
    }
    public void WriteLog(string message) {
        File.AppendAllText(logPath, log(message));
    }
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public void WriteErrorLog(string message = "", Exception e = null) {
        if(e != null) {
            File.AppendAllText(logPath, logEx(e));
        } else if(!string.IsNullOrEmpty(message)) {
            File.AppendAllText(logPath, logErr(message));
        } else {
            throw new Exception("error trying to write error log");
        }
    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
