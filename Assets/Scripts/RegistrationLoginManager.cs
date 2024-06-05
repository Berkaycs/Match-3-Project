using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RegistrationLoginManager : MonoBehaviour
{
    public TMP_InputField regUserInput;
    public TMP_InputField regPasswordInput;
    public TMP_InputField logUserInput;
    public TMP_InputField logPasswordInput;
    public TextMeshProUGUI regErrorText;
    public TextMeshProUGUI logErrorText;

    public GameObject RegPanel;
    public GameObject LogPanel;

    public string loggedInUsername;

    private void Awake()
    {
        RegPanel.gameObject.SetActive(true);
        LogPanel.gameObject.SetActive(false);
        regErrorText.gameObject.SetActive(false);
        logErrorText.gameObject.SetActive(false);
    }

    public void RegisterUser()
    {
        StartCoroutine(Registration());
    }

    public void LoginUser()
    {
        StartCoroutine(Login());
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

    IEnumerator Registration()
    {
        // Sanitize inputs
        string userName = UnityWebRequest.EscapeURL(regUserInput.text);
        string password = UnityWebRequest.EscapeURL(regPasswordInput.text);

        
        // Create form data for registration
        WWWForm form = new WWWForm();
        form.AddField("user_name", userName);
        form.AddField("user_password", password);

        // "{\"user_name\":\"" + userName + "\", \"user_password\":\"" + password + "\"}"

        using (UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1/match3/register.php", form))
        {
            yield return request.SendWebRequest();

            Debug.Log("Response Code: " + request.responseCode);
            Debug.Log("Result: " + request.result);
            Debug.Log("Error: " + request.error);
            Debug.Log("Response: " + request.downloadHandler.text);
            Debug.Log("Raw Data: " + System.Text.Encoding.UTF8.GetString(request.downloadHandler.data));

            if (request.result == UnityWebRequest.Result.Success)
            {
                string errorMessage = request.downloadHandler.text;
                if (errorMessage.Contains("Username already taken")) // You should replace this condition with the actual error message indicating username is already taken
                {
                    regErrorText.gameObject.SetActive(true); // Display error message
                }
                else
                {
                    RegPanel.gameObject.SetActive(false);
                    LogPanel.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("Login request failed " + request.error);
            }
        }
    }

    IEnumerator Login()
    {
        DontDestroyOnLoad(gameObject);

        string userName = UnityWebRequest.EscapeURL(logUserInput.text);
        string password = UnityWebRequest.EscapeURL(logPasswordInput.text);

        // Create URL with user credentials
        string loginURL = "http://127.0.0.1/match3/login.php?user_name=" + userName + "&user_password=" + password;

        using (UnityWebRequest request = UnityWebRequest.Get(loginURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                
                if (response == "TRUE")
                {
                    loggedInUsername = userName;
                    SceneManager.LoadScene(1);
                    logErrorText.gameObject.SetActive(false);
                }

                else
                {
                    Debug.LogError("Login failed");
                    logErrorText.gameObject.SetActive(true);
                }
            }

            else
            {
                Debug.LogError("Login request failed " + request.error);
            }
        }
    }
}
