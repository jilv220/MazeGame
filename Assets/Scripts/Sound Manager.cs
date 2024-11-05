using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    GameObject soundManagerObj = new("SoundManager");
                    _instance = soundManagerObj.AddComponent<SoundManager>();
                    DontDestroyOnLoad(soundManagerObj);
                }
            }
            return _instance;
        }
    }

    private AudioSource sfxSrc;
    private readonly string SFXPath = "SFX";
    private Dictionary<string, AudioClip> audioClipCache = new();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayOneShot(string audioClipBasename)
    {
        if (!audioClipCache.TryGetValue(audioClipBasename, out AudioClip clip))
        {
            var audioClipPath = $"{SFXPath}/{audioClipBasename}";
            clip = Resources.Load<AudioClip>(audioClipPath);
            if (clip == null)
            {
                Debug.LogWarning($"Failed to load AudioClip: {audioClipPath}");
                return;
            }
            audioClipCache[audioClipBasename] = clip;
        }

        if (sfxSrc == null)
        {
            sfxSrc = gameObject.AddComponent<AudioSource>();
            sfxSrc.playOnAwake = false;
        }

        sfxSrc.PlayOneShot(clip);
    }

    public void ClearAudioCache()
    {
        audioClipCache.Clear();
        Resources.UnloadUnusedAssets();
    }
}
