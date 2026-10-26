using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameEvent _onTimerFinished;

    [SerializeField] private GameEvent _onTimeChanged;

    private Coroutine coroutine;

    public float CurrentTimeValue { get; private set; }

    public void StartTimer(float duration)
    {
        CurrentTimeValue = duration;
        coroutine = StartCoroutine(RunTimer(duration));
    }

    public void StopTimer()
    {
        if (coroutine == null)
            return;

        StopCoroutine(coroutine);
        coroutine = null;
    }

    private IEnumerator RunTimer(float duration)
    {
        while (CurrentTimeValue > 0)
        {
            CurrentTimeValue = Mathf.Max(0, CurrentTimeValue - Time.deltaTime);
            _onTimeChanged.Raise(this, CurrentTimeValue);
            yield return null;
        }

        coroutine = null;
        _onTimerFinished.Raise();
    }
}