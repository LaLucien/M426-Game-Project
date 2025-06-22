using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    //Make sure to attach these Buttons in the Inspector
    public Button m_PlayerOne, m_PlayerTwo, m_PlayerThree, m_Quit;
    [SerializeField] TMP_Text m_PlayerOneText;
    [SerializeField] TMP_Text m_PlayerTwoText;
    [SerializeField] TMP_Text m_PlayerThreeText;
    private StorageManager m_storageManager;

    void Start()
    {
        m_storageManager = GetComponent<StorageManager>();
        m_PlayerOneText.text = GetText(0);
        m_PlayerTwoText.text = GetText(1);
        m_PlayerThreeText.text = GetText(2); ;


        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        m_PlayerOne.onClick.AddListener(()=>TaskOnClick(0));
        m_PlayerTwo.onClick.AddListener(()=>TaskOnClick(1));
        m_PlayerThree.onClick.AddListener(()=>TaskOnClick(2));
        m_Quit.onClick.AddListener(() => Application.Quit());

    }

    private string GetText(int playerId)
    {
        PlayerData data  = m_storageManager.ReadData(playerId);
        string name = string.IsNullOrEmpty(data.Name)?$"Player {playerId +1}":data.Name;
        return $"{name}, Highscore: {data.Highscore}";

    }

    public void TaskOnClick(int playerId)
    {
        SceneManager.LoadScene("Arena");
        StaticClass.Player = playerId;
    }
}
