using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void startGame()
    {
        StartCoroutine(WaitSecond("LevelSelection"));
        // SceneManager.LoadScene("Level_1");
    }

    public void levels()
    {
        StartCoroutine(WaitSecond("LevelSelection"));
        // SceneManager.LoadScene("LevelSelection");
    }

    public void manual()
    {
        StartCoroutine(WaitSecond("Instruction"));
        // SceneManager.LoadScene("Instruction");
    }


    public void back()
    {
        StartCoroutine(WaitSecond("MainMenu"));
        // SceneManager.LoadScene("MainMenu");
    }

    public void level1()
    {
        StartCoroutine(WaitSecond("Level_1"));
        // SceneManager.LoadScene("Level_1");
    }

    public void level2()
    {
        StartCoroutine(WaitSecond("Level_2"));
        // SceneManager.LoadScene("Level_2");
    }

    public void level3()
    {
        StartCoroutine(WaitSecond("Level_3"));
    }

    public void level4()
    {
        StartCoroutine(WaitSecond("Level_4"));
    }

    public void level5()
    {
        StartCoroutine(WaitSecond("Level_5"));
    }

    public void credit()
    {
        StartCoroutine(WaitSecond("EndScene"));
    }

    public void quit()
    {
        Application.Quit();
    }

    private IEnumerator WaitSecond(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);
    }

    public void restartGame()
    {
        StartCoroutine(WaitSecond("MainMenu"));
    }
}
