using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayText : MonoBehaviour
{
    [SerializeField] private float _letterDelay;
    [SerializeField] private float _hideTextBoxDelay = 2.0f;

    private TextMeshProUGUI _gameplayText;
    private bool _isTyping;
    private string _text;

    private Coroutine _typingRoutine;

    private void Awake()
    {
        _gameplayText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void PrintReplica(string replica)
    {
        if (_typingRoutine != null)
            StopCoroutine(_typingRoutine);

        _text = replica;

        _typingRoutine = StartCoroutine(TypeText(replica));
    }

    private void OnDisable()
    {
        if (_typingRoutine != null)
            StopCoroutine(_typingRoutine);

        _typingRoutine = null;
    }

    //private void Update()
    //{
    //    if (Input.anyKeyDown)
    //    {
    //        OnClick();
    //    }
    //}

    //private void OnClick()
    //{
    //    if (_isTyping)
    //    {
    //        SkipTyping();
    //    }
    //}

    private IEnumerator TypeText(string text)
    {
        _isTyping = true;
        _gameplayText.text = "";

        foreach (char c in text)
        {
            _gameplayText.text += c;
            yield return new WaitForSeconds(_letterDelay);
        }

        _isTyping = false;
        Invoke(nameof(FinishTyping), _hideTextBoxDelay);
    }

    private void SkipTyping()
    {
        if (_typingRoutine != null)
            StopCoroutine(_typingRoutine);

        _gameplayText.text = _text;

        Invoke(nameof(FinishTyping), _hideTextBoxDelay);
    }

    private void FinishTyping()
    {
        if (_isTyping)
        {
            Invoke(nameof(FinishTyping), _hideTextBoxDelay);

        }
        else
        {
            _isTyping = false;
            _typingRoutine = null;
            gameObject.SetActive(false);
        }
        
    }

}
