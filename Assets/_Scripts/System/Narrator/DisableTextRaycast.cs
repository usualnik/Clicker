using TMPro;
using UnityEngine;

public class DisableTextRaycast : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().raycastTarget = false;
    }
}