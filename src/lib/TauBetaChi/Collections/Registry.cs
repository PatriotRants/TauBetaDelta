using ForgeWorks.RailThorn.Mathematics;
using ForgeWorks.TauBetaDelta.Extensibility;

namespace ForgeWorks.TauBetaDelta.Collections;

public class Registry
{
    private static readonly Lazy<Registry> INSTANCE = new(() => new());

    private readonly List<IRegistryItem> registry = new();

    private static Registry Instance => INSTANCE.Value;


    private Registry() { }

    public static void Add<TRegistryItem>(TRegistryItem registryItem) where TRegistryItem : IRegistryItem
    {
        Instance.registry.Add(registryItem);
    }
    public static TRegistryItem Get<TRegistryItem>() where TRegistryItem : IRegistryItem
    {
        return Instance.registry.Where(r => r is TRegistryItem)
            .Select(ri => (TRegistryItem)ri)
            .FirstOrDefault();
    }
}

public static class CollectionExtensions
{

    public static string ToString<T>(this T[,] matrix, int width)
    {
        var rowCount = matrix.Length / width;
        string[] stringMatrix = new string[rowCount];

        for (int r = 0; r < rowCount; r++)
        {
            var rowElements = new string[width];

            for (int i = 0; i < width; i++)
            {
                rowElements[i] = $"{matrix[r, i],15}";
            }

            stringMatrix[r] = $"\t{{{string.Join(",", rowElements)}}}";
        }

        return $"\n{{{string.Join("\n", stringMatrix)}}}";
    }

    public static string ToString(this Vertex[] vertices, int length)
    {
        string[] stringVertices = new string[length];

        for (int r = 0; r < vertices.Length; r++)
        {
            stringVertices[r] = $"{vertices[r],18}";
        }

        return $"\n{string.Join("\n", stringVertices)}";
    }
}
