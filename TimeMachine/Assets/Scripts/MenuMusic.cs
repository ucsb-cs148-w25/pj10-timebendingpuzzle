using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip highlightMusic;
    [SerializeField] private AudioClip selectMusic;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHighlight()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = highlightMusic;
        audioSource.Play();
        Debug.Log("Highlight");
    }

    public void PlaySelect()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = selectMusic;
        audioSource.Play();
        Debug.Log("Select");
    }
    
}
