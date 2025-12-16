using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Beggining : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private float letterDelay = 0.03f;

    [Header("BeginingBackgrounds")]
    [SerializeField] private Sprite _bg;
    private Image _background;

    private const int SHOULD_CHANGE_BG = 3;

    private string[] _beginingReplicsRU =
    {
        "Ты думал твои долги просто пропадут?",
        "30000 завтра, либо мы придём за тобой.",
        "Ты замираешь. Откуда тебе достать столько денег?",
        "Ты думал об этом по дороге домой, когда увидел небольшую будку.",
        "Там сидит привлекательная девушка. Ты подходишь.",
        "Привет! Я Анна. Добро пожаловать в мою будку!",
        "За каждый раз, когда ты касаешься меня, я выдаю тебе долларовую купюру. Хорошо звучит, нет?",
        "Кто знает, может есть способы увеличить ценность купюр...",
        "Впрочем, давай, попробуй. Коснись меня!"
    };

    private string[] _beginingReplicsEN =
    {
        "Did you think your debts would just disappear?",
        "30000 by tomorrow, or we'll come for you.",
        "You freeze. Where are you supposed to get that much money?",
        "You were thinking about this on the way home when you saw a small booth.",
        "There is an attractive girl sitting there. You walk up to her.",
        "Hi! I am Anna. Welcome to my booth!",
        "Every time you tap me, I give you a one dollar bill. Sounds good, right?",
        "Who knows, maybe there are ways to increase the value of the bills...",
        "Anyway, come on, try it. Touch me!"
    };

    private TextMeshProUGUI _textBox;
    private Coroutine _typingRoutine;

    private int _currentReplicaIndex;
    private bool _isTyping;
    private string[] CurrentReplics =>
    YG2.envir.language == "ru" ? _beginingReplicsRU : _beginingReplicsEN;

    private void Awake()
    {
        _textBox = GetComponentInChildren<TextMeshProUGUI>();
        _background = GetComponent<Image>();

    }

    private void OnEnable()
    {
        _currentReplicaIndex = 0;
        ShowReplica();
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

        _typingRoutine = StartCoroutine(TypeText(CurrentReplics[_currentReplicaIndex]));
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

        _textBox.text = CurrentReplics[_currentReplicaIndex];
        _isTyping = false;
    }

    private void NextReplica()
    {
        _currentReplicaIndex++;

        if (_currentReplicaIndex == SHOULD_CHANGE_BG)
            ChangeBackground();

        if (_currentReplicaIndex >= CurrentReplics.Length)
        {
            gameObject.SetActive(false);
            return;
        }

        ShowReplica();
    }

    private void ChangeBackground()
    {
        _background.sprite = _bg;
    }
}