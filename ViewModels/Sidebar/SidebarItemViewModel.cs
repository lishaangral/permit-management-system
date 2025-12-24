namespace PemitManagement.ViewModels.Sidebar;

public class SidebarItemViewModel
{
    public string Title { get; set; } = "";
    public string Icon { get; set; } = "";
    public string Controller { get; set; } = "";
    public string Action { get; set; } = "Index";
    public string? RequiredPermission { get; set; }
    public List<SidebarItemViewModel> Children { get; set; } = new();
}
