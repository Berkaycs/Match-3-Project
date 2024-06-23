using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private string LevelName;
    public int CurrentLevel;

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void UnlockedLevel()
    {
        int nextLevel = CurrentLevel + 1;
        LevelName = "Level " + nextLevel;

        SceneManager.LoadScene(LevelName);
    }

    public void Retry()
    {
        LevelName = "Level " + CurrentLevel;
        SceneManager.LoadScene(LevelName);
    }
}