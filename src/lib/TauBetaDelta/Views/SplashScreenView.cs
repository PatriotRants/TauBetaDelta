using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.GlowFork.Graphics;

namespace ForgeWorks.TauBetaDelta;

public class SplashScreenView : GameView
{
    private int _vertexBufferObject;
    private int _elementBufferObject;
    private int _vertexArrayObject;
    private Texture _texture;

    private float[] Vertices { get; set; }
    private uint[] Indices { get; set; }

    public SplashScreenView(GameState gameState) : base(gameState)
    {
    }

    public override void OnLoad()
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnLoad)}");

        //  get program
        ShaderProgram = SHADERS.GetProgram("splash");
        GL.UseProgram(ShaderProgram);

        Vertices = new float[] {
            //Position          Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };
        Indices = new uint[]
        {
            0, 1, 3,
            1, 2, 3
        };

        GL.ClearColor(Background);

        // create VAO to align VBO
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        //  create VBO
        // bind the buffer
        // upload vertices to buffer
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

        //  pass vertex array to the buffer
        var vertexLocation = GL.GetAttribLocation(ShaderProgram, "aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        // setup texture coordinates
        var texCoordLocation = GL.GetAttribLocation(ShaderProgram, "aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        if (ASSETS.LoadTexture("splash", out _texture))
        {
            _texture.Use(TextureUnit.Texture0);
        }
        else
        {
            //  log error condition
        }
    }
    public override void OnResize(ResizeEventArgs args)
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnResize)} [{Location};{ViewPort}]");

        //  update the opengl viewport
        SetViewport(Location, ViewPort);
    }
    public override void OnRenderFrame(FrameEventArgs args)
    {
        // GL.ClearColor(Background);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Bind the VAO
        GL.BindVertexArray(_vertexArrayObject);

        //  use the texture & program
        _texture.Use(TextureUnit.Texture0);
        GL.UseProgram(ShaderProgram);

        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }
}
