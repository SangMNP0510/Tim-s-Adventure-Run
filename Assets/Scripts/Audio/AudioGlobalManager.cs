using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioGlobalManager : MonoBehaviour
{
    public static AudioGlobalManager Instance;

    [System.Serializable]
    public class MusicData
    {
        public AudioClip clip;
        public List<string> scenesAllowed;
    }

    [Header("Music List")]
    public List<MusicData> musics;

    private AudioSource musicSource;
    private bool isSoundOn;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = GetComponent<AudioSource>();

        isSoundOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        ApplyVolume();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic(scene.name);
    }

    void UpdateMusic(string sceneName)
    {
        foreach (var music in musics)
        {
            if (music.scenesAllowed.Contains(sceneName))
            {
                if (musicSource.clip != music.clip || !musicSource.isPlaying)
                {
                    musicSource.clip = music.clip;
                    musicSource.loop = true;
                    musicSource.Play();
                }
                return;
            }
        }

        musicSource.Stop();
    }

    public void SetSound(bool state)
    {
        isSoundOn = state;
        PlayerPrefs.SetInt("Sound", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplyVolume();
    }

    public bool GetSoundState()
    {
        return isSoundOn;
    }

    void ApplyVolume()
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}