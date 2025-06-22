using System;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    [SerializeField] TMP_InputField m_nameInput;
    [SerializeField] Button m_continueButton;
    private StorageManager m_storageManager;
    private PlayerData m_playerData;
    private ScoreDisplay m_scoreDisplay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // getting the components
        m_storageManager = GetComponent<StorageManager>();
        m_scoreDisplay = GetComponent<ScoreDisplay>();

        //load adn display player data
        m_playerData = m_storageManager.ReadData(StaticClass.Player);
        m_nameInput.text = string.IsNullOrEmpty(m_playerData.Name)?$"Player {StaticClass.Player + 1}":m_playerData.Name;
        m_scoreDisplay.UpdateText(m_playerData);

        //Events
        //m_nameInput.onValueChanged.AddListener(ChangedName);
        m_continueButton.onClick.AddListener(Continue);
    }

    private void Continue()
    {
        m_playerData.Name = m_nameInput.text;
        m_storageManager.WritePlayerData(StaticClass.Player, m_playerData);
        SceneManager.LoadScene("MainMenu");
    }

    //void ChangedName(string value)
    //{
    //    m_playerData.Name = value;
    //    m_storageManager.WritePlayerData(StaticClass.Player, m_playerData);
    //}
}
