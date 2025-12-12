using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickRegister : MonoBehaviour, IPointerClickHandler
{
    public static ClickRegister Instance {  get; private set; }

    public event Action OnClick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("More than one insctance of click register");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
