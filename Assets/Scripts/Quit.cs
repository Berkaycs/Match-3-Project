using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    private Button _quitButton;

    private void Awake()
    {
        _quitButton = GetComponent<Button>();
    }

    private void Start()
    {
        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
