using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

    #region Fields

    [Header("Audio Config")]
    [SerializeField]
    private AudioConfig audioConfig;

    private AudioSource bgmSource;

    private AudioSource sfxSource;

    #endregion


    #region Properties

    public static AudioManager Instance { get; private set; }

    #endregion


    #region Methods

    public void PlayBGM()
    {
        if (!SettingsPopup.IsBGMEnabled() || audioConfig.BgmClip == null)
        {
            return;
        }

        if (bgmSource.isPlaying)
        {
            return; // already playing, don't restart
        }

        bgmSource.Play();
    }

    public void PlayButtonSFX()
    {
        PlaySFX(audioConfig.ButtonClickClip);
    }

    public void PlayPlacementSFX()
    {
        PlaySFX(audioConfig.PlacementClip);
    }

    public void PlayPopupSFX()
    {
        PlaySFX(audioConfig.PopupClip);
    }

    public void PlayWinSFX()
    {
        PlaySFX(audioConfig.WinClip);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        SetupAudioSources();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (!SettingsPopup.IsSFXEnabled() || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    private void SetupAudioSources()
    {
        // First AudioSource on the GameObject is for music.
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.clip = audioConfig.BgmClip;

        // Second AudioSource added dynamically for SFX.
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayBGM();
    }

    #endregion

}