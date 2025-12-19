using UnityEngine;
using UnityEngine.UI;

public class UI_ShopButtonColor : MonoBehaviour
{
    private Color _normalColor = Color.white;
    private Color _highlightedColor = Color.yellow;

    private Button _shopButton;

    private int _currentItemIndex;
    private int _nextItemInShopIndex;

    private void Awake()
    {
        _shopButton = GetComponent<Button>();
    }

    private void Start()
    {
        // Подписываемся на события
        ShopItemsManager.Instance.OnNewItemSpawnedInShop += ShopItemsManager_OnNewItemSpawnedInShop;
        WindowManager.Instance.OnWindowOpened += WindowManager_OnWindowOpened;
        GameManager.Instance.OnPlayerMoneyChanged += GameManager_OnPlayerMoneyChanged;

        // Инициализируем индексы
        UpdateItemIndices();
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        if (WindowManager.Instance != null)
            WindowManager.Instance.OnWindowOpened -= WindowManager_OnWindowOpened;

        if (ShopItemsManager.Instance != null)
        {
            ShopItemsManager.Instance.OnNewItemSpawnedInShop -= ShopItemsManager_OnNewItemSpawnedInShop;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerMoneyChanged -= GameManager_OnPlayerMoneyChanged;
    }

    private void UpdateItemIndices()
    {
        if (ShopItemsManager.Instance == null || ShopItemsManager.Instance.CurrentItem == null)
            return;

        _currentItemIndex = ShopItemsManager.Instance.CurrentItem.ItemIndex;
        _nextItemInShopIndex = _currentItemIndex + 1;

        // Проверяем, доступен ли уже следующий предмет для подсветки
        CheckAndUpdateButtonColor();
    }

    private void CheckAndUpdateButtonColor()
    {
        bool shouldHighlight = ShouldHighlightButton();

        ColorBlock colorBlock = _shopButton.colors;
        colorBlock.normalColor = shouldHighlight ? _highlightedColor : _normalColor;
        _shopButton.colors = colorBlock;
    }

    private bool ShouldHighlightButton()
    {
        // Проверяем, существует ли следующий предмет
        if (_nextItemInShopIndex >= ShopItemsManager.Instance.AllItemsInShop.Length)
            return false;

        // Проверяем, открыт ли следующий предмет в магазине
        bool isNextItemInShop = ShopItemsManager.Instance.OpenedItems.Contains(_nextItemInShopIndex);
        if (!isNextItemInShop)
            return false;

        // Проверяем, хватает ли денег на покупку
        int playerMoney = GameManager.Instance.PlayerMoneyAmount;
        int itemPrice = ShopItemsManager.Instance.AllItemsInShop[_nextItemInShopIndex].ItemPrice;

        return playerMoney >= itemPrice;
    }

    private void GameManager_OnPlayerMoneyChanged(int playerMoney)
    {
        CheckAndUpdateButtonColor();
    }

    private void ShopItemsManager_OnNewItemSpawnedInShop(ShopItem newItem)
    {
        // Обновляем индексы при появлении нового предмета
        UpdateItemIndices();
    }

    private void WindowManager_OnWindowOpened(WindowManager.WindowType windowType)
    {
        if (windowType == WindowManager.WindowType.Store)
        {
            // При открытии магазина обновляем индексы
            UpdateItemIndices();
        }
    }
}