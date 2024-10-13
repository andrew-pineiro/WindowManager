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
            Windows.MonitorInfoEx monitorInfo = new();
            monitorInfo.cbSize = Marshal.SizeOf(typeof(Windows.MonitorInfoEx));

            if (Windows.GetMonitorInfo(hMonitor, ref monitorInfo)) {
                monitors.Add(new MonitorModel() {
                    Id = id,
                    Top = monitorInfo.rcMonitor.Top,
                    Bottom = monitorInfo.rcMonitor.Bottom,
                    Left = monitorInfo.rcMonitor.Left,
                    Right = monitorInfo.rcMonitor.Right,
                    Name = monitorInfo.szDevice,
                    IsPrimary = (monitorInfo.dwFlags & Windows.MONITORINFOF_PRIMARY) != 0
                });
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
            }
            return true;
        }), IntPtr.Zero);
        return windows;
    }

    public void MoveWindow(IntPtr hWnd, int screen) {
//TODO(#4): moving window needs to be improved
        //BUGS - Shows more windows than open
        //     - When switched it doesn't take over as main process
        //     - Full screen looks strange
        var monitors = GetAllMonitors();
        screen--;
        if (hWnd != IntPtr.Zero && monitors.Count > 0)
        {
            int width = monitors[screen].Right - monitors[screen].Left;
            int height = monitors[screen].Bottom - monitors[screen].Top;

            Windows.MoveWindow(hWnd, monitors[screen].Left, monitors[screen].Top, width, height, true);
        }
    }
}
