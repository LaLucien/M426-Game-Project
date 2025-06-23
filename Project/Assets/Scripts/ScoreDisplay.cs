using TMPro;
using UnityEngine;


public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] public TMP_Text m_scoreText;
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

    public void UpdateText(PlayerData playerData)
        => m_scoreText.text = $"Score: {playerData.Score}\nHighscore: {playerData.Highscore}";

    
}
