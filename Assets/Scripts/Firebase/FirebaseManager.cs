using System.Collections;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    public TMP_InputField RegMailInput;
    public TMP_InputField RegUserInput;
    public TMP_InputField RegPasswordInput;
    public TMP_InputField LogMailInput;
    public TMP_InputField LogPasswordInput;
    public TextMeshProUGUI RegErrorText;
    public TextMeshProUGUI LogErrorText;

    public GameObject RegPanel;
    public GameObject LogPanel;

    public FirebaseUser User;
    public DatabaseReference Database;

    private FirebaseAuth _auth;
    private DependencyStatus _dependencyStatus;

    private void Awake()
    {
        RegPanel.gameObject.SetActive(true);
        LogPanel.gameObject.SetActive(false);
        RegErrorText.gameObject.SetActive(false);
        LogErrorText.gameObject.SetActive(false);       

        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            _dependencyStatus = task.Result;
            if (_dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependencies: " + _dependencyStatus);
            }
        });
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        _auth = FirebaseAuth.DefaultInstance;
        Database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void RegisterUser()
    {
        StartCoroutine(Register(RegMailInput.text ,RegUserInput.text, RegPasswordInput.text));
    }

    public void LoginUser()
    {
        StartCoroutine(Login(LogMailInput.text, LogPasswordInput.text));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HaveAccount()
    {
        RegPanel.gameObject.SetActive(false);
        LogPanel.gameObject.SetActive(true);
    }

    public void DontHaveAccount()
    {
        RegPanel.gameObject.SetActive(true);
        LogPanel.gameObject.SetActive(false);
    }

    // Register User
    public IEnumerator Register(string email, string username, string password)
    {
        // Create user with email and password
        var createUserTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => createUserTask.IsCompleted);

        if (createUserTask.Exception != null)
        {
            Debug.LogError($"User registration failed: {createUserTask.Exception}");
            RegErrorText.gameObject.SetActive(true);
            yield break;
        }

        var result = createUserTask.Result;
        User = result.User;

        if (User != null)
        {
            RegPanel.gameObject.SetActive(false);
            LogPanel.gameObject.SetActive(true);

            // Update user profile with username
            UserProfile profile = new UserProfile { DisplayName = username };
            var updateProfileTask = User.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(() => updateProfileTask.IsCompleted);

            if (updateProfileTask.Exception != null)
            {
                Debug.LogError($"Failed to set username: {updateProfileTask.Exception}");
            }
            else
            {
                StartCoroutine(SetInitialUserValues(username));
                Debug.Log("Username is now set");
            }
        }
        else
        {
            Debug.LogError("User registration failed: user is null.");
        }
    }

    // Login User
    public IEnumerator Login(string email, string password)
    {
        // Initiate the login process
        var loginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);

        // Wait for the task to complete
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            // Log the error if the task fails
            Debug.LogError($"Login failed: {loginTask.Exception}");
            LogErrorText.gameObject.SetActive(true);
        }
        else
        {
            // Retrieve the result and log the user email if successful
            AuthResult result = loginTask.Result;
            User = result.User;
            Debug.Log($"User logged in: {User.Email}");

            LogErrorText.gameObject.SetActive(false);

            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator SetInitialUserValues(string username)
    {
        var userValues = new Dictionary<string, object>
    {
        { "Username", username },
        { "Score", 0 },
        { "Level", 1 },
    };

        var DBTask = Database.Child("users").Child(User.UserId).SetValueAsync(userValues);
        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogError($"Failed to set initial user values: {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Initial user values set successfully");
        }
    }

    public void UpdateInfo()
    {
        StartCoroutine(UpdateLevelAndScore(GameManager.Instance.Score));
    }

    private IEnumerator UpdateLevelAndScore(int newScore)
    {
        string userId = User?.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID is null or empty.");
            yield break;
        }

        Debug.Log($"Updating level and score for user: {User.DisplayName}");

        var getLevelTask = Database.Child("users").Child(userId).Child("Level").GetValueAsync();
        var getScoreTask = Database.Child("users").Child(userId).Child("Score").GetValueAsync();

        yield return new WaitUntil(() => getLevelTask.IsCompleted && getScoreTask.IsCompleted);

        if (getLevelTask.Exception != null || getScoreTask.Exception != null)
        {
            if (getLevelTask.Exception != null)
                Debug.LogError($"Failed to retrieve current level: {getLevelTask.Exception}");

            if (getScoreTask.Exception != null)
                Debug.LogError($"Failed to retrieve current score: {getScoreTask.Exception}");

            yield break;
        }

        if (getLevelTask.Result.Exists && getScoreTask.Result.Exists)
        {
            int currentLevel = int.Parse(getLevelTask.Result.Value.ToString());
            int newLevel = currentLevel + 1;

            if (newLevel > 5)
            {
                Debug.Log("Level cannot exceed 5. Not updating level.");
                newLevel = currentLevel;
            }

            var updateLevelTask = Database.Child("users").Child(userId).Child("Level").SetValueAsync(newLevel);

            int currentScore = int.Parse(getScoreTask.Result.Value.ToString());
            int updatedScore = currentScore + newScore;

            var updateScoreTask = Database.Child("users").Child(userId).Child("Score").SetValueAsync(updatedScore);

            yield return new WaitUntil(() => updateLevelTask.IsCompleted && updateScoreTask.IsCompleted);

            if (updateLevelTask.Exception != null || updateScoreTask.Exception != null)
            {
                if (updateLevelTask.Exception != null)
                    Debug.LogError($"Failed to update level: {updateLevelTask.Exception}");

                if (updateScoreTask.Exception != null)
                    Debug.LogError($"Failed to update score: {updateScoreTask.Exception}");
            }
            else
            {
                Debug.Log("Level and score updated successfully");
            }
        }
        else
        {
            if (!getLevelTask.Result.Exists)
                Debug.LogError("Current level does not exist in the database.");

            if (!getScoreTask.Result.Exists)
                Debug.LogError("Current score does not exist in the database.");
        }
    }
}

