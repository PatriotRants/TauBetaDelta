using OpenTK.Graphics.OpenGL4;

using ForgeWorks.RailThorn.Graphics;

namespace ForgeWorks.RailThorn.Controls;

public abstract class ImageRenderer : RendererControl
{
    private float[] _vertices;
    private uint[] _indices;
    private int _vao;
    private int _vbo;
    private int _ebo;

    protected Texture texture { get; init; }

    protected ImageRenderer(string name) : base(name)
    {
        Shader = SHADERS.LoadShader("image.shaders");
        Shader.Use();
    }

    public override void Init()
    {
        _vertices = new float[] {
            //Position          Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };
        _indices = new uint[]
        {
            0, 1, 3,
            1, 2, 3
        };

        Shader.Use();

        // create VAO to align VBO
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        //  create VBO
        // bind the buffer
        // upload vertices to buffer
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        //  pass vertex array to the buffer
        var vertexLocation = GL.GetAttribLocation(Shader.Program, "aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        // setup texture coordinates
        var texCoordLocation = GL.GetAttribLocation(Shader.Program, "aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
    }

    public override void Render()
    {
        // Bind the VAO
        GL.BindVertexArray(_vao);

        //  use the texture & shader program
        texture.Use(TextureUnit.Texture0);
        Shader.Use();

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}