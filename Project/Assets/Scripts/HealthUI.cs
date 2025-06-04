// HealthUI.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Health))]
public class HealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI healthText = null;

    private Health m_health;

    void Awake()
    {
        m_health = GetComponent<Health>();

        if (healthText == null)
            Debug.LogWarning($"[{nameof(HealthUI)}] No TextMeshProUGUI assigned on '{gameObject.name}'.");
    }

    void OnEnable()
    {
        m_health.OnHealthChanged += OnHealthChangedHandler;

        OnHealthChangedHandler(m_health.CurrentHealth, m_health.MaxHealth);
    }

    void OnDisable()
    {
        m_health.OnHealthChanged -= OnHealthChangedHandler;
    }

    /// <summary>
    /// Called whenever Health.InvokeOnHealthChanged(current, max) fires.
    /// </summary>
    private void OnHealthChangedHandler(int current, int max)
    {
        if (healthText != null)
            healthText.text = $"Health: {current} / {max}";
    }
}
