using System.Text;

using OpenTK.Graphics.OpenGL4;

using ForgeWorks.GlowFork;
using ErrorCode = ForgeWorks.GlowFork.ErrorCode;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.RailThorn.Graphics;

public class ShaderManager : IDisposable
{
    private static readonly Lazy<ShaderManager> INSTANCE = new(() => new());

    private static readonly Dictionary<string, ShaderType> TAGTYPES = new() {
        { "[fragment]",     ShaderType.FragmentShader },
        { "[frag_arb]",     ShaderType.FragmentShaderArb },
        { "[vertex]",       ShaderType.VertexShader },
        { "[vert_arb]",     ShaderType.VertexShaderArb },
        { "[geometry]",     ShaderType.GeometryShader },
        { "[tess_eval]",    ShaderType.TessEvaluationShader },
        { "[tess_ctrl]",    ShaderType.TessControlShader },
        { "[compute]",      ShaderType.ComputeShader },
    };
    private static readonly string[] TAGS = TAGTYPES.Keys.ToArray();

    private (LoadStatus condition, ErrorCode errCode, string message) Status { get; set; }

    internal static ShaderManager Instance => INSTANCE.Value;

    public string ShaderDirectory { get; }
    public string Info => $"[{Status.condition}.{Status.errCode}] {Status.message}";
    public ResourceLogger Log { get; } = LoggerManager.Instance.Post;

    private ShaderManager()
    {
        //  TODO: replace hardcode directory with settings
        ShaderDirectory = "./shaders";

        if (!Directory.Exists(ShaderDirectory))
        {
            Status = (LoadStatus.Error, ErrorCode.DirNotFound, $"[{nameof(ShaderManager)}] Shader directory '{ShaderDirectory}' does not exist");
        }
        //  We could preload validation values ...???
        // else
        // {
        //     Status = (LoadStatus.Okay, ErrorCode.NoErr, $"[{nameof(ShaderManager)}] Loading Shaders '{ShaderDirectory}'");
        // }
    }

    /// <summary>
    /// Loads all shaders
    /// </summary>
    public void LoadShaders()
    {
        var shaderFiles = Directory.GetFiles(Instance.ShaderDirectory);

        foreach (var file in shaderFiles)
        {
            LoadShader(file);
        }
    }
    /// <summary>
    /// Load a single shader
    /// </summary>
    public Shader LoadShader(string shaderFile)
    {
        var file = Path.Combine(ShaderDirectory, shaderFile);
        var source = LoadFile(file);
        string shaderName = string.Empty;
        List<int> shaders = new();
        Shader shader = null;

        if (source.Length > 0)
        {
            shaderName = Path.GetFileNameWithoutExtension(shaderFile);

            foreach (var shaderInfo in ParseShaders(shaderName, source))
            {
                //  compile
                var handle = GL.CreateShader(shaderInfo.type);
                GL.ShaderSource(handle, shaderInfo.source);
                GL.CompileShader(handle);
                if (!ValidateShader(handle, out string shaderLog))
                {
                    Log(LoadStatus.Error, $"{nameof(LoadShader)}.{shaderFile}] {shaderLog}");
                    handle = -1;
                }

                shaders.Add(handle);
            }

            shader = new(shaderName, shaders.ToArray());
            if (!ValidateProgram(shader.Program, out string programLog))
            {
                Log(LoadStatus.Error, programLog);
                shader = null;
            }
        }

        return shader;
    }
    /// <summary>
    /// Get the current ShaderManager error status
    /// </summary>
    /// <param name="errCode"></param>
    /// <returns></returns>
    public bool GetError(out ErrorCode errCode)
    {
        errCode = Status.errCode;

        return errCode != ErrorCode.NoErr;
    }

    private static string[] LoadFile(string sourceFile)
    {
        List<string> lines = new();
        string line = string.Empty;

        using (var file = File.OpenRead(sourceFile))
        using (var reader = new StreamReader(file, Encoding.UTF8, false, leaveOpen: false))
        {
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }

        //  add a null line at the end
        lines.Add(null);

        return lines.ToArray();
    }
    private static IEnumerable<(ShaderType type, string source)> ParseShaders(string sourceTag, string[] source)
    {
        int line = 0;
        ShaderType type;
        StringBuilder shader;

        while (line < source.Length && source[line] != null)
        {
            if (TAGS.Any(t => t.Equals(source[line])))
            {
                //  we have one of the [tag] values
                if (!TAGTYPES.TryGetValue(source[line], out type))
                {
                    //  unrecognized shader type ... the whole source needs to be ignored
                    //  TODO: log and break
                    break;
                }
                //  get a new string builder
                shader = new();
                //  increment line count
                ++line;
                //  we have a tag type so read until the next tag type
                while (source[line] != null && !TAGS.Any(t => t.Equals(source[line])))
                {
                    shader.AppendLine(source[line]);
                    ++line;
                }

                if (shader.Length > 0)
                {
                    //  yield parsed result
                    yield return (type, shader.ToString());
                }
            }
        }
    }
    private bool ValidateShader(int shader, out string info)
    {
        info = GL.GetShaderInfoLog(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int flag);

        return flag != 0;
    }
    private bool ValidateProgram(int program, out string info)
    {
        info = GL.GetProgramInfoLog(program);
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int flag);

        return flag != 0;
    }

    public void Dispose()
    {
        //  ???
    }
}
