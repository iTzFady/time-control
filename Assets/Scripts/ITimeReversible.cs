public interface ITimeReversible
{
    void RecordState();
    void RestoreState();
    void OnTimeStateChanged(TimeState newState);
}