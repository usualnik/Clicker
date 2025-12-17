using System;
using UnityEngine;
using YG;

public class Narrator : MonoBehaviour
{
    public static Narrator Instance { get; private set; }
    public event Action<string> OnReplicaPrinted; 

    [Header("System Refs")]
    [SerializeField] private GameObject _beginig;
    [SerializeField] private GameObject _gamePlayText;
    [SerializeField] private GameObject _ending;

    private int _clicks;
    private int[] _clicksNeededToPrintMessage;
    private int _currentReplicaIndex = 0;

    private bool _isBeginingShouldStart = true;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            Debug.LogError("More than one instance of Narrator");
        }
    }
    private void Start()
    {
        InitBegining();
        LoadClicksAmount();
        ReConfigureNarrativeData(ShopItemsManager.Instance.CurrentItem);

        ShopItemsManager.Instance.OnShopItemBought += ShopItemsManager_OnShopItemBought;
        ClickRegister.Instance.OnClick += ClickRegister_OnClick;
        GameManager.Instance.OnWinGame += GameManager_OnWinGame;

    }


    private void OnDestroy()
    {
        ClickRegister.Instance.OnClick -= ClickRegister_OnClick;
        ShopItemsManager.Instance.OnShopItemBought -= ShopItemsManager_OnShopItemBought;
    }
    private void LoadClicksAmount()
    {
        _clicks = PlayerData.Instance.GetCurrentClicksAmount();
    }

    private void ShopItemsManager_OnShopItemBought(ShopItem obj)
    {
        ReConfigureNarrativeData(obj);
    }
    private void ClickRegister_OnClick()
    {
        OnClick();
    }
    private void GameManager_OnWinGame()
    {
        InitEnding();
    }

    private void InitBegining()
    {
        _beginig.SetActive(PlayerData.Instance.GetIsFirstTimePlaying());

        PlayerData.Instance.ChangeFisrtTimePlaying(false);
    }
    private void InitEnding()
    {
        _ending.SetActive(true);
    }
    private void ReConfigureNarrativeData(ShopItem shopItem)
    {
        _clicks = 0;
        _currentReplicaIndex = 0;
        _clicksNeededToPrintMessage = shopItem.ClicksNeededToPrintText;
    }
    private void OnClick()
    {
        _clicks += 1;

        PlayerData.Instance.SetCurrentClicks(_clicks);
     
        if (_currentReplicaIndex >= _clicksNeededToPrintMessage.Length)
        {
            return;
        }


        if (_clicks >= _clicksNeededToPrintMessage[_currentReplicaIndex])
        {
            PrintReplica();
            _currentReplicaIndex++;

        }
    }
    private void PrintReplica()
    {
        // Вызвать окно и напечатать реплику
        _gamePlayText.gameObject.SetActive(true);

        string replicaToPrint = YG2.envir.language == "ru" ? ShopItemsManager.Instance.CurrentItem.ReplicsRU[_currentReplicaIndex]
            : ShopItemsManager.Instance.CurrentItem.ReplicsEN[_currentReplicaIndex];

        _gamePlayText.GetComponent<GameplayText>().PrintReplica(replicaToPrint);

        OnReplicaPrinted?.Invoke(replicaToPrint);

    }
}
