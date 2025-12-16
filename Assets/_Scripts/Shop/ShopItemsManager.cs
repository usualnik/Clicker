using System;
using UnityEngine;

public class ShopItemsManager : MonoBehaviour
{
    public static ShopItemsManager Instance {  get; private set; }

    public event Action<ShopItem> OnShopItemBought;
    public event Action<ShopItem> OnNewItemSpawnedInShop;
    public ShopItem CurrentItem => _currentItem;

    [SerializeField] private ShopItem _currentItem;

    [SerializeField] private ShopItem[] _allItemsInShop;

    [SerializeField]
    private int _currentItemsAvailableInShop = 1;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("More than one instance of game manager");
        }
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerMoneyChanged += GameManager_OnPlayerMoneyChanged;

        InitAvailableItems();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerMoneyChanged -= GameManager_OnPlayerMoneyChanged;

    }

    private void GameManager_OnPlayerMoneyChanged(int obj)
    {
        TryOpenNewItem(obj);
    }

    private void InitAvailableItems()
    {
        for (int i = 0; i < _currentItemsAvailableInShop; i++)
        {
            _allItemsInShop[i].gameObject.SetActive(true);
        }
    }

    public void PlayerBuyItem(ShopItem shopItem)
    {
        AudioManager.Instance.Play("Buy");
        OnShopItemBought?.Invoke(shopItem);
        _currentItem = shopItem;
    }

    private void TryOpenNewItem(int playerMoneyAmount)
    {
        foreach (var item in _allItemsInShop)
        {
            if (item.IsCanBeBought && playerMoneyAmount >= item.ItemPrice)
            {
                item.gameObject.SetActive(true);
                _currentItemsAvailableInShop++;
                OnNewItemSpawnedInShop?.Invoke(item);

            }
        }
    }

    public void SetCurrentItem(ShopItem shopItem)
    {
        _currentItem = shopItem;
    }

}
