using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
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

    [SerializeField] private Image _icon;

    [SerializeField] private Item[] _itemList;

    private int _moveCount;
    private int _goalAmount;
    private int _itemIndex;
    public int Score;

    private void Awake()
    {
        Instance = this;
        _completePanel.SetActive(false);
        _failedPanel.SetActive(false);
        _buttons.SetActive(true);

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

        _goalAmount = UnityEngine.Random.Range(10, 20);
        _goalText.text = ": " + _goalAmount.ToString();

        _itemIndex = UnityEngine.Random.Range(0, _itemList.Length);
        _icon.sprite = _itemList[_itemIndex].Sprite;

        OnLevelCompleted += GameManager_OnLevelCompleted;
        OnLevelFailed += GameManager_OnLevelFailed;
    }

    public void GameManager_OnTileSwappedScore(object sender, Board.OnTileSwappedScoreEventArgs e)
    {
        _itemList[_itemIndex].Value = 15;
        Score += Board.Instance.ValueOfItem * e.Multiplier;

        _scoreText.text = 0.ToString();
        _scoreText.text = Score.ToString();
        //DOTween.To(() => _scoreText.text, x => _scoreText.text = x, Score.ToString(), 0.5f);
    }

    public void GameManager_OnLevelCompleted(object sender, EventArgs e)
    {
        StartCoroutine(_gameMenu.UpdateLevel());
        StartCoroutine(_gameMenu.UpdateScore());
        _completePanel.SetActive(true);
        _buttons.SetActive(false);
    }

    private void GameManager_OnLevelFailed(object sender, EventArgs e)
    {
        _failedPanel.SetActive(true);
        _buttons.SetActive(false);
    }

    public void GameManager_OnTileSwappedMove(object sender, EventArgs e)
    {
        _moveCount--;
        if (_moveCount < 0)
        {
            _moveCount = 0;
        }

        if (_moveCount == 0 && _goalAmount > 0)
        {
            OnLevelFailed?.Invoke(this, EventArgs.Empty);
        }
        _moveText.text = _moveCount.ToString();
    }

    public void GameManager_OnGoalFruitPopped(object sender, EventArgs e)
    {
        if (_itemIndex == Board.Instance.IndexOfItem)
        {
            _goalAmount -= Board.Instance.PoppedCount;
            if(_goalAmount <= 0)
            {
                _goalAmount = 0;
                OnLevelCompleted?.Invoke(this, EventArgs.Empty);
            }
            _goalText.text = ": " + _goalAmount.ToString();
        }
        
    }
}
