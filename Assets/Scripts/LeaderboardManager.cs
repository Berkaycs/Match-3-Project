using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    public Transform PlayerInfoContainer;
    public GameObject PlayerInfoTemplate;
    public GameObject Leaderboard;
    public GameObject Buttons;

    public int ChildWidth = 850;
    public int ChildHeight = 150;

    private void Awake()
    {
        Leaderboard.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(GetLeaderboard());
    }

    IEnumerator GetLeaderboard()
    {
        // Create URL with user credentials
        string leaderboardURL = "http://127.0.0.1/match3/leaderboard.php";

        using (UnityWebRequest request = UnityWebRequest.Get(leaderboardURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log(json);
                //List<LeaderboardEntry> leaderboardData = JsonUtility.FromJson<List<LeaderboardEntry>>(json);
                LeaderboardList leaderboardData = JsonUtility.FromJson<LeaderboardList>("{\"entries\":" + json + "}");

                // Clear existing entries
                foreach (Transform child in PlayerInfoContainer)
                {
                    Destroy(child.gameObject);
                }

                // Populate leaderboard with fetched data
                int rank = 1;
                foreach (LeaderboardEntry entry in leaderboardData.entries)
                {
                    GameObject temp = Instantiate(PlayerInfoTemplate);
                    temp.transform.SetParent(PlayerInfoContainer.transform);

                    RectTransform rectTransform = temp.GetComponent<RectTransform>();

                    // Set size
                    rectTransform.sizeDelta = new Vector2(ChildWidth, ChildHeight);

                    // Set entry data
                    temp.transform.Find("RankText").GetComponent<TextMeshProUGUI>().text = rank.ToString();
                    temp.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = entry.UserName;
                    temp.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = entry.Score.ToString();

                    rank++;
                }              
            }

            else
            {
                Debug.LogError("Login request failed " + request.error);
            }
        }
    }

    public void Show()
    {
        Leaderboard.SetActive(true);
        Buttons.SetActive(false);
    }

    public void Close()
    {
        Leaderboard.SetActive(false);
        Buttons.SetActive(true);
    }
}
