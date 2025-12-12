using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public event Action<int> OnPlayerMoneyChanged;

    private int _playerMoneyAmount = 0;
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
    }
    private void OnDestroy()
    {
        ClickRegister.Instance.OnClick -= ClickRegister_OnClick;
    }

    private void ClickRegister_OnClick()
    {
        IncreasePlayerMoneyOnClick();
    }

    private void IncreasePlayerMoneyOnClick()
    {
        _playerMoneyAmount += _playerIncome;

        OnPlayerMoneyChanged?.Invoke(_playerMoneyAmount);
    }

}
