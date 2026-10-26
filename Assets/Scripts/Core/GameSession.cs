using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private GameEvent _onCurrencyChanged;

    [SerializeField] private int _nextLevel;

    public int Currency { get; private set; }

    public void AddCurrency(int currencyAmount)
    {
        Currency += currencyAmount;
        _onCurrencyChanged.Raise(this, Currency);
    }

    public void ResetCurrency()
    {
        Currency = 0;
        _onCurrencyChanged.Raise(this, Currency);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(_nextLevel);
    }
}