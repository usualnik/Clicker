using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickRegister : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
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

    [Header("System")]
    [SerializeField] private UI_AdWarningText _adWarningText;

    private Vector3 _originalScale;
    private Color _originalColor;
    private DG.Tweening.Sequence _clickSequence;
    private bool _isMobilePlatform;

    private bool _isTouching = false;
    private int _currentTouchId = -1;

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
        if (Narrator.Instance != null)
        {
            Narrator.Instance.OnSoapPreEndingStarted -= Narrator_OnSoapPreEndingStarted;
        }

        if (_clickSequence != null && _clickSequence.IsActive())
        {
            _clickSequence.Kill();
        }
    }

    private void Narrator_OnSoapPreEndingStarted()
    {
        _clickDuration = _clickDurationSoapPreEnding;
        _clickScale = _clickScaleSoapPreEnding;
        _characterImage = _characterSoapPreEndingImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isMobilePlatform)
        {
            HandleClick();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isMobilePlatform) return;

        if (!_isTouching)
        {
            _isTouching = true;
            _currentTouchId = eventData.pointerId;
            HandleClick();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isMobilePlatform) return;

        if (eventData.pointerId == _currentTouchId)
        {
            _isTouching = false;
            _currentTouchId = -1;
        }
    }

    private void HandleClick()
    {
        if (_adWarningText != null && _adWarningText.IsTimerRunning)
            return;

        OnClick?.Invoke();
        PlayClickAnimation();
    }

    private void PlayClickAnimation()
    {
        // Останавливаем предыдущую анимацию, если она есть
        if (_clickSequence != null && _clickSequence.IsActive())
        {
            _clickSequence.Kill();
        }

        // Создаем новую анимацию
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
            _clickParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _clickParticles.Play();
        }

        _clickSequence.Play();

        string clickSoundName = "Button";
        if (ShopItemsManager.Instance?.CurrentItem != null)
        {
            clickSoundName = ShopItemsManager.Instance.CurrentItem.ItemClickAudioClipName;
        }

        AudioManager.Instance?.Play(clickSoundName);
    }

    public void ResetTouchState()
    {
        _isTouching = false;
        _currentTouchId = -1;
    }
}