using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip musicOnStart;
    [SerializeField]
    AudioClip musicOnDeath;

    AudioSource audioSource;

    bool isDead = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.clip = musicOnStart;
        audioSource.Play();
    }

    private void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.GameOver && isDead == false)
        {
            isDead = true;
            audioSource.Stop();
            audioSource.PlayOneShot(musicOnDeath);
        }
    }
}
