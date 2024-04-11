using ForgeWorks.RailThorn.Graphics;

namespace ForgeWorks.RailThorn.Controls;

public class Image : ImageRenderer
{
    public Image(string name, string texName) : base(name)
    {
        if (ASSETS.LoadTexture(texName, out Texture _texture))
        {
            texture = _texture;
        }
        else
        {
            //  log error condition
        }
    }
}
