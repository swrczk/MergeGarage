using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float CurrentTimeValue => timeValue;

    [SerializeField]
    private GameEvent onTimerFinished;

    [SerializeField]
    private GameEvent onTimeChanged;

    private float timeValue;
    private Coroutine coroutine;

    public void StartTimer(float duration)
    {
        timeValue = duration;
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
        while (timeValue > 0)
        {
            timeValue = Mathf.Max(0, timeValue - Time.deltaTime);
            onTimeChanged.Raise(this, timeValue);
            yield return null;
        }

        coroutine = null;
        onTimerFinished.Raise();
    }
}