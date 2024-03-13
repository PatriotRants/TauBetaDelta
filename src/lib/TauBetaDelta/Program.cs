using ForgeWorks.TauBetaDelta.Collections;

using ForgeWorks.TauBetaDelta;

const string NAME = "TauBetaDelta";
const string TITLE = $"ForgeWorks: {NAME}";

Console.WriteLine($"Welcome to {TITLE}");

Registry.Add(new Resources());
Registry.Add(new Game(NAME, TITLE));

using (var tbd = new TauBetaDelta())
{
    tbd.Run();
}