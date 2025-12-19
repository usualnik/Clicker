using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemsManager : MonoBehaviour
{
    public static ShopItemsManager Instance { get; private set; }
    public ShopItem[] AllItemsInShop => _allItemsInShop;

    public event Action<ShopItem> OnShopItemBought;
    public event Action<ShopItem> OnNewItemSpawnedInShop;
    public ShopItem CurrentItem => _currentItem;

    [SerializeField] private ShopItem _currentItem;
    [SerializeField] private ShopItem[] _allItemsInShop;

    [SerializeField]
    private int _currentItemsAvailableInShop = 2;
    private int _currentItemIndex = 0;

    private HashSet<int> _openedItems = new HashSet<int>();

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
        LoadItemsAvailable();
        LoadCurrentItem();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerMoneyChanged -= GameManager_OnPlayerMoneyChanged;
        }
    }

    private void GameManager_OnPlayerMoneyChanged(int obj)
    {
        //TryOpenNewItem(obj);
    }

    private void LoadCurrentItem()
    {
        _currentItem = _allItemsInShop[PlayerData.Instance.GetCurrentItemIndex()];

        if (_currentItem)
        {
            _currentItem.SetSellectedColor();
        }
    }

    private void LoadItemsAvailable()
    {
        _currentItemsAvailableInShop = PlayerData.Instance.GetItemsAvailableInShop();
        _currentItemIndex = PlayerData.Instance.GetCurrentItemIndex();

        foreach (var item in _allItemsInShop)
        {
            if (PlayerData.Instance.GetItemsBought().Contains(item.ItemIndex))
            {
                item.SetCanBeBought(false);
            }
            else
            {
                item.SetCanBeBought(true);
            }
        }

        InitAvailableItems();
    }

    private void InitAvailableItems()
    {
        for (int i = 0; i < _currentItemsAvailableInShop; i++)
        {
            if (i < _allItemsInShop.Length)
            {
                _allItemsInShop[i].gameObject.SetActive(true);
                _openedItems.Add(_allItemsInShop[i].ItemIndex);
            }
        }

        foreach (var item in _allItemsInShop)
        {
            if (PlayerData.Instance.GetItemsBought().Contains(item.ItemIndex))
            {
                item.SetCanBeBought(false);
            }
        }
    }

    public void PlayerBuyItem(ShopItem shopItem)
    {
        AudioManager.Instance.Play("Buy");
        OnShopItemBought?.Invoke(shopItem);

        _currentItem = shopItem;
        _currentItemIndex = _currentItem.ItemIndex;

        HandleItemsColor();

        // Set color to green 
        _currentItem.SetSellectedColor();


        OpenNextItemInShop();

        PlayerData.Instance.SetNewCurrentItemIndex(_currentItemIndex);
        PlayerData.Instance.AddNewItemToBoughtList(_currentItem.ItemIndex);
    }

    private void HandleItemsColor()
    {
        foreach(var item in _allItemsInShop)
        {
            if (item.isActiveAndEnabled)
            {
                item.SetDeSelectedColor();
            }
        }
    }

    private void TryOpenNewItem(int playerMoneyAmount)
    {
        foreach (var item in _allItemsInShop)
        {
            if (!_openedItems.Contains(item.ItemIndex) &&
                item.IsCanBeBought &&
                playerMoneyAmount >= item.ItemPrice)
            {
                // Активируем предмет сразу при открытии
                item.gameObject.SetActive(true);
                _currentItemsAvailableInShop++;
                _openedItems.Add(item.ItemIndex);
                PlayerData.Instance.SetItemsAvailableInShop(_currentItemsAvailableInShop);
                OnNewItemSpawnedInShop?.Invoke(item);

                // Открываем следующий предмет сразу же
                OpenNextAvailableItem();
            }
        }
    }

    public void SetCurrentItem(ShopItem shopItem)
    {
        _currentItem = shopItem;
    }

    private void OpenNextItemInShop()
    {
        // Проверяем, есть ли следующий предмет
        if (_currentItem.ItemIndex + 1 < _allItemsInShop.Length)
        {
            int nextItemIndex = _currentItem.ItemIndex + 1;

            if (!_openedItems.Contains(nextItemIndex))
            {
                // Активируем следующий предмет
                _allItemsInShop[nextItemIndex].gameObject.SetActive(true);
                _currentItemsAvailableInShop++;
                _openedItems.Add(nextItemIndex);
                PlayerData.Instance.SetItemsAvailableInShop(_currentItemsAvailableInShop);
                OnNewItemSpawnedInShop?.Invoke(_allItemsInShop[nextItemIndex]);

                // Можем продолжить открывать следующие предметы, если нужно
                // OpenNextAvailableItem();
            }
        }
    }

    private void OpenNextAvailableItem()
    {
        for (int i = 0; i < _allItemsInShop.Length; i++)
        {
            if (!_openedItems.Contains(_allItemsInShop[i].ItemIndex) &&
                _allItemsInShop[i].IsCanBeBought)
            {
                _allItemsInShop[i].gameObject.SetActive(true);
                _currentItemsAvailableInShop++;
                _openedItems.Add(_allItemsInShop[i].ItemIndex);
                PlayerData.Instance.SetItemsAvailableInShop(_currentItemsAvailableInShop);
                OnNewItemSpawnedInShop?.Invoke(_allItemsInShop[i]);
                break;
            }
        }
    }

    public void OpenMultipleItems(int count)
    {
        int openedCount = 0;

        for (int i = 0; i < _allItemsInShop.Length && openedCount < count; i++)
        {
            if (!_openedItems.Contains(_allItemsInShop[i].ItemIndex))
            {
                _allItemsInShop[i].gameObject.SetActive(true);
                _openedItems.Add(_allItemsInShop[i].ItemIndex);
                openedCount++;

                OnNewItemSpawnedInShop?.Invoke(_allItemsInShop[i]);
            }
        }

        _currentItemsAvailableInShop += openedCount;
        PlayerData.Instance.SetItemsAvailableInShop(_currentItemsAvailableInShop);
    }
}