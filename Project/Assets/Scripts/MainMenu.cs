using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    //Make sure to attach these Buttons in the Inspector
    public Button m_PlayerOne, m_PlayerTwo, m_PlayerThree;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        m_PlayerOne.onClick.AddListener(TaskOnClick);
        
    }
    public void TaskOnClick()
    {
        SceneManager.LoadScene("Arena");
        StaticClass.Player = 0;
    }
}
