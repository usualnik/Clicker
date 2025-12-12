using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickRegister : MonoBehaviour, IPointerClickHandler
{
    public static ClickRegister Instance { get; private set; }
    public event Action OnClick;

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

    private Vector3 _originalScale;
    private Color _originalColor;
    private Sequence _clickSequence;

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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
        PlayClickAnimation();
    }

    private void PlayClickAnimation()
    {
        // Останавливаем предыдущую анимацию частиц
        if (_clickParticles != null && _clickParticles.isPlaying)
        {
            _clickParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // Убиваем предыдущую анимацию чтобы не было конфликтов
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

        // Запускаем частицы
        if (_clickParticles != null)
        {
            _clickParticles.Play();
        }

        // Запускаем анимацию
        _clickSequence.Play();
    }

    private void OnDestroy()
    {
        if (_clickSequence != null && _clickSequence.IsActive())
        {
            _clickSequence.Kill();
        }
    }
}