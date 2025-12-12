using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public event Action<int> OnPlayerMoneyChanged;
    public event Action<int> OnPlayerIncomeChanged;
    public int PlayerMoneyAmount { get; private set; } = 0;

    private int _playerIncome = 1;
    
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
    }   

    private void OnDestroy()
    {
        ClickRegister.Instance.OnClick -= ClickRegister_OnClick;
        ShopItemsManager.Instance.OnShopItemBought -= ShopItemManager_OnShopItemBought;

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
    }


    private void SpendPlayerMoney(int value)
    {
        if (PlayerMoneyAmount - value >= 0)
        {
            PlayerMoneyAmount -= value;
            OnPlayerMoneyChanged?.Invoke(PlayerMoneyAmount);
        }
    }



}
