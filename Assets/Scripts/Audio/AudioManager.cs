using UnityEngine;

public enum Sfx
{
    Click,
    Spawn,
    Merge,
    Error,
    QuestDone,
    Win,
    Lose
}

// Plays music and sound effects with the volumes from GameSettings.
// Created automatically from Resources/AudioManager (assign the clips there);
// any clip left empty is simply not played.
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField] private AudioClip _music;

    [Header("Sound effects")]
    [SerializeField]
    private AudioClip _click;

    [SerializeField] private AudioClip _spawn;

    [SerializeField] private AudioClip _merge;

    [SerializeField] private AudioClip _error;

    [SerializeField] private AudioClip _questDone;

    [SerializeField] private AudioClip _win;

    [SerializeField] private AudioClip _lose;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        _instance = this;
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        ApplyVolumes();
        GameSettings.OnChanged += ApplyVolumes;

        if (_music != null)
        {
            musicSource.clip = _music;
            musicSource.Play();
        }
    }

    private void OnDestroy()
    {
        GameSettings.OnChanged -= ApplyVolumes;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        var prefab = Resources.Load<AudioManager>("AudioManager");
        var manager = prefab != null ? Instantiate(prefab) : new GameObject().AddComponent<AudioManager>();
        manager.name = "AudioManager";
        DontDestroyOnLoad(manager.gameObject);
    }

    private void ApplyVolumes()
    {
        musicSource.volume = GameSettings.MusicVolume;
        sfxSource.volume = GameSettings.SfxVolume;
    }

    public static void Play(Sfx sfx)
    {
        if (_instance == null)
            return;

        var clip = _instance.ClipFor(sfx);
        if (clip != null)
            _instance.sfxSource.PlayOneShot(clip);
    }

    public static void Vibrate()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (GameSettings.Vibration)
            Handheld.Vibrate();
#endif
    }

    private AudioClip ClipFor(Sfx sfx)
    {
        switch (sfx)
        {
            case Sfx.Click: return _click;
            case Sfx.Spawn: return _spawn;
            case Sfx.Merge: return _merge;
            case Sfx.Error: return _error;
            case Sfx.QuestDone: return _questDone;
            case Sfx.Win: return _win;
            case Sfx.Lose: return _lose;
            default: return null;
        }
    }
}