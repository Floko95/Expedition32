using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
    
    [SerializeField] private Slider hpSlider;
    [SerializeField] private bool hideOnStart;
    
    [SerializeField] private TextMeshProUGUI tmpHealth;
    [SerializeField] private TextMeshProUGUI tmpMaxHealth;
    [SerializeField] private Slider delayedDamageSlider;
    [SerializeField] private float delay;
    [SerializeField] private float delayLerpTime;

    public HealthSystem HealthSystem {
        get => _healthSystem;
        private set => SetHealthSystem(value);
    }

    private HealthSystem _healthSystem;
    
    private void Start() {
        if (hideOnStart) {
            Hide();
        }
    }

    private void OnDestroy() {
        if (delayedDamageSlider) delayedDamageSlider.DOKill();
    }

    public void SetHealthSystem(HealthSystem healthSystem) {
        if (_healthSystem != null) {
            _healthSystem.OnHealthChanged -= UpdateHealthBar;
            _healthSystem.OnHealthChanged -= UpdateHealthText;
            _healthSystem.OnHealthMaxChanged -= UpdateHealthText;
            _healthSystem.OnDamaged -= OnDamaged;
            _healthSystem.OnDead -= Hide;
        }

        _healthSystem = healthSystem;

        _healthSystem.OnHealthChanged += UpdateHealthBar;
        _healthSystem.OnHealthChanged += UpdateHealthText;
        _healthSystem.OnHealthMaxChanged += UpdateHealthText;
        _healthSystem.OnDamaged += OnDamaged;
        _healthSystem.OnDead += Hide;

        UpdateHealthBar();
        UpdateHealthText();
    }

    private void UpdateHealthBar() {
        gameObject.SetActive(true);
        hpSlider.value = _healthSystem.GetHealthNormalized();
    }

    private void UpdateHealthText() {
        if (tmpHealth) tmpHealth.text = _healthSystem.GetHealth().ToString();
        if (tmpMaxHealth) tmpMaxHealth.text = _healthSystem.GetHealthMax().ToString();
    }

    private void OnDamaged() {
        if (delayedDamageSlider)
            delayedDamageSlider.DOValue(_healthSystem.GetHealthNormalized(), delayLerpTime).SetDelay(delay);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}

