using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] Buttons;
    private RegistrationLoginManager login;

    private void Awake()
    {
        login = GameObject.Find("LoginManager").GetComponent<RegistrationLoginManager>();

        gameObject.SetActive(false);    

        // Disable the buttons initially
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
    }

    private void Start()
    {
        // Fetch player level from the server
        StartCoroutine(GetLevel());
    }

    IEnumerator GetLevel()
    {
        string username = UnityWebRequest.EscapeURL(login.loggedInUsername);
        Debug.Log(username);
        string url = "http://127.0.0.1/match3/userlevel.php?user_name=" + username;
        Debug.Log(url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log(json);

                UserDataList userData = JsonUtility.FromJson<UserDataList>("{\"entries\":" + json + "}");

                if (userData.entries.Length > 0)
                {
                    int unlockedLevel = userData.entries[0].Level;

                    for (int i = 0; i < unlockedLevel; i++)
                    {
                        Buttons[i].interactable = true;
                    }
                }
                else
                {
                    Debug.Log("There is no entry");
                }
            }
            else
            {
                Debug.LogError("Failed to fetch player level: " + request.error);
            }
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
