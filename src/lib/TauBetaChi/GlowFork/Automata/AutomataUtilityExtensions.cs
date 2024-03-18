namespace ForgeWorks.GlowFork.Automata;

public static class AutomataUtilityExtensions
{

    internal static bool TryGet(this Type[] typeList, string stateTypeName, out Type stateType)
    {
        stateType = typeList
            .Where(t => t.Name.Equals(stateTypeName))
            .Select(t => t)
            .FirstOrDefault();

        return stateType != null;
    }
}