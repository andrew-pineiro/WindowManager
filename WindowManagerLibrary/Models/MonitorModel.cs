namespace WindowManagerLibrary.Models;

public class MonitorModel {
    public int Id { get; set; }
    public int Top { get; set; }
    public int Bottom { get; set; }
    public int Left { get; set; }
    public int Right { get; set; }
    public string? Name { get; set; }
    public bool IsPrimary { get; set; }
}