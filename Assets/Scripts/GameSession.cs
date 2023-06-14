using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    private GameEvent onCurrencyChanged;
    
    private int _currency = 0;

    public void AddCurrency(int currencyAmount)
    {
        _currency += currencyAmount;
        onCurrencyChanged.Raise(this, _currency);
    }

    public void ResetCurrency()
    {
        _currency = 0;
        onCurrencyChanged.Raise(this, _currency);
    }
}