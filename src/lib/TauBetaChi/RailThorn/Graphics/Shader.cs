using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace ForgeWorks.RailThorn.Graphics;
// A simple class meant to help create shaders.
public class Shader : IDisposable
{
    private static readonly ShaderManager SHADERS = ShaderManager.Instance;

    public readonly int Program;

    private readonly Dictionary<string, int> _uniformLocations;

    internal Shader(string name, int[] shaders)
    {
        // The shaders must be merged into a shader program, which can then be used by OpenGL.
        // To do this, create a program...
        Program = GL.CreateProgram();
        //  Attach shaders to program
        foreach (var handle in shaders)
        {
            GL.AttachShader(Program, handle);
        }
        // And then link them together.
        GL.LinkProgram(Program);
        // Program validation will occur by the ShaderManager once this shader is constructed.

        // When the shader program is linked, no longer need the individual shaders attached to it; the compiled code is copied into the shader program.
        // Detach and then delete them.
        foreach (var handle in shaders)
        {
            GL.DetachShader(Program, handle);
            GL.DeleteShader(handle);
        }

        // Cache the active uniforms to boost uniform access performance
        // Get the number of active uniforms in the shader.
        GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        // Next, allocate the dictionary to hold the locations.
        _uniformLocations = new Dictionary<string, int>();

        // Loop over all the uniforms,
        for (var i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = GL.GetActiveUniform(Program, i, out _, out _);

            // get the location,
            var location = GL.GetUniformLocation(Program, key);

            // and then add it to the dictionary.
            _uniformLocations.Add(key, location);
        }

    }

    // A wrapper function that enables the shader program.
    public void Use()
    {
        GL.UseProgram(Program);
    }
    // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
    // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Program, attribName);
    }

    // Uniform setters
    // Uniforms are variables that can be set by user code, instead of reading them from the VBO.
    // You use VBOs for vertex-related data, and uniforms for almost everything else.

    // Setting a uniform is almost always the exact same, so I'll explain it here once, instead of in every method:
    //     1. Bind the program you want to set the uniform on
    //     2. Get a handle to the location of the uniform with GL.GetUniformLocation.
    //     3. Use the appropriate GL.Uniform* function to set the uniform.

    /// <summary>
    /// Set a uniform int on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetInt(string name, int data)
    {
        GL.UseProgram(Program);
        GL.Uniform1(_uniformLocations[name], data);
    }
    /// <summary>
    /// Set a uniform float on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetFloat(string name, float data)
    {
        GL.UseProgram(Program);
        GL.Uniform1(_uniformLocations[name], data);
    }
    /// <summary>
    /// Set a uniform Matrix4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    /// <remarks>
    ///   <para>
    ///   The matrix is transposed before being sent to the shader.
    ///   </para>
    /// </remarks>
    public void SetMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(Program);
        GL.UniformMatrix4(_uniformLocations[name], true, ref data);
    }
    /// <summary>
    /// Set a uniform Vector3 on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetVector3(string name, Vector3 data)
    {
        GL.UseProgram(Program);
        GL.Uniform3(_uniformLocations[name], data);
    }

    public void Dispose()
    {
        GL.DeleteProgram(Program);
    }
}

