using UnityEngine;
using YG;

public class YandexInterstitial : MonoBehaviour
{
    [SerializeField] private GameObject _adWarning;

    private const float AD_COOLDOWN_MAX = 65f;
    private const float AD_WARNING_TIME = 2f;

    private float adTimer;

    private void Awake()
    {
        adTimer = AD_COOLDOWN_MAX;
    }

    private void Update()
    {
        adTimer -= Time.deltaTime;

        if (adTimer <= AD_WARNING_TIME)
        {
            ShowAdWarning();

            if (adTimer <= 0)
            {
                ShowInterstitial();
                adTimer = AD_COOLDOWN_MAX;
                HideAdWarning();
            }

        }
    }

    private void ShowInterstitial()
    {
        YG2.InterstitialAdvShow();
    }

    private void ShowAdWarning()
    {
        _adWarning.SetActive(true);
    }
    private void HideAdWarning()
    {
        _adWarning.SetActive(false);
    }
}
