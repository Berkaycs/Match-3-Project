using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private string _levelName;
    public int CurrentLevel;

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void UnlockedLevel()
    {
        int nextLevel = CurrentLevel + 1;
        _levelName = "Level " + nextLevel;

        SceneManager.LoadScene(_levelName);
    }

    public void Retry()
    {
        _levelName = "Level " + CurrentLevel;
        SceneManager.LoadScene(_levelName);
    }
}