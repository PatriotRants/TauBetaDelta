using OpenTK.Mathematics;

namespace ForgeWorks.RailThorn.Mathematics;

public struct Vertex
{
    public Vector2 Pos;
    public Vector2 Tex;

    public static implicit operator Vertex(((float x, float y) pos, (float x, float y) tex) vec)
    {
        return new()
        {
            Pos = vec.pos,
            Tex = vec.tex
        };
    }

    public override string ToString()
    {
        return $"{{{{Pos:{Pos}}};{{Tex:{Tex}}}}}";
    }
}
