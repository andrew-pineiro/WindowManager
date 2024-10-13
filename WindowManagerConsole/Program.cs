using WindowManagerLibrary.Data;

WindowRespository repo = new();

var windows = repo.GetAllWindows();
if(windows.Count <= 0) {Environment.Exit(0);}

foreach(var window in windows) {
    Console.WriteLine($"ID: {window.Id}, PID: {window.WindowProcessId}, Title: {window.WindowName}");
}

Console.Write("Which window?: ");
var id = Console.ReadLine();

var monitors = repo.GetAllMonitors();
if(monitors.Count <= 0) {
    Console.WriteLine("ERROR: no monitors found");
    Environment.Exit(0);
}

var monitorId = 1;
foreach(var monitor in monitors) {
    Console.WriteLine($"Monitor {monitorId}: Device={monitor.Name}, " +
                        $"X={monitor.Left}, Y={monitor.Top}, " +
                        $"Width={monitor.Right -monitor.Left}, " +
                        $"Height={monitor.Bottom - monitor.Top}, " +
                        $"Primary={monitor.IsPrimary}");
    monitorId++;
}

Console.Write("Which screen?: ");
var screenId = Console.ReadLine();


if((!int.TryParse(id, out int parsedId)) || (!int.TryParse(screenId, out int parsedScreenId))) {
    Console.WriteLine("ERROR: Invalid ID Supplied");
    Environment.Exit(1);
}
repo.MoveWindow(windows[parsedId].WindowProcessId, int.Parse(screenId));
