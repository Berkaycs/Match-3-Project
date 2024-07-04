using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnLevelCompleted;
    public event EventHandler OnLevelFailed;

    [SerializeField] private TextMeshProUGUI _moveText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _goalText;

    [SerializeField] private GameObject _completePanel;
    [SerializeField] private GameObject _failedPanel;
    [SerializeField] private GameObject _buttons;

    [SerializeField] private GameMenu _gameMenu;

    private FirebaseManager _firebase;

    [SerializeField] private Image _icon;

    [SerializeField] private Item[] _itemList;

    public GameObject BackgroundMusic;

    private int _moveCount;
    private int _goalAmount;
    public int ItemIndex;
    public int Score;

    private void Awake()
    {
        Instance = this;
        _firebase = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();

        _completePanel.SetActive(false);
        _failedPanel.SetActive(false);
        _buttons.SetActive(true);
        BackgroundMusic.SetActive(true);

        for (int x = 0; x < _itemList.Length; x++)
        {
            _itemList[x].Value = 5;
        }
    }

    private void Start()
    {
        _moveCount = UnityEngine.Random.Range(10, 20);
        _moveText.text = _moveCount.ToString();

        _scoreText.text = Score.ToString();

        _goalAmount = UnityEngine.Random.Range(7,14);
        _goalText.text = ": " + _goalAmount.ToString();

        ItemIndex = UnityEngine.Random.Range(0, _itemList.Length);
        _icon.sprite = _itemList[ItemIndex].Sprite;

        OnLevelCompleted += GameManager_OnLevelCompleted;
        OnLevelFailed += GameManager_OnLevelFailed;
    }

    public void GameManager_OnTileSwappedScore(object sender, Board.OnTileSwappedScoreEventArgs e)
    {
        _itemList[ItemIndex].Value = 15;
        Score += Board.Instance.ValueOfItem * e.Multiplier;

        Debug.Log(Score);

        _scoreText.text = 0.ToString();
        _scoreText.text = Score.ToString();
    }

    public void GameManager_OnLevelCompleted(object sender, EventArgs e)
    {
        _firebase.UpdateInfo();
        _completePanel.SetActive(true);
        BackgroundMusic.SetActive(false);
        _buttons.SetActive(false);
    }

    private void GameManager_OnLevelFailed(object sender, EventArgs e)
    {
        _failedPanel.SetActive(true);
        _buttons.SetActive(false);
        BackgroundMusic.SetActive(false);
        AudioManager.instance.PlayGameOverSound();
    }

    public void GameManager_OnTileSwappedMove(object sender, EventArgs e)
    {
        _moveCount--;
        if (_moveCount < 0)
        {
            _moveCount = 0;
        }
        _moveText.text = _moveCount.ToString();
    }

    public void GameManager_OnGoalFruitPopped(object sender, EventArgs e)
    {
        if (ItemIndex == Board.Instance.IndexOfItem)
        {
            _goalAmount -= Board.Instance.PoppedCount;
            if (_goalAmount <= 0)
            {
                _goalAmount = 0;
            }
            _goalText.text = _goalAmount.ToString();
            AudioManager.instance.PlayGoalSound();
        }
        
    }

    public void CheckGoalAmount()
    {

        if (_moveCount == 0 && _goalAmount > 0)
        {
            OnLevelFailed?.Invoke(this, EventArgs.Empty);

        }
        if (_goalAmount <= 0)
        {
            _goalAmount = 0;
            OnLevelCompleted?.Invoke(this, EventArgs.Empty);
            AudioManager.instance.PlayCompletedSound();
        }
    }
}
