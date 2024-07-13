using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _select, _pop, _repop, _completed, _gameOver, _notAllowed, _swap, _goal;

    private void Awake()
    {
        Instance = this;
    }
    public void PlaySelectSound()
    {
        _audioSource.clip = _select;
        _audioSource.Play();
    }
    public void PlayPopSound()
    {
        _audioSource.clip = _pop;
        _audioSource.Play();
    }
    public void PlayRepopSound()
    {
        _audioSource.clip = _repop;
        _audioSource.Play();
    }
    public void PlayCompletedSound()
    {
        _audioSource.clip = _completed;
        _audioSource.Play();
    }
    public void PlayGameOverSound()
    {
        _audioSource.clip = _gameOver;
        _audioSource.Play();
    }
    public void PlayNotallowedSound()
    {
        _audioSource.clip = _notAllowed;
        _audioSource.Play();
    }
    public void PlaySwapSound()
    {
        _audioSource.clip = _swap;
        _audioSource.Play();
    }
    public void PlayGoalSound()
    {
        _audioSource.clip = _goal;
        _audioSource.Play();
    }
}
