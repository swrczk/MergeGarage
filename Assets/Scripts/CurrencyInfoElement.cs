using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyInfoElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currencyText;

    public void OnCurrencyChanged(Component sender, object data)
    {
        if (data is int amount)
            ChangeCurrencyText(amount);
    }

    public void ChangeCurrencyText(int amount)
    {
        currencyText.text = amount.ToString();
    }
}