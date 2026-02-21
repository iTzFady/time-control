using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public TimeState CurrentState { get; private set; } = TimeState.Normal;
    [Range(0.1f, 5f)]
    public float fastForwardMultiplier = 10f;
    private List<ITimeReversible> registeredObjects = new();
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Update()
    {
        HandleInput();
    }
    private void HandleInput()
    {
        bool isRewinding = InputManager.Instance.Rewind();
        bool isFastForwarding = InputManager.Instance.FastForward();
        bool isPaused = InputManager.Instance.Pause();
        if (isPaused)
        {
            SetState(CurrentState == TimeState.Paused
                ? TimeState.Normal
                : TimeState.Paused);

            return;
        }

        if (CurrentState == TimeState.Paused)
            return;
        if (isRewinding)
        {
            SetState(TimeState.Rewinding);
        }
        else if (isFastForwarding)
        {
            SetState(TimeState.FastForward);
        }
        else
        {
            SetState(TimeState.Normal);
        }
    }
    public void Register(ITimeReversible obj)
    {
        if (!registeredObjects.Contains(obj))
            registeredObjects.Add(obj);
    }
    public void SetState(TimeState state)
    {
        if (CurrentState == state)
            return;

        CurrentState = state;

        switch (state)
        {
            case TimeState.Paused:
                Time.timeScale = 0f;
                break;

            case TimeState.FastForward:
                Time.timeScale = fastForwardMultiplier;
                break;

            default:
                Time.timeScale = 1f;
                break;
        }
        foreach (var obj in registeredObjects)
            obj.OnTimeStateChanged(state);
    }



}
