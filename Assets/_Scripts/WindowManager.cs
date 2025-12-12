using System;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance { get; private set; }
    public event Action<WindowType> OnWindowOpened;
    public enum WindowType
    {
        Gameplay = 0,
        Store = 1,
    }
    private WindowType _windowType;    

    [Header("Menu windows refs")]
    [SerializeField] private CanvasGroup[] _windowCanvasGroups;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of WindowManager");
        }
    }

    private void Start()
    {

        OpenWindow((int)WindowType.Gameplay);
    }   

    public void OpenWindow(int index)
    {
        if (index < 0 || index >= _windowCanvasGroups.Length)
        {
            Debug.LogError($"Window index {index} is out of range!");
            return;
        }

        foreach (var canvasGroup in _windowCanvasGroups)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        _windowCanvasGroups[index].alpha = 1;
        _windowCanvasGroups[index].interactable = true;
        _windowCanvasGroups[index].blocksRaycasts = true;


        ChangeCurrentWindowType(index);
        OnWindowOpened?.Invoke(_windowType);
    }

    private void ChangeCurrentWindowType(int index)
    {
        switch (index)
        {
            case 0:
                _windowType = WindowType.Gameplay; break;
            case 1:
                _windowType = WindowType.Store; break;
            default:
                break;
        }
    }
}
