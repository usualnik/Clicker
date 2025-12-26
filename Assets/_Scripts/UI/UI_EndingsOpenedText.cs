using TMPro;
using UnityEngine;
using YG;

public class UI_EndingsOpenedText : MonoBehaviour
{
    private string _endingsOpenedTextRU;
    private string _endingsOpenedTextEN;

    private int _endingsOpened;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
       
    }
    private void Start()
    {
        _endingsOpened = PlayerData.Instance.GetEndingsOpened();

        _endingsOpenedTextRU = string.Format($"Концовок открыто: {_endingsOpened}/6. Пройдите игру другими предметами, чтобы открыть все!");
        _endingsOpenedTextEN = string.Format($"Endings unlocked: {_endingsOpened}/6. Complete the game using other items to unlock them all!");

        _text.text = YG2.envir.language == "ru" ? _endingsOpenedTextRU : _endingsOpenedTextEN;
    }
}
