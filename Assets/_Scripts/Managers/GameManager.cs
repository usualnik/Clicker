using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public event Action<int> OnPlayerMoneyChanged;
    public event Action<int> OnPlayerIncomeChanged;
    public event Action OnWinGame;
    public int PlayerMoneyAmount { get; private set; } = 0;

    private int _playerIncome = 1;
    //private const int MONEY_AMOUNT_TO_WIN_GAME = 30000;
    private const int MONEY_AMOUNT_TO_WIN_GAME = 300;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("More than one instance of GameManager");
        }
    }

    private void Start()
    {
        ClickRegister.Instance.OnClick += ClickRegister_OnClick;
        ShopItemsManager.Instance.OnShopItemBought += ShopItemManager_OnShopItemBought;

        LoadMoneyAmount();
    }   

    private void OnDestroy()
    {
        ClickRegister.Instance.OnClick -= ClickRegister_OnClick;
        ShopItemsManager.Instance.OnShopItemBought -= ShopItemManager_OnShopItemBought;
    }

    private void LoadMoneyAmount()
    {
        PlayerMoneyAmount = PlayerData.Instance.GetCurrentMoneyAmount();       
        _playerIncome = ShopItemsManager.Instance.AllItemsInShop[PlayerData.Instance.GetCurrentItemIndex()].ItemIncomeIncrease;
    }

    private void ShopItemManager_OnShopItemBought(ShopItem shopItem)
    {
        SpendPlayerMoney(shopItem.ItemPrice);

        if (_playerIncome != shopItem.ItemIncomeIncrease)
        {
            _playerIncome = shopItem.ItemIncomeIncrease;
            OnPlayerIncomeChanged?.Invoke(_playerIncome);
        }

    }

    private void ClickRegister_OnClick()
    {
        IncreasePlayerMoneyOnClick();
    }

    private void IncreasePlayerMoneyOnClick()
    {
        PlayerMoneyAmount += _playerIncome;
        OnPlayerMoneyChanged?.Invoke(PlayerMoneyAmount);

        PlayerData.Instance.SetCurrentMoney(PlayerMoneyAmount);

        if (PlayerMoneyAmount >= MONEY_AMOUNT_TO_WIN_GAME)
        {
            OnWinGame?.Invoke();
        }

    }


    private void SpendPlayerMoney(int value)
    {
        if (PlayerMoneyAmount - value >= 0)
        {
            PlayerMoneyAmount -= value;
            OnPlayerMoneyChanged?.Invoke(PlayerMoneyAmount);
            PlayerData.Instance.SetCurrentMoney(PlayerMoneyAmount);
        }
    }



}
