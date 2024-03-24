using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using ForgeWorks.RailThorn.Fonts;
using ForgeWorks.RailThorn.Graphics;

namespace ForgeWorks.RailThorn.Controls;

public class Label : TextRenderer
{
    private const string DEFAULT_NAME = $"{nameof(Label)}_";
    private static int SPIN_COUNT = 0;

    public Label(IViewContainer container) : this(container, $"{DEFAULT_NAME}{SPIN_COUNT++:###}") { }
    public Label(IViewContainer container, string name) : base(container, name) { }
}

public abstract class TextRenderer : RendererControl
{
    private ICharacterSet characterMap;
    private float width;
    private float height;
    private Matrix4 projection;
    private int _vao;
    private int _vbo;

    public string Text { get; set; } = string.Empty;
    public Color4 Color { get; set; } = Color4.Black;
    public Font Font { get; set; } = "LiberationMono-Regular"; //   default font
    public float Scale { get; set; } = 0.5f;

    protected TextRenderer(IViewContainer container, string name) : base(name)
    {
        width = container.Width;
        height = container.Height;

        shader = SHADERS.LoadShader("label.shaders");
        shader.Use();
    }

    public override void Init()
    {
        //  get the font's character map
        characterMap = Font.CharacterSet;

        //  configure everything for rendering Teext
        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.Src1Alpha, BlendingFactor.OneMinusSrcAlpha);
        projection = Matrix4.CreateOrthographic(0.0f, width, 0.0f, height);
        GL.UniformMatrix4(GL.GetUniformLocation(shader.Program, "projection"), false, ref projection);

        //  create VAO & VBO
        GL.GenVertexArrays(1, out _vao);
        GL.GenBuffers(1, out _vbo);
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        /* **
            glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 6 * 4, NULL, GL_DYNAMIC_DRAW);
            glEnableVertexAttribArray(0);
            glVertexAttribPointer(0, 4, GL_FLOAT, GL_FALSE, 4 * sizeof(float), 0);
            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindVertexArray(0);
        ** */
        //  not sure about 'nint.Zero' ... c sample uses NULL
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, nint.Zero, BufferUsageHint.DynamicDraw);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public override void Update()
    {
        //  if font changes
        if (characterMap?.Name != Font.Name)
        {
            characterMap = Font.CharacterSet;
        }

        //  activate corresponding render state
        shader.Use();
        //  update colors
        GL.Uniform3(GL.GetUniformLocation(shader.Program, "textColor"), Color.R, Color.G, Color.B);
        //  set active texture
        GL.ActiveTexture(TextureUnit.Texture0);
        //  bind VAO
        GL.BindVertexArray(_vao);
    }
    public override void Render()
    {
        // iterate character map
        var text = Text.ToArray();
        var ndx = 0;
        var x = (float)Location.X;
        var y = (float)Location.Y;

        while (ndx < text.Length)
        {
            float[] vertices = MapText(text[ndx], ref x, ref y, out uint textureId);

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, sizeof(float) * 6 * 4, vertices);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            ++ndx;
        }

        GL.BindVertexArray(0);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private float[] MapText(char c, ref float x, ref float y, out uint textureId)
    {
        Font.Character character = characterMap[c];
        textureId = character.TextureId;
        var map = character.GetMapping();

        float xpos = x + map.xpos * Scale;
        float ypos = y - map.ypos * Scale;

        float w = map.w * Scale;
        float h = map.h * Scale;

        float[] vertices = new float[]{
            xpos,     ypos + h,   0.0f, 0.0f,
            xpos,     ypos,       0.0f, 1.0f,
            xpos + w, ypos,       1.0f, 1.0f ,
            xpos,     ypos + h,   0.0f, 0.0f ,
            xpos + w, ypos,       1.0f, 1.0f ,
            xpos + w, ypos + h,   1.0f, 0.0f
        };

        /*         
            // now advance cursors for next glyph (note that advance is number of 1/64 pixels)
            x += (ch.Advance >> 6) * scale; // bitshift by 6 to get value in pixels (2^6 = 64 (divide amount of 1/64th pixels by 64 to get amount of pixels))
         */
        x += character.Advance / 64 * Scale;

        return vertices;
    }
}