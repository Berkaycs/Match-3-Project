using System.Collections;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Database;

public class LeaderboardManager : MonoBehaviour
{
    public Transform PlayerInfoContainer;
    public GameObject PlayerInfoTemplate;
    public GameObject Leaderboard;
    public GameObject Buttons;

    private FirebaseManager _firebase;

    //public FirestoreManager Firestore;

    public int ChildWidth = 850;
    public int ChildHeight = 150;

    private void Awake()
    {
        Leaderboard.SetActive(false);
    }

    private void Start()
    {
        _firebase = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();

        StartCoroutine(LoadLeaderboard());
    }

    private IEnumerator LoadLeaderboard()
    {
        var DBTask = _firebase.Database.Child("users").OrderByChild("Score").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.Log(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            // Clear existing entries
            foreach (Transform child in PlayerInfoContainer)
            {
                Destroy(child.gameObject);
            }

            int rank = 1;

            // Populate leaderboard with fetched data
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                GameObject temp = Instantiate(PlayerInfoTemplate);
                temp.transform.SetParent(PlayerInfoContainer);

                RectTransform rectTransform = temp.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(ChildWidth, ChildHeight);

                // Set entry data
                temp.transform.Find("RankText").GetComponent<TextMeshProUGUI>().text = rank.ToString();
                temp.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = childSnapshot.Child("Username").Value.ToString();
                temp.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = childSnapshot.Child("Score").Value.ToString();

                rank++;
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