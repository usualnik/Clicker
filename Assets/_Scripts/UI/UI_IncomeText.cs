using TMPro;
using UnityEngine;
using YG;

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
        LoadIncomeText();

    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerIncomeChanged -= GameManager_OnPlayerIncomeChanged;

    }
    private void LoadIncomeText()
    {
        int incomeValue = ShopItemsManager.Instance.AllItemsInShop[PlayerData.Instance.GetCurrentItemIndex()]
            .ItemIncomeIncrease;

        _incomeText.text = YG2.envir.language == "ru" ? string.Format($"1 клик = {incomeValue} долларов")
        : string.Format($"1 click = {incomeValue} dollars");
    }
    private void GameManager_OnPlayerIncomeChanged(int newIncomeValue)
    {
        _incomeText.text = YG2.envir.language == "ru" ? string.Format($"1 клик = {newIncomeValue} долларов")
        : string.Format($"1 click = {newIncomeValue} dollars");
    }
}
