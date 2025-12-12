using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    public int ItemPrice => _itemPrice;
    public int ItemIncomeIncrease => _itemIncomeIncrese;
    public bool IsCanBeBought => _isCanBeBought;

    [SerializeField] private string _shopItemName;
    [SerializeField] private int _itemIndex;
    [SerializeField] private int _itemPrice;
    [SerializeField] private int _itemIncomeIncrese;

    private bool _isCanBeBought = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        TryBuyItem();
    }

    private void TryBuyItem()
    {
        if (GameManager.Instance.PlayerMoneyAmount - _itemPrice >= 0 && _isCanBeBought)
        {
            BuyItem();
        }
    }

    private void BuyItem()
    {
        ShopItemsManager.Instance.PlayerBuyItem(this);
        _isCanBeBought = false;
    }
}
