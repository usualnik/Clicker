using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    public int ItemPrice => _itemPrice;
    public int ItemIndex => _itemIndex;
    public int ItemIncomeIncrease => _itemIncomeIncrese;
    public bool IsCanBeBought => _isCanBeBought;
    public string ItemClickAudioClipName => _itemClickAudioClipName;
    public int[] ClicksNeededToPrintText => _clicksNeeded;
    public string[] ReplicsRU => _replicsRU;
    public string[] ReplicsEN => _replicsEN;
    public string[] EndingReplicsRU => _endingReplicsRU;
    public string[] EndingReplicsEN => _endingReplicsEN;
    public Sprite[] EndingBackgrounds => _endingBackgrounds;
    public string EndingAudioName => _endingAudioName;

    [Header("Text data")]
    [Tooltip("Как много кликов нужно, чтобы текст предмета начал проигрываться")][SerializeField] private int[] _clicksNeeded;
    [TextArea][SerializeField] private string[] _replicsRU;
    [TextArea][SerializeField] private string[] _replicsEN;
    
    [Header("Ending with this item")]
    [SerializeField] private Sprite[] _endingBackgrounds;
    [TextArea][SerializeField] private string[] _endingReplicsRU;
    [TextArea][SerializeField] private string[] _endingReplicsEN;
    [SerializeField] private string _endingAudioName;

    [Header("Item Data")] 
    [SerializeField] private string _shopItemName;
    [SerializeField] private int _itemIndex;
    [SerializeField] private int _itemPrice;
    [SerializeField] private int _itemIncomeIncrese;
    [SerializeField] private string _itemClickAudioClipName;

    [SerializeField] private bool _isCanBeBought = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isCanBeBought)
        {
            TryBuyItem();
        }
        // Больше свапать предметы нельзя
        //else
        //{
        //    ShopItemsManager.Instance.SetCurrentItem(this);
        //}
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

    public void SetCanBeBought(bool isCanBeBought)
    {
        _isCanBeBought = isCanBeBought;
    }
}
