using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private RegistrationLoginManager login;
    private int lockedLevel = 2;
    private int nextLevel;
    private string LevelName;
    public int CurrentLevel;

    private void Awake()
    {
        login = GameObject.Find("LoginManager").GetComponent<RegistrationLoginManager>();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void UnlockedLevel()
    {
        StartCoroutine(UpdateLevel());
    }

    public void Retry()
    {
        LevelName = "Level " + CurrentLevel;
        Debug.Log(LevelName);
        SceneManager.LoadScene(LevelName);
    }

    IEnumerator UpdateLevel()
    {
        // Escape the username to ensure it's URL-safe
        string username = UnityWebRequest.EscapeURL(login.loggedInUsername);

        // Construct the URL with the username as a parameter
        string url = "http://127.0.0.1/match3/updatelevel.php?user_name=" + username;

        // Send the GET request to the PHP script
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Get the response text containing the new level data
                string json = request.downloadHandler.text;
                Debug.Log(json);

                // Parse the JSON response to extract the new level
                UserDataList userData = JsonUtility.FromJson<UserDataList>("{\"entries\":" + json + "}");

                // Access the new level from the response
                lockedLevel = userData.entries[0].Level + 1;

                nextLevel = CurrentLevel + 1;

                if (nextLevel <= lockedLevel)
                {
                    LevelName = "Level " + nextLevel;                   
                }
                else
                {
                    LevelName = "Level " + lockedLevel;                   
                }

                SceneManager.LoadScene(LevelName);

                // Use the new level as needed in your game logic
                Debug.Log("Locked level: " + lockedLevel);
            }
            else
            {
                Debug.LogError("Failed to update level: " + request.error);
            }
        }
    }
}
