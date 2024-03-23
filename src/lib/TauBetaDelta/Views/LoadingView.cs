using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

using ForgeWorks.RailThorn.Graphics;
using ForgeWorks.RailThorn.Controls;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.TauBetaDelta;

public class LoadingView : GameView
{
    private int vertexBufferObject;
    private int elementBufferObject;
    private int vertexArrayObject;

    private Texture Texture { get; set; }
    private float[] Vertices { get; set; }
    private uint[] Indices { get; set; }

    public LoadingView(LoadingState gameState) : base(gameState)
    {
        Shader = gameState.Shader;
        Texture = gameState.Textures[0];
    }

    public override void Init()
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(Init)}");

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

        Shader.Use();

        // create VAO to align VBO
        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);

        //  create VBO
        // bind the buffer
        // upload vertices to buffer
        vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

        elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

        //  pass vertex array to the buffer
        var vertexLocation = GL.GetAttribLocation(Shader.Program, "aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        // setup texture coordinates
        var texCoordLocation = GL.GetAttribLocation(Shader.Program, "aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        OnLoad();
    }

    public override void OnLoad()
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnLoad)}");

        GL.ClearColor(Background);

        Texture.Use(TextureUnit.Texture0);
        Container.Add(new Label("Status_1")
        {
            Color = Color4.PaleGoldenrod,
            Text = "Hello, TauBeta"
        });
        //  raise event
        base.OnLoad();
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
        GL.BindVertexArray(vertexArrayObject);

        //  use the texture & shader program
        Texture.Use(TextureUnit.Texture0);
        Shader.Use();

        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }
}
