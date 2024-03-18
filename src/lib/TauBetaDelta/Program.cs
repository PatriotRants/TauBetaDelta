using ForgeWorks.ShowBird;

using ForgeWorks.TauBetaDelta;
using ForgeWorks.TauBetaDelta.Collections;

const string NAME = "TauBetaDelta";
const string TITLE = $"ForgeWorks: {NAME}";

Console.WriteLine($"Welcome to {TITLE}");

Registry.Add(new Network(NAME));
Registry.Add(new Resources());
Registry.Add(new Game(NAME, TITLE));

using (TauBetaDelta tbd = new())
{
    tbd.Run();
}