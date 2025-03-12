public interface IRewindable
{
    void SaveState();
    void RewindState();
    void ClearHistory();

}
