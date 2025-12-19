using UnityEngine;

public class ResetBackgroundMusci : MonoBehaviour
{
    [SerializeField] private AudioClip _defaultBgMusic;

    private void Start()
    {
        BackgroundMusic.Instance.SetBackgroundMusic(_defaultBgMusic);
    }
}
