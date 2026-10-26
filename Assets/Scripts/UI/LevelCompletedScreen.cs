using TMPro;
using UnityEngine;

public class LevelCompletedScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;

    [SerializeField] private TextMeshProUGUI _timeBonusText;

    public void OnLevelCompleted(int currency, int timeBonus)
    {
        gameObject.SetActive(true);
        _currencyText.text = currency.ToString();
        _timeBonusText.text = timeBonus.ToString();
    }

    public void HideScreen()
    {
        gameObject.SetActive(false);
    }
}