namespace ForgeWorks.ShowBird.Messaging;

public delegate void UpdateAgent(string statusMessage);

public interface IUpdate
{
    /// <summary>
    /// Updater delegate
    /// </summary>
    UpdateAgent Updater { get; set; }

}