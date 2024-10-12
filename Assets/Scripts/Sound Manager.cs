using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource sfxSrc;
    public AudioClip deathSound;
    public AudioClip respawnSound;
    public AudioClip levelCompleteSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            sfxSrc = gameObject.AddComponent<AudioSource>();
            sfxSrc.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is not assigned");
            return;
        }

        sfxSrc.PlayOneShot(clip);
    }

    public void PlayDeathSound() { PlayOneShot(deathSound); }
    public void PlayRespawnSound() { PlayOneShot(respawnSound); }
    public void PlayLevelCompleteSound() { PlayOneShot(levelCompleteSound); }
}
