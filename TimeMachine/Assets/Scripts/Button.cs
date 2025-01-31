using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void levels()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void level1()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void level2()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void quit()
    {
        Application.Quit();
    }
}
