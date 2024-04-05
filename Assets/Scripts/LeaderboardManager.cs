using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    IEnumerator GetLeaderboard()
    {

        // Create URL with user credentials
        string leaderboardURL = "http://127.0.0.1/match3/leaderboard.php?user_name=";

        using (UnityWebRequest request = UnityWebRequest.Get(leaderboardURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;

                if (response == "TRUE")
                {

                }

                else
                {

                }
            }

            else
            {
                Debug.LogError("Login request failed " + request.error);
            }
        }
    }
}
