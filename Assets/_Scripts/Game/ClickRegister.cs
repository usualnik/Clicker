using DG.Tweening;
using System;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

public class ClickRegister : MonoBehaviour, IPointerClickHandler
{
    public static ClickRegister Instance { get; private set; }
    public event Action OnClick;

    [SerializeField] private Image _characterSoapPreEndingImage;
    [SerializeField] private Image _characterImage;
    [SerializeField] private ParticleSystem _clickParticles;

    [Header("Animation Settings")]
    [SerializeField] private float _clickScale = 0.9f;
    [SerializeField] private float _clickDuration = 0.1f;
    [SerializeField] private float _returnDuration = 0.05f;
    [SerializeField] private Color _clickColor = new Color(0.8f, 0.8f, 1f, 1f);
    [SerializeField] private float _shakeStrength = 10f;
    [SerializeField] private int _shakeVibrato = 10;
    [SerializeField] private float _shakeDuration = 0.3f;

    [Header("Animation Special Case Settings")]
    [SerializeField] private float _clickScaleSoapPreEnding = 1.1f;
    [SerializeField] private float _clickDurationSoapPreEnding = 0f;




    [Header("Mobile Settings")]
    [SerializeField] private float _mobileClickCooldown = 0.2f;
    [SerializeField] private bool _enableMobileCooldown = true;

    [Header("System")]
    [SerializeField] private UI_AdWarningText _adWarningText;
    [SerializeField] private GameplayText _gameplayText;


    private Vector3 _originalScale;
    private Color _originalColor;
    private DG.Tweening.Sequence _clickSequence;
    private bool _isOnCooldown = false;
    private bool _isMobilePlatform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("More than one instance of click register");
        }

        _originalScale = _characterImage.transform.localScale;
        _originalColor = _characterImage.color;

        _isMobilePlatform = Application.isMobilePlatform;

    }

    private void Start()
    {
        Narrator.Instance.OnSoapPreEndingStarted += Narrator_OnSoapPreEndingStarted;
    }

    private void OnDestroy()
    {
        Narrator.Instance.OnSoapPreEndingStarted -= Narrator_OnSoapPreEndingStarted;

        if (_clickSequence != null && _clickSequence.IsActive())
        {
            _clickSequence.Kill();
        }

        CancelInvoke(nameof(ResetCooldown));
    }

    private void Narrator_OnSoapPreEndingStarted()
    {
        _clickDuration = _clickDurationSoapPreEnding;
        _clickScale = _clickScaleSoapPreEnding;
        _characterImage = _characterSoapPreEndingImage;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (_adWarningText.IsTimerRunning)
            return;

        if (_isMobilePlatform && _enableMobileCooldown && _isOnCooldown)
            return;

        if (_isMobilePlatform && _enableMobileCooldown)
            StartCooldown();

        OnClick?.Invoke();
        PlayClickAnimation();
    }

    private void StartCooldown()
    {
        _isOnCooldown = true;
        Invoke(nameof(ResetCooldown), _mobileClickCooldown);
    }

    private void ResetCooldown()
    {
        _isOnCooldown = false;
    }

    private void PlayClickAnimation()
    {
        if (_clickParticles != null && _clickParticles.isPlaying)
        {
            _clickParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (_clickSequence != null && _clickSequence.IsActive())
        {
            _clickSequence.Kill();
        }

        _clickSequence = DOTween.Sequence();

        // 1. Быстрое уменьшение
        _clickSequence.Append(_characterImage.transform.DOScale(_clickScale, _clickDuration)
            .SetEase(Ease.OutQuad));

        // 2. Эффект "вспышки" цвета
        _clickSequence.Join(_characterImage.DOColor(_clickColor, _clickDuration));

        // 3. Возврат к исходному состоянию
        _clickSequence.Append(_characterImage.transform.DOScale(_originalScale, _returnDuration)
            .SetEase(Ease.OutBack));

        // 4. Возврат цвета
        _clickSequence.Join(_characterImage.DOColor(_originalColor, _returnDuration));

        // 5. Легкая тряска после клика
        _clickSequence.Append(_characterImage.transform.DOShakePosition(
            _shakeDuration,
            strength: new Vector3(_shakeStrength, _shakeStrength, 0),
            vibrato: _shakeVibrato,
            randomness: 90,
            snapping: false
        ));

        if (_clickParticles != null)
        {
            _clickParticles.Play();
        }

        _clickSequence.Play();

        string clickSoundName = string.Empty;

        if (ShopItemsManager.Instance?.CurrentItem != null)
        {
            clickSoundName = ShopItemsManager.Instance.CurrentItem.ItemClickAudioClipName;
        }
        else
        {
            clickSoundName = "Button";
        }

        AudioManager.Instance?.Play(clickSoundName);
    }


    public void ForceResetCooldown()
    {
        CancelInvoke(nameof(ResetCooldown));
        _isOnCooldown = false;
    }
}