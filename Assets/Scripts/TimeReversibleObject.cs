using UnityEngine;
public class TimeReversibleObject : MonoBehaviour, ITimeReversible
{
    public int maxRecordedFrames = 600;
    private TimeBuffer<Snapshot> buffer;
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        buffer = new TimeBuffer<Snapshot>(maxRecordedFrames);

    }
    private void Start()
    {
        TimeManager.Instance.Register(this);
    }
    private void Update()
    {
        switch (TimeManager.Instance.CurrentState)
        {
            case TimeState.Normal:
            case TimeState.FastForward:
                RecordState();
                break;
            case TimeState.Rewinding:
                RestoreState();
                break;
        }
    }
    public void RecordState()
    {
        buffer.Record(new Snapshot(transform, rb, gameObject));
    }
    public void RestoreState()
    {
        if (buffer.Count > 0)
        {
            var snapshot = buffer.PopLast();
            snapshot.Apply(transform, rb, gameObject);
        }
    }
    public void OnTimeStateChanged(TimeState newState)
    {
        if (rb == null) return;

        if (newState == TimeState.Rewinding)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}
