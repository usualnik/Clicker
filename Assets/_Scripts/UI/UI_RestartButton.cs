using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_RestartButton : MonoBehaviour
{
    private Button _restartButton;

    private void Awake()
    {
        _restartButton = GetComponent<Button>();
    }

    private void Start()
    {
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(Restart);
        }
    }
    private void OnDestroy()
    {
        if (_restartButton != null)
        {
            _restartButton.onClick.RemoveListener(Restart);
        }
    }

    private void Restart()
    {
        PlayerData.Instance.HardResetPlayerData();
        Invoke(nameof(Reload), 0.5f);
    }
    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
