using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip select, pop, repop, completed, gameOver, notAllowed, swap, goal;

    private void Awake()
    {
        instance = this;
    }
    public void PlaySelectSound()
    {
        audioSource.clip = select;
        audioSource.Play();
    }
    public void PlayPopSound()
    {
        audioSource.clip = pop;
        audioSource.Play();
    }
    public void PlayRepopSound()
    {
        audioSource.clip = repop;
        audioSource.Play();
    }
    public void PlayCompletedSound()
    {
        audioSource.clip = completed;
        audioSource.Play();
    }
    public void PlayGameOverSound()
    {
        audioSource.clip = gameOver;
        audioSource.Play();
    }
    public void PlayNotallowedSound()
    {
        audioSource.clip = notAllowed;
        audioSource.Play();
    }
    public void PlaySwapSound()
    {
        audioSource.clip = swap;
        audioSource.Play();
    }
    public void PlayGoalSound()
    {
        audioSource.clip = goal;
        audioSource.Play();
    }
}
