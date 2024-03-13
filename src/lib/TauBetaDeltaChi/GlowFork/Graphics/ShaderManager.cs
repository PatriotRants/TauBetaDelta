using OpenTK.Graphics.OpenGL4;

using ForgeWorks.GlowFork;
using ErrorCode = ForgeWorks.GlowFork.ErrorCode;

using ForgeWorks.TauBetaDelta.Logging;
using System.Text;

namespace ForgeWorks.GlowFork.Graphics;

public class ShaderManager
{
    private static readonly Lazy<ShaderManager> INSTANCE = new(() => new());

    private readonly Dictionary<string, int> _programs = new();
    private (LoadStatus condition, ErrorCode errCode, string message) Status { get; set; }

    internal static ShaderManager Instance => INSTANCE.Value;

    public string ShaderDirectory { get; }
    public string Info => Status.message;
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
        //     Status = (LoadStatus.Okay, ErrorCode.NoErr, $"[{nameof(ShaderManager)}] Loading Shader directory '{ShaderDirectory}'");
        // }
    }

    public int Load(ShaderType shaderType, string sourceTag)
    {
        string source = LoadSource(sourceTag);
        var shader = GL.CreateShader(shaderType);

        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);
        if (!ValidateShader(shader, out string info))
        {
            Log(LoadStatus.Error, info);
            return -1;
        }

        return shader;
    }
    public void CreateProgram(string programName, params int[] shaders)
    {
        //  create the program
        var program = GL.CreateProgram();
        //  attach shaders
        foreach (var shader in shaders)
        {
            GL.AttachShader(program, shader);
        }
        //  link
        GL.LinkProgram(program);
        if (!ValidateProgram(program, out string info))
        {
            Log(LoadStatus.Error, info);
            return;
        }

        //  clean up unnecessary artifacts
        foreach (var shader in shaders)
        {
            GL.DetachShader(program, shader);
            GL.DeleteShader(shader);
        }

        //  cache the program
        _programs.Add(programName, program);
    }
    public void DeleteProgram(string programName)
    {
        if (_programs.TryGetValue(programName, out int program))
        {
            GL.DeleteProgram(program);
        }
    }
    public int GetProgram(string programName)
    {
        if (_programs.TryGetValue(programName, out int program))
        {
            return program;
        }

        return -1;
    }
    public bool GetError(out ErrorCode errCode)
    {
        errCode = Status.errCode;

        return errCode != ErrorCode.NoErr;
    }

    private string LoadSource(string tag)
    {
        var tagInfo = tag.Split(".", 2, StringSplitOptions.RemoveEmptyEntries);
        var shaderInfo = (file: $"{tagInfo[0]}.shaders", tag: tagInfo[1]);
        var source = new StringBuilder();

        //  if validation fails, check error status & code
        if (ValidateShaderInfo(shaderInfo, out int position))
        {
            //  position tells us where reading stopped
            var shaderFile = Path.Combine(ShaderDirectory, shaderInfo.file);
            using (var file = File.OpenRead(shaderFile))
            {
                file.Position = position;
                string line = string.Empty;
                using (var reader = new StreamReader(file, Encoding.UTF8, false, leaveOpen: false))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        //  did we reach end of file?
                        if (IsShaderTag(line))
                        { break; }
                        if (!string.IsNullOrEmpty(line))
                        //  new lines are stripped with ReadLine method
                        { source.AppendLine(line); }
                    }
                }
            }
            //  end reading where a line matches the format "[...]"
        }

        return source.ToString();
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
    private bool ValidateShaderInfo((string file, string tag) shaderInfo, out int position)
    {
        position = -1;

        //  validate string are not null or empty
        if (string.IsNullOrEmpty(shaderInfo.file) ||
            string.IsNullOrEmpty(shaderInfo.tag))
        {
            //  invalid

            //  TODO: set error status & code
        }
        //  validate file exists
        var shaderFile = Path.Combine(ShaderDirectory, shaderInfo.file);
        if (!File.Exists(shaderFile))
        {
            //  invalid

            //  TODO: set error status & code
        }
        //  validate tag - locate line position following tag
        var shaderTag = $"[{shaderInfo.tag}]";
        using (var file = File.OpenRead(shaderFile))
        {
            string line = string.Empty;
            int length = 0;
            using (var reader = new StreamReader(file, Encoding.UTF8, false, leaveOpen: true))
            {
                while (line != shaderTag)
                {
                    //  did we reach end of file?
                    if ((line = reader.ReadLine()) == null)
                    {
                        break;
                    }

                    //  have to account for the length of the new line since it's lost
                    length += line.Length + Environment.NewLine.Length;
                }
            }

            if (line == shaderTag)
            {
                //  set position
                position = length;
            }
        }

        return position != -1;
    }
    private bool IsShaderTag(string line)
    {
        /* **
            assumptions:
                1. assessing a full line
                2. a shader tag is properly formed
                    - self-contained on a single line
                    - demarked with [...]
                    - nothing of value precedes a tag
                    - nothing of value follows a tag

                we really don't care what is between the [...]
        ** */
        return line.StartsWith('[') &&
               line.EndsWith(']');
    }
}
