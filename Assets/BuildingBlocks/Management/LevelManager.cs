using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string GetCurrSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string levelName)
    {
        Debug.Log("Loading level " + levelName);
        SceneManager.LoadScene(levelName);
    }

    public int GetBuildIdxFromSceneName(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
