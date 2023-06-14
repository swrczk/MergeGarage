using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    private GameEvent onCurrencyChanged;

    public int Currency { get; private set; } = 0;

    public void AddCurrency(int currencyAmount)
    {
        Currency += currencyAmount;
        onCurrencyChanged.Raise(this, Currency);
    }

    public void ResetCurrency()
    {
        Currency = 0;
        onCurrencyChanged.Raise(this, Currency);
    }
}