using TMPro;
using UnityEngine;

public class UI_ShopItemPriceText : MonoBehaviour
{
    private TextMeshProUGUI _priceText;
    private ShopItem _shopItem;

    private void Awake()
    {
        _priceText = GetComponent<TextMeshProUGUI>();
        _shopItem = GetComponentInParent<ShopItem>();

        _priceText.text = _shopItem.ItemPrice.ToString();
    }

    private void Start()
    {
        ShopItemsManager.Instance.OnShopItemBought += ShopItemsManager_OnShopItemBought;
        LoadCanBeBought();
    }
    private void OnDestroy()
    {
        ShopItemsManager.Instance.OnShopItemBought -= ShopItemsManager_OnShopItemBought;

    }
    private void LoadCanBeBought()
    {
        if (!_shopItem.IsCanBeBought)
        {
            DeactivateText();
        }
    }

    private void ShopItemsManager_OnShopItemBought(ShopItem obj)
    {
        if (obj == _shopItem)
        {
            DeactivateText();
        }
    }

    private void DeactivateText()
    {
        gameObject.SetActive(false);
    }
}
