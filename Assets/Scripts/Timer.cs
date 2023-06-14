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

    public void StopTimer(float duration)
    {
        StopCoroutine(coroutine);
    }

    private IEnumerator RunTimer(float duration)
    {
        while (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
            onTimeChanged.Raise(this, timeValue);
            yield return null;
        }

        onTimerFinished.Raise();
    }
}