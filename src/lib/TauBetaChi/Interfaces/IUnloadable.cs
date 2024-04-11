namespace ForgeWorks.TauBetaDelta.Extensibility;

public interface IUnloadable
{
    public void Unload(AutoResetEvent taskEvent);
}
