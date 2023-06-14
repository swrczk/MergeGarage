using UnityEngine;

public class GameSession : MonoBehaviour
{
    public GameEvent OnCurrencyChanged;
    
    private int _currency = 0;

    public void AddCurrency(int currencyAmount)
    {
        _currency += currencyAmount;
        OnCurrencyChanged.Raise(this, _currency);
    }

    public void ResetCurrency()
    {
        _currency = 0;
        OnCurrencyChanged.Raise(this, _currency);
    }
}