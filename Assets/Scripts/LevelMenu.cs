using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;  // For ContinueWithOnMainThread

public class LevelMenu : MonoBehaviour
{
    public Button[] Buttons;
    public int PlayerLevel;
    private FirebaseManager _firebase;

    private void Awake()
    {
        _firebase = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();

        //gameObject.SetActive(false);

        // Disable the buttons initially
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
    }

    private void Start()
    {
        // Fetch player level from Firestore
        StartCoroutine(GetLevel());
    }

    IEnumerator GetLevel()
    {
        // Fetch the player level from the database
        var DBTask = _firebase.Database.Child("users").Child(_firebase.User.UserId).Child("Level").GetValueAsync();

        // Wait for the task to complete
        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogError($"Failed to fetch player level: {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            // Check if the snapshot contains data
            if (snapshot.Exists)
            {
                PlayerLevel = int.Parse(snapshot.Value.ToString());

                // Enable buttons based on player level
                for (int i = 0; i < Buttons.Length; i++)
                {
                    if (i < PlayerLevel)
                    {
                        Buttons[i].interactable = true;
                    }
                    else
                    {
                        Buttons[i].interactable = false;
                    }
                }

                gameObject.SetActive(true); // Activate the game object now that buttons are set
            }
            else
            {
                Debug.LogWarning("Player level data not found.");
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


