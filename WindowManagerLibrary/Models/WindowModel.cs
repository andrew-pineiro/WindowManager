namespace WindowManagerLibrary.Models;

public class WindowModel {
    public required int Id { get; set; }
    public string? WindowName { get; set; }
    public nint WindowProcessId { get; set; }
}