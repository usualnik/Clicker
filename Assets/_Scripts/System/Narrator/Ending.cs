using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Ending : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private float letterDelay = 0.03f;

    [Header("EndingBackgrounds")]
    [SerializeField] private Sprite[] _endingBackgrounds;

    [Header("System refs")]
    [SerializeField] private UI_RestartButton _restartButton;


    private int _endingBackgroundIndex = 0;
    private Image _background;

    private string[] _endingReplicsRU;
    private string[] _endingReplicsEN;

    private TextMeshProUGUI _textBox;
    private Coroutine _typingRoutine;

    private int _currentReplicaIndex;
    private bool _isTyping;
    private string[] _currentReplics;

    


    private void Awake()
    {
        _textBox = GetComponentInChildren<TextMeshProUGUI>();
        _background = GetComponent<Image>();
    }

    private void OnEnable()
    {
        InitText();
        InitBackground();

        AudioManager.Instance.Play(ShopItemsManager.Instance.CurrentItem.ItemClickAudioClipName);

        _currentReplicaIndex = 0;
        ShowReplica();
    }

    private void InitBackground()
    {
        _background.sprite = _endingBackgrounds[0];
    }
    private void InitText()
    {
        _endingReplicsRU = ShopItemsManager.Instance.CurrentItem.EndingReplicsRU;
        _endingReplicsEN = ShopItemsManager.Instance.CurrentItem.EndingReplicsEN;
        _endingBackgrounds = ShopItemsManager.Instance.CurrentItem.EndingBackgrounds;

        _currentReplics = YG2.envir.language == "ru" ? _endingReplicsRU : _endingReplicsEN;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            OnClick();
        }
    }

    private void OnClick()
    {
        if (_isTyping)
        {
            SkipTyping();
        }
        else
        {
            NextReplica();
        }
    }

    private void ShowReplica()
    {
        if (_typingRoutine != null)
            StopCoroutine(_typingRoutine);

        _typingRoutine = StartCoroutine(TypeText(_currentReplics[_currentReplicaIndex]));
    }

    private IEnumerator TypeText(string text)
    {
        _isTyping = true;
        _textBox.text = "";

        foreach (char c in text)
        {
            _textBox.text += c;
            yield return new WaitForSeconds(letterDelay);
        }

        _isTyping = false;
    }

    private void SkipTyping()
    {
        if (_typingRoutine != null)
            StopCoroutine(_typingRoutine);

        _textBox.text = _currentReplics[_currentReplicaIndex];
        _isTyping = false;
    }

    private void NextReplica()
    {
        _currentReplicaIndex++;

        if (_endingBackgrounds.Length > 1)
        {
            BadEndingChangeBackgrounds();
        }

        if (_currentReplicaIndex >= _currentReplics.Length)
        {
            _restartButton.gameObject.SetActive(true);
            return;
        }

        ShowReplica();
    }

    private void BadEndingChangeBackgrounds()
    {

        switch (_currentReplicaIndex)
        {
            case 0:
                _background.sprite = _endingBackgrounds[0];
                break;
            case 2:
                _background.sprite = _endingBackgrounds[1];
                break;
            case 5:
                _background.sprite = _endingBackgrounds[2];
                break;
            case 7:
                _background.sprite = _endingBackgrounds[3];
                break;
        }
    }


}
