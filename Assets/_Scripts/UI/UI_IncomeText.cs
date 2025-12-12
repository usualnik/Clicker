using TMPro;
using UnityEngine;

public class UI_IncomeText : MonoBehaviour
{
    private TextMeshProUGUI _incomeText;

    private void Awake()
    {
        _incomeText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerIncomeChanged += GameManager_OnPlayerIncomeChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerIncomeChanged -= GameManager_OnPlayerIncomeChanged;

    }

    private void GameManager_OnPlayerIncomeChanged(int newIncomeValue)
    {
        _incomeText.text = string.Format($"1 click = {newIncomeValue} dollars");
    }
}
