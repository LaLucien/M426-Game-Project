using TMPro;
using UnityEngine;


public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text m_healthText;
    [SerializeField] private Health m_playerHealth;

    void OnEnable()
    {
        m_playerHealth.OnHealthChanged += UpdateText;
        m_playerHealth.OnDied += OnDeath;
    }

    void Start()
    {
        // Ensure the health text is initialized at start
        UpdateText(m_playerHealth.CurrentHealth, m_playerHealth.MaxHealth);
    }

    private void UpdateText(int current, int max)
        => m_healthText.text = $"Health: {current}/{max}";

    private void OnDeath()
        => m_healthText.text = "Health: DEAD";
}
