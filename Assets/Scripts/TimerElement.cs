using System;
using TMPro;
using UnityEngine;

public class TimerElement:MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    public void OnTimerChanged(Component sender, object data)
    {
        if (data is float time)
            ChangeTimerText(time);
    }

    private void ChangeTimerText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timerText.text = timeSpan.ToString("mm':'ss");
    }
}