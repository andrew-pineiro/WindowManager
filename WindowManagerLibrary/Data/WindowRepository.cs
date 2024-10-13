using System.Runtime.InteropServices;
using System.Text;
using WindowManagerLibrary.Models;
using WindowManagerLibrary.Api;
namespace WindowManagerLibrary.Data;

public class WindowRespository {
    
    private readonly Logger _logger = new();
    public WindowRespository() {
    }
    public List<MonitorModel> GetAllMonitors() {
        var monitors = new List<MonitorModel>();
        var id = 1;
        
        bool result = Windows.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (hMonitor, hdcMonitor, lprcMonitor, dwData) =>
        {   
            _logger.WriteLog("Enumerating Monitors...");
            Windows.MonitorInfoEx monitorInfo = new();
            monitorInfo.cbSize = Marshal.SizeOf(typeof(Windows.MonitorInfoEx));

            if (Windows.GetMonitorInfo(hMonitor, ref monitorInfo)) {
                string monitorName = monitorInfo.szDevice;
                if(!string.IsNullOrEmpty(monitorName)) {
                    monitorName = monitorName.Substring(monitorName.LastIndexOf('\\')+1);
                }
                monitors.Add(new MonitorModel() {
                    Id = id,
                    Top = monitorInfo.rcMonitor.Top,
                    Bottom = monitorInfo.rcMonitor.Bottom,
                    Left = monitorInfo.rcMonitor.Left,
                    Right = monitorInfo.rcMonitor.Right,
                    Name = monitorName,
                    IsPrimary = (monitorInfo.dwFlags & Windows.MONITORINFOF_PRIMARY) != 0
                });
                _logger.WriteLog($"Monitor Found: {monitorInfo.szDevice}; ID: {id}");
            } else {
                _logger.WriteErrorLog($"GetMonitorInfo failed - {Marshal.GetLastWin32Error()}");
            }
            id++;
            return true;
        }, IntPtr.Zero);
        return monitors;

    }
    public List<WindowModel> GetAllWindows() {
    int counter = 0;
    var windows = new List<WindowModel>();
    _logger.WriteLog("Enumerating Windows...");
        Windows.EnumWindows(new Windows.EnumWindowsProc((hWnd, lParam) => {
            StringBuilder windowText = new StringBuilder(256);
            Windows.GetWindowText(hWnd, windowText, windowText.Capacity);

            if (windowText.Length > 0 && Windows.IsWindowVisible(hWnd)) {

                windows.Add(new WindowModel(){
                    Id = counter,
                    WindowName = windowText.ToString(),
                    WindowProcessId = hWnd
                });
                counter++;
                _logger.WriteLog($"Window found: {windowText}; PID: {hWnd}");
            }
            return true;
        }), IntPtr.Zero);
        return windows;
    }

    public void MoveWindow(WindowModel window, MonitorModel monitor) {
        //TODO(#4): moving window needs to be improved
        //BUGS - Shows more windows than open
        //     - When switched it doesn't take over as main process
        //     - Full screen looks strange
        if (window.WindowProcessId != IntPtr.Zero && monitor != null)
        {

            int width = monitor.Right - monitor.Left;
            int height = monitor.Bottom - monitor.Top;

            if(Windows.MoveWindow(window.WindowProcessId, monitor.Left, monitor.Top, width, height, true)) {
                _logger.WriteLog($"Moved Window `{window.WindowName}` to `{monitor.Name}`");
            } else {
                _logger.WriteErrorLog($"Unable to move Window: {window.WindowName}");
            }
        }
    }
}
