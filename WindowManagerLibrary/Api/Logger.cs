namespace WindowManagerLibrary.Api;


public class Logger {
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
        if(GlobalSettings.DebugMode) {
            Console.WriteLine($"DEBUG: {message}");
        }
    }
    public Logger() {
        
    }
    public void WriteLog(string message) {
        try {
            File.AppendAllText(logPath, log(message));
        } catch (Exception ex) {
            Console.WriteLine($"CRITICAL: Unable to write log: {ex.Message}");
        }
    }
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public void WriteErrorLog(string message = "", Exception e = null) {
        if(e != null) {
            try {
                File.AppendAllText(logPath, logEx(e));
            } catch (Exception ex) {
               Console.WriteLine($"CRITICAL: Unable to write log: {ex.Message}");
            }   
        } else if(!string.IsNullOrEmpty(message)) {
            try {
                File.AppendAllText(logPath, logErr(message));
            } catch (Exception ex) {
                Console.WriteLine($"CRITICAL: Unable to write log: {ex.Message}");
            }
        } else {
            Console.WriteLine($"CRITICAL: Unable to write log: WriteErrorLog message is an invalid type. Expected `string` or `Exception` type");
        }
    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
