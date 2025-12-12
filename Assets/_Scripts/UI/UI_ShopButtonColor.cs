using UnityEngine;
using UnityEngine.UI;

public class UI_ShopButtonColor : MonoBehaviour
{
    private Color _normalColor = Color.white;
    private Color _highlightedColor = Color.yellow;

    private Button _shopButton;

    private void Awake()
    {
        _shopButton = GetComponent<Button>();
    }

    private void Start()
    {
        ShopItemsManager.Instance.OnNewItemSpawnedInShop += ShopItemsManager_OnNewItemSpawnedInShop;
        WindowManager.Instance.OnWindowOpened += WindowManager_OnWindowOpened;
    }

    private void OnDestroy()
    {
        WindowManager.Instance.OnWindowOpened -= WindowManager_OnWindowOpened;
        ShopItemsManager.Instance.OnNewItemSpawnedInShop -= ShopItemsManager_OnNewItemSpawnedInShop;

    }
    private void ShopItemsManager_OnNewItemSpawnedInShop(ShopItem obj)
    {
        ColorBlock colorBlock = _shopButton.colors;
        colorBlock.normalColor = _highlightedColor;
        _shopButton.colors = colorBlock;
    }

    private void WindowManager_OnWindowOpened(WindowManager.WindowType obj)
    {
        if (obj == WindowManager.WindowType.Store)
        {
            ColorBlock colorBlock = _shopButton.colors;
            colorBlock.normalColor = _normalColor;
            _shopButton.colors = colorBlock;
        }
    }
}
