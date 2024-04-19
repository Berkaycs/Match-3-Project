using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public UserData UserData;
    public string LevelName;
    public void UnlockedLevel()
    {
        UserData.Level++;
        LevelName = "Level " + UserData.Level;
        SceneManager.LoadScene(LevelName);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Level " + UserData.Level);
    }
}
