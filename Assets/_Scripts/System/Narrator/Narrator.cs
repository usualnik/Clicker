using UnityEngine;
using YG;

public class Narrator : MonoBehaviour
{
    public static Narrator Instance { get; private set; }

    [Header("Props")]
    [SerializeField] private int _clicks;
    [SerializeField] private int[] _clicksNeededToPrintMessage;
    [SerializeField] private int _currentReplicaIndex = 0;



    [Header("System Refs")]
    [SerializeField] private GameObject _beginig;
    [SerializeField] private GameObject _gamePlayText;
    [SerializeField] private GameObject _ending;

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
        ReConfigureNarrativeData(ShopItemsManager.Instance.CurrentItem);

        ShopItemsManager.Instance.OnShopItemBought += ShopItemsManager_OnShopItemBought;
        ClickRegister.Instance.OnClick += ClickRegister_OnClick;

    }
    private void OnDestroy()
    {
        ClickRegister.Instance.OnClick -= ClickRegister_OnClick;
        ShopItemsManager.Instance.OnShopItemBought -= ShopItemsManager_OnShopItemBought;

    }

    private void ShopItemsManager_OnShopItemBought(ShopItem obj)
    {
        ReConfigureNarrativeData(obj);
    }
    private void ClickRegister_OnClick()
    {
        OnClick();
    }

    private void InitBegining()
    {
        _beginig.SetActive(_isBeginingShouldStart);
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

    }
}
