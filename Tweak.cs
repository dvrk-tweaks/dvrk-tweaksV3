namespace DvrkTweaks.Models;

public class Tweak
{
    public Tweak() { }

    public Tweak(string name, string description, string actionId, bool supported = true, bool requiresAdmin = false)
    {
        Name = name;
        Description = description;
        ActionId = actionId;
        Supported = supported;
        RequiresAdmin = requiresAdmin;
    }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ActionId { get; set; } = "";
    public bool Supported { get; set; } = true;
    public bool RequiresAdmin { get; set; }
}
