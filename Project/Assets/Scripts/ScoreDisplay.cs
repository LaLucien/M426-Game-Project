using TMPro;
using UnityEngine;


public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text m_scoreText;
    //[SerializeField] private PlayerData m_playerHealth;

    //void OnEnable()
    //{
    //    //m_playerHealth.OnHealthChanged += UpdateText;
    //    //m_playerHealth.OnDied += OnDeath;
    //}

    //void Start()
    //{
    //    // Ensure the health text is initialized at start
    //    UpdateText(m_playerHealth.CurrentHealth, m_playerHealth.MaxHealth);
    //}

    public void UpdateText(int score, int highscore)
        => m_scoreText.text = $"Score:{score}, Highscore: {highscore}";

    
}
