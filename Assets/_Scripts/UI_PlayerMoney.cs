using TMPro;
using UnityEngine;

public class UI_PlayerMoney : MonoBehaviour
{
    private TextMeshProUGUI _moneyText;

    private void Awake()
    {
        _moneyText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerMoneyChanged += GameManager_OnPlayerMoneyChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerMoneyChanged -= GameManager_OnPlayerMoneyChanged;

    }
    private void GameManager_OnPlayerMoneyChanged(int newPlayerMoneyValue)
    {
        _moneyText.text = newPlayerMoneyValue.ToString();
    }
}
