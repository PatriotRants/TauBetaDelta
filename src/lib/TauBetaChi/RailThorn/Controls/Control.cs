using OpenTK.Mathematics;

namespace ForgeWorks.RailThorn.Controls;

public abstract class Control
{
    public string Name { get; }
    public Vector2i Location { get; set; } = (0, 0);

    protected Control(string name)
    {
        Name = name;
    }
}