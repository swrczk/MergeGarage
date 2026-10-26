using System;
using TMPro;
using UnityEngine;

public class TimerElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    public void OnTimerChanged(Component sender, object data)
    {
        if (data is float time)
            ChangeTimerText(time);
    }

    private void ChangeTimerText(float time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        _timerText.text = timeSpan.ToString("mm':'ss");
    }
}