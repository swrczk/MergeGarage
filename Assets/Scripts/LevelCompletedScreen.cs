using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCompletedScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currencyText;

    [SerializeField]
    private TextMeshProUGUI timeBonusText;

    public void OnLevelCompleted(int currency, int timeBonus)
    {
        gameObject.SetActive(true);
        currencyText.text = currency.ToString();
        timeBonusText.text = timeBonus.ToString();
    }

    public void HideScreen( )
    {
        gameObject.SetActive(false);
    }
}