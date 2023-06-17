using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    private GameEvent onCurrencyChanged;
    [SerializeField]
    private int nextLevel;

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
    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}