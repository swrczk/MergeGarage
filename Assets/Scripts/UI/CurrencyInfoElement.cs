using TMPro;
using UnityEngine;

public class CurrencyInfoElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;

    public void OnCurrencyChanged(Component sender, object data)
    {
        if (data is int amount)
            ChangeCurrencyText(amount);
    }

    public void ChangeCurrencyText(int amount)
    {
        _currencyText.text = amount.ToString();
    }
}